using IncidentReportModel;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WsiWebHelpers;
using System.Xml;
using System.Globalization;
using System.Web.Script.Serialization;
using OdpMDataAccess;

namespace OnlineServices
{
    public partial class IREntry : System.Web.UI.Page
    {

        //Private m_pobList As PartOfBodyList
        private OnlineServicesModel.PartOfBodyList m_pobList;

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            // account # and company name required - if not provided on querystring then redirect to select page.
            if (Session.Contents["employer_act_no"] == null || Session.Contents["employer_name"] == null)
            {
                Response.Redirect("IRCompanySelect.aspx");
            }
            else
            {
                setupLinks();

                lblAccountNumber.Text = Session.Contents["employer_act_no"].ToString();
                lblBusinessName.Text = Session.Contents["employer_name"].ToString();

                if (!IsPostBack)
                {
                    setCompanyUserInfo();                    // if company info is stored in session cache then prefill those fields
                    populateNatureOfInjuryOptions();         // get the Nature of Injury values from database and populate dropdown
                    startMsUser();                           // starts a new FROI number, creates master_status row, and puts value in session
                }
            }
            forceUpdateXml();
            xmlLastUpdated();
        }



        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            this.Save();
            string responseHtml = this.makeHTML();
            this.HandleWsiAccountEntry(responseHtml);
            this.StopMSuser();


            string reDir = @"IRConfirmation.aspx";
            this.Response.Redirect(reDir);

        }

        private void HandleWsiAccountEntry(string htmlString)
        {
            if (this.Request["employer_act_no"] == "63958")
            {
                string mailClientSmtpServername = ConfigurationManager.AppSettings["SMTPServer"];
                SmtpClient mailclient = new SmtpClient(mailClientSmtpServername);
                MailMessage message = new MailMessage();
                string toAddress = ConfigurationManager.AppSettings["IR_SMTP_MailTo"];
                message.To.Add(toAddress);
                message.Subject = "WSI Incident Reported";
                string fromAddress = ConfigurationManager.AppSettings["IR_SMTP_MailFrom"];
                MailAddress addr = new MailAddress(fromAddress);
                message.From = addr;
                message.Body = htmlString;
                message.IsBodyHtml = true;
                mailclient.Send(message);
            }
        }




        /// <summary>
        ///  Assembles html string using the IRConfirmationDocument.html html template
        ///  and IncidentReportWorker
        /// </summary>
        private string makeHTML()
        {
            string reportResponseHTML = string.Empty;
            string hyphendate = Strings.Right("0" + Conversions.ToString(DateAndTime.Month(DateAndTime.Now)), 2) + "-" + Strings.Right("0" + Conversions.ToString(DateAndTime.Day(DateAndTime.Now)), 2) + "-" + Conversions.ToString(DateAndTime.Year(DateAndTime.Now));
            string reportname = hyphendate + "_" + this.Session.Contents["employer_act_no"].ToString() + "_" + this.Session.Contents["IR_ID"].ToString() + "_FinalIR.htm";
            string reportPath = ConfigurationManager.AppSettings["IR_HtmlReportPath"];
            string finalreportname = string.Format("{0}{1}", reportPath, reportname);
            this.Session.Contents["finalreportname"] = finalreportname;

            string inReceipt = this.Session.Contents["IR_ID"].ToString();
            IncidentReportWorker tw = new IncidentReportWorker(Convert.ToInt32(inReceipt));
            htmTemplateHelper htmHelp = new htmTemplateHelper(MapPath(@"IRConfirmationDocument.html"));

            tw.BodyPartInjuredName = getBodyPartInjured();

            string[] tagValueArr = {"spanFirstName", tw.FirstName, "spanMI", tw.MiddleInitial, "spanLastName", tw.LastName,
                             "spanSsn", tw.SocialSecurityNumber.ToString(), "spanBirthDate", tw.DateOfBirth.ToShortDateString(),
                             "spanDateOfInjury", tw.InjuryDate.ToShortDateString(), "spanBodyPartInjured", tw.BodyPartInjuredName, "spanLocationOfBodyPartInjured", tw.BodyLocationName,
                             "spanNatureOfInjury", tw.NatureOfInjuryName, "spanDescriptionOfAccident", tw.DescriptionOfInjury, "spanBusinessName", tw.EmployerName,
                             "spanSubmittedByName", tw.EmployerSignature, "spanSubmittedDateTime", tw.SubmittedDate.ToShortDateString()};

            htmHelp.ReplaceTags(tagValueArr);
            reportResponseHTML = htmHelp.Template;
            new IncidentReportDataAccess().UpdateInjuredWorkerRecordWithHtml(Conversions.ToInteger(this.Session.Contents["IR_ID"].ToString()), reportResponseHTML);

            //Store the incident report worker in session to retrieve on the confirmation page
            Session.Contents["IRW"] = tw;

            return reportResponseHTML;
        }




        /// <summary>
        ///  Function Parses Json from hiddenBodyPartList and returns
        ///  formatted string
        /// </summary>
        private string getBodyPartInjured()
        {
            string returnStr = "";
            string getBodyParts = hiddenBodyPartList.Value;
            if (getBodyParts != "")
            {
                //string jsonString = @"{""BodyPartsCol"":[{""BodyPartsDrpDwn"":""1st Toe"",""LocationsDrpDwn"":""Right""},{""BodyPartsDrpDwn"":""Ear"",""LocationsDrpDwn"":""Left""},{""BodyPartsDrpDwn"":""Tooth"",""LocationsDrpDwn"":""19th""}]}";
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                OnlineServicesModel.BodyPartsCollection bpCol = serializer.Deserialize<OnlineServicesModel.BodyPartsCollection>(getBodyParts);
                foreach (OnlineServicesModel.BodyPart bp in bpCol.BodyPartsCol)
                {
                    returnStr += bp.LocationsDrpDwn + " " + bp.BodyPartsDrpDwn + ", ";
                }
                returnStr = returnStr.Trim();
                returnStr = returnStr.Trim(',');
            }
            return returnStr;
        }






        private void populateNatureOfInjuryOptions()
        {
            Dictionary<string, string> natureOfInjuryList = new NatureOfInjury("claimSystemData").GetNatureOfInjuryValidValues();
            this.ddlNatureOfInjury.DataSource = natureOfInjuryList;
            this.ddlNatureOfInjury.DataValueField = "Key";
            this.ddlNatureOfInjury.DataTextField = "Value";
            this.ddlNatureOfInjury.DataBind();

        }

        private void Save()
        {
            string bodyField = string.Empty;
            this.Session.Contents["dataissue"] = string.Empty;
            this.Session.Contents["IR_ID"] = this.Hidden1.Value;
            this.Session.Contents["APPLICATION_ID"] = this.Hidden2.Value;
            /*StringBuilder bodyPartDescBuilder = new StringBuilder();
            for (int icount = 1; icount < 25; icount++)
            {
              bodyField = string.Format("part{0}", icount);
              if (this.Request[bodyField] != null)
              {
                bodyPartDescBuilder.Append(this.Request[bodyField].ToString() + ", ");
              }
            }*/

            string bodyPartDesc = getBodyPartInjured();
            /*if (bodyPartDesc.Length > 1)
            {
              bodyPartDesc = bodyPartDesc.Substring(0, bodyPartDesc.Length - 1);  // strip off last comma
              if (bodyPartDesc.Length > 80)
              {
                bodyPartDesc = bodyPartDesc.Substring(0, 80);
              }
            }*/

            this.Session.Contents["employer_signature"] = this.txtYourName.Text;
            this.Session.Contents["employer_name"] = this.lblBusinessName.Text;
            this.Session.Contents["employer_act_no"] = this.lblAccountNumber.Text;

            string natureOfInjuryId = string.Empty;
            string natureOfInjuryDescription = string.Empty;

            if (ddlNatureOfInjury.SelectedItem.Value.Trim() != "-1")
            {
                natureOfInjuryId = this.ddlNatureOfInjury.SelectedItem.Value;
                natureOfInjuryDescription = this.ddlNatureOfInjury.SelectedItem.Text;
            }

            //string bodyLocationText = string.Empty;
            //if (this.Request["body_loc_nm"] != null)
            //{
            //  bodyLocationText = this.Request["body_loc_nm"].ToString();
            //}
            //new IncidentReportDataAccess().AddInjuredWorkerRecord(this.txtFirstName.Text, this.txtMiddleInitial.Text, this.txtLastName.Text, this.txtSsn.Text, bodyPartDesc, bodyLocationText, this.lblBusinessName.Text, this.lblAccountNumber.Text, this.txtYourName.Text, DateAndTime.Now, this.Session.Contents["IR_ID"].ToString(), Convert.ToDateTime(this.txtDoi.Text), Convert.ToDateTime(this.txtBirthDate.Text), natureOfInjuryDescription, this.textAreaDescriptionOfInjury.Value);

            string checkedDescriptionOfInjury = (this.textAreaDescriptionOfInjury.InnerText.Length > 2000) ? this.textAreaDescriptionOfInjury.InnerText.Substring(0, 2000) : this.textAreaDescriptionOfInjury.InnerText;
            new IncidentReportDataAccess().AddInjuredWorkerRecord(this.txtFirstName.Text, this.txtMiddleInitial.Text, this.txtLastName.Text, this.txtSsn.Text, bodyPartDesc, this.lblBusinessName.Text, this.lblAccountNumber.Text, this.txtYourName.Text, DateAndTime.Now, this.Session.Contents["IR_ID"].ToString(), Convert.ToDateTime(this.txtDoi.Text), Convert.ToDateTime(this.txtBirthDate.Text), natureOfInjuryDescription, checkedDescriptionOfInjury);
        }

        private void setCompanyUserInfo()
        {
            if (this.Session.Contents["employer_signature"] != null)
            {
                this.txtYourName.Text = this.Session.Contents["employer_signature"].ToString();
                this.lblBusinessName.Text = this.Session.Contents["employer_name"].ToString();
                this.lblAccountNumber.Text = this.Session.Contents["employer_act_no"].ToString();
            }
        }

        private void setupLinks()
        {
            HtmlHelper.SetupLinkFromConfigForPopupAnchor(linkFroi, "Link_FROI");
        }

        /// <summary>
        ///  gets the next incident report id sequence value, puts it in a session value and 
        ///  then starts the new IR user in the master status table by doing an insert of the keys 
        ///  and the start time.
        /// </summary>
        private void startMsUser()
        {

            IncidentReportDataAccess accessor = new IncidentReportDataAccess();
            int newFroiNumber = accessor.GetNewIncidentSequenceNumber();
            this.Session.Contents["IR_ID"] = newFroiNumber;
            this.Session.Contents["APPLICATION_ID"] = newFroiNumber;
            this.Hidden1.Value = newFroiNumber.ToString();
            this.Hidden2.Value = newFroiNumber.ToString();
            accessor.AddMasterStatus("IR", newFroiNumber.ToString(), DateTime.Now);
            accessor = null;

        }

        public void StopMSuser()
        {
            //this.Session.Contents["employer_act_no"] = null;
            string newAppID = this.Session.Contents["IR_ID"].ToString() + "-" + this.Session.Contents["employer_act_no"].ToString();
            string oldAppId = this.Session.Contents["IR_ID"].ToString();
            new IncidentReportDataAccess().UpdateMasterStatus(newAppID, oldAppId, "IR");
        }

        protected void resetCompany_Click(object sender, EventArgs e)
        {
            Response.Redirect("IRCompanySelect.aspx");
        }












        #region "BODY PART CODE - DP - 08/05/2015"

        /// <summary>
        ///  Function returns string value with the last date the xml file was updated
        /// </summary>
        protected void xmlLastUpdated()
        {
            string xmlLastUpdatedStr = Request.QueryString["xmllastupdated"];
            if (xmlLastUpdatedStr == "true")
            {
                try
                {
                    string colorBPLStr = "red";
                    string path = Server.MapPath("~/App_Data/PartsOfBody.xml");
                    System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
                    xmlDoc.Load(path);
                    XmlNode root = xmlDoc.DocumentElement;
                    string curDateStr = root.Attributes["date"].Value;
                    string[] format = { "MM/dd/yyyy", "M/d/yyyy", "MM-dd-yyyy" };
                    System.DateTime expenddt = default(System.DateTime);
                    System.DateTime.TryParseExact(curDateStr, format, System.Globalization.DateTimeFormatInfo.InvariantInfo, System.Globalization.DateTimeStyles.None, out expenddt);
                    if (expenddt == System.DateTime.Today)
                    {
                        colorBPLStr = "green";
                    }
                    Response.Write("<h1 style=\"color:" + colorBPLStr + ";\">Xml file last updated:" + curDateStr + "</h1>");
                }
                catch (Exception ex)
                {
                    Global.ErrorLogger.LogError(ex, Global.AgencyAbbreviation);
                    Response.Write(ReturnErrorMessage(ex));
                }
            }
        }


        /// <summary>
        ///  Function refreshes data for the body parts xml files
        /// </summary>
        protected void forceUpdateXml()
        {
            string updateXmlStr = Request.QueryString["updatexml"];
            if (updateXmlStr == "true")
            {
                //Boolean updateBodyPartsBool = updateBodyPartsXML();
                //Boolean updateBodyPartLocationsBool = false;


                OnlineServicesModel.BodyPartUpdate BPU = updateBodyPartsXML();



                string updateBPStr = "XML Update failed for Body Parts";
                string colorBPStr = "red";
                string updateBPLStr = "XML Update failed for Body Part Location";
                string colorBPLStr = "red";
                string updateXMLStr = "";


                ////////////////////////////////////////////////////////////
                ///// CURRENTLY THE BODY PART LOCATIONS WON'T GET UPDATED //
                ///// UNLESS THE BODY PART UPDATE SUCCEEDS. I'M NOT SURE ///
                ///// IF THAT SHOULD CHANGE ////////////////////////////////
                if (BPU.success)
                {
                    updateBPStr = "Body Parts XML Updated";
                    colorBPStr = "green";


                    OnlineServicesModel.BodyPartUpdate BPLU = updateBodyPartsLocationXML();
                    if (BPLU.success)
                    {
                        updateBPLStr = "<br>Body Parts Location Updated";
                        colorBPLStr = "green";
                        Session.Remove("bodyPartsOptions");
                    }
                    else
                    {
                        if (BPLU.errMsg != "")
                        {
                            updateBPLStr += BPLU.errMsg;
                        }
                    }


                }
                else
                {
                    if (BPU.errMsg != "")
                    {
                        updateBPStr += "<br>" + BPU.errMsg;
                    }
                }

                updateXMLStr = "<h1 style=\"color:" + colorBPStr + "\">" + updateBPStr + "</h1>";
                updateXMLStr += "<h1 style=\"color:" + colorBPLStr + "\">" + updateBPLStr + "</h1>";
                Response.Write(updateXMLStr);
            }
        }

        /// <summary>
        /// Function returns options for the Body Parts Location dropdown list
        /// dropdownlists
        /// </summary>
        public string returnBodyPartLocationOptions(Boolean updateXml)
        {
            string ReturnStr = "";
            if (Session["bodyPartsLocationOptions"] == null)
            {
                string path = Server.MapPath("~/App_Data/PartsOfBodyLocation.xml");
                try
                {
                    System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
                    xmlDoc.Load(path);
                    XmlNodeList nodeList = default(XmlNodeList);
                    XmlNode root = xmlDoc.DocumentElement;
                    string curDateStr = root.Attributes["date"].Value;

                    if ((updateXml) && (updateTime()))
                    {
                        //Check to see if the date is today's date, if not then update the xml
                        if (!(curDateStr == null))
                        {
                            string[] format = { "MM/dd/yyyy", "M/d/yyyy", "MM-dd-yyyy" };
                            System.DateTime expenddt = default(System.DateTime);
                            System.DateTime.TryParseExact(curDateStr, format, System.Globalization.DateTimeFormatInfo.InvariantInfo, System.Globalization.DateTimeStyles.None, out expenddt);

                            //Setup xml update to run daily DP 08/05/2015
                            if (expenddt < System.DateTime.Today)
                            {



                                ////////////////////////////////////////////////////////////////////
                                ///// BODY PARTS XML FILE UPDATE IS DISABLED UNTIL ISSUES WITH /////
                                ///// ERROR HANDLING ARE FIXED /////////////////////////////////////
                                ///// D.P. 09/22/2015 //////////////////////////////////////////////
                                //The Body Part location xml file hasn't been updated today so run the update
                                xmlBodyPartLocationPnl.Visible = true;
                                OnlineServicesModel.BodyPartUpdate BPLU = updateBodyPartsLocationXML();
                                if (BPLU.success)
                                {
                                    //Update was successful
                                    xmlBPL_fail.Visible = false;
                                    xmlBPL_success.Visible = true;
                                }
                                else
                                {
                                    //Updatefailed
                                    xmlBPL_fail.Visible = true;
                                    xmlBPL_success.Visible = false;
                                    //This is temporary and should be removed when done troubleshooting
                                    //DP 09/23/2015
                                    xmlBPL_fail_Lit.Text = BPLU.errMsg;
                                }
                                ///// BODY PARTS XML FILE UPDATE IS DISABLED UNTIL ISSUES WITH /////
                                ///// ERROR HANDLING ARE FIXED /////////////////////////////////////
                                ///// D.P. 09/22/2015 //////////////////////////////////////////////
                                ////////////////////////////////////////////////////////////////////





                            }
                            else
                            {
                                //Dates matched so do nothing
                                xmlBodyPartLocationPnl.Visible = false;
                            }
                        }


                    }
                    nodeList = root.SelectNodes("Table");

                    ////////////////////////////////////////////////////////////
                    ///////// CREATE THE DATATABLE /////////////////////////////
                    System.Data.DataTable BodyPartsLocDT = returnBodyPartsLocDT();

                    //Iterate xml list and append option values as javascript
                    foreach (System.Xml.XmlNode XmlNode_loopVariable in nodeList)
                    {
                        string pob_cd = "";
                        string pob_nm = "";
                        string pob_void_ind = "";
                        //string pob_opt_loc_ind = "";
                        foreach (System.Xml.XmlNode bodyPartXmlNode in XmlNode_loopVariable)
                        {
                            string bpStr = bodyPartXmlNode.Name;
                            switch (bpStr.ToLower())
                            {
                                case "pob_loc_cd":
                                    pob_cd = bodyPartXmlNode.InnerText;
                                    break;
                                case "pob_loc_nm":
                                    pob_nm = bodyPartXmlNode.InnerText;
                                    break;
                                case "pob_loc_void_ind":
                                    pob_void_ind = bodyPartXmlNode.InnerText;
                                    break;
                            }
                        }

                        //Only show body part locations that have not been voided
                        if (pob_void_ind == "n")
                        {
                            //ReturnStr += "locationTxt += ""<option value='" & pob_cd & "' class='" & pob_opt_loc_ind & "'>" & pob_nm & "</option>"";"
                            if ((BodyPartsLocDT != null))
                            {
                                //Add to temp datatable
                                BodyPartsLocDT.Rows.Add(pob_nm, "", 0, "", pob_cd);
                            }
                        }
                    }

                    /////////////////////////////////////////////////////////////
                    ///////// DP 11/14/ 2014 ///////////////////////////////////
                    if ((BodyPartsLocDT != null))
                    {
                        BodyPartsLocDT = returnUpdatedDT(BodyPartsLocDT);
                        System.Data.DataView dv = new System.Data.DataView(BodyPartsLocDT);
                        dv.Sort = "ints";

                        /////////////////////////////////////////////////////////////
                        ///////// DP 07/24/ 2014 ////////////////////////////////////
                        ///////// -Converting data to Json object ///////////////////
                        System.Data.DataTable dtToDictionary = dv.ToTable();
                        System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                        List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                        Dictionary<string, object> row;

                        foreach (System.Data.DataRow dr in dtToDictionary.Rows)
                        {
                            row = new Dictionary<string, object>();
                            foreach (System.Data.DataColumn col in dtToDictionary.Columns)
                            {
                                row.Add(col.ColumnName, dr[col]);
                            }
                            rows.Add(row);
                        }
                        ReturnStr = serializer.Serialize(rows);
                    }
                    ////////////////////////////////////////////////////////////
                    ////////////////////////////////////////////////////////////



                    Session["bodyPartsLocationOptions"] = ReturnStr;
                }
                catch (Exception ex)
                {
                    Global.ErrorLogger.LogError(ex, Global.AgencyAbbreviation);
                    Response.Write(ReturnErrorMessage(ex));
                }
            }
            else
            {
                ReturnStr = Session["bodyPartsLocationOptions"].ToString();
            }
            return ReturnStr;
        }


        /// <summary>
        /// Function returns options for the Body Parts dropdown list
        /// dropdownlists
        /// </summary>
        public string returnBodyPartOptions(Boolean updateXml)
        {
            string ReturnStr = "";
            if (Session["bodyPartsOptions"] == null)
            {
                string path = Server.MapPath("~/App_Data/PartsOfBody.xml");
                try
                {
                    System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
                    xmlDoc.Load(path);
                    XmlNodeList nodeList = default(XmlNodeList);
                    XmlNode root = xmlDoc.DocumentElement;
                    string curDateStr = root.Attributes["date"].Value;

                    if ((updateXml) && (updateTime()))
                    {
                        //Check to see if the date is today's date, if not then update the xml
                        if (!(curDateStr == null))
                        {
                            string[] format = { "MM/dd/yyyy", "M/d/yyyy", "MM-dd-yyyy" };
                            System.DateTime expenddt = default(System.DateTime);
                            System.DateTime.TryParseExact(curDateStr, format, System.Globalization.DateTimeFormatInfo.InvariantInfo, System.Globalization.DateTimeStyles.None, out expenddt);
                            if (expenddt < System.DateTime.Today)
                            {



                                ////////////////////////////////////////////////////////////////////
                                ///// BODY PARTS XML FILE UPDATE IS DISABLED UNTIL ISSUES WITH /////
                                ///// ERROR HANDLING ARE FIXED /////////////////////////////////////
                                ///// D.P. 09/22/2015 //////////////////////////////////////////////
                                ////The Body Part xml file hasn't been updated today so run the update
                                xmlBodyPartPnl.Visible = true;
                                OnlineServicesModel.BodyPartUpdate BPU = updateBodyPartsXML();
                                if (BPU.success)
                                {
                                    //Update was successful
                                    xmlBP_fail.Visible = false;
                                    xmlBP_success.Visible = true;
                                }
                                else
                                {
                                    //Updatefailed
                                    xmlBP_fail.Visible = true;
                                    xmlBP_success.Visible = false;
                                    //This is temporary and should be removed when done troubleshooting
                                    //DP 09/23/2015
                                    xmlBP_fail_Lit.Text = BPU.errMsg;
                                }
                                ///// BODY PARTS XML FILE UPDATE IS DISABLED UNTIL ISSUES WITH /////
                                ///// ERROR HANDLING ARE FIXED /////////////////////////////////////
                                ///// D.P. 09/22/2015 //////////////////////////////////////////////
                                ////////////////////////////////////////////////////////////////////


                            }
                            else
                            {
                                //Dates matched so do nothing
                                xmlBodyPartPnl.Visible = false;
                            }
                        }
                    }

                    nodeList = root.SelectNodes("Table");


                    ////////////////////////////////////////////////////////////
                    ///////// CREATE THE DATATABLE /////////////////////////////
                    System.Data.DataTable BodyPartsDT = returnBodyPartsLocDT();


                    //Iterate xml list and append option values as javascript
                    foreach (System.Xml.XmlNode XmlNode_loopVariable in nodeList)
                    {
                        //XmlNode = XmlNode_loopVariable;
                        string pob_cd = "";
                        string pob_nm = "";
                        string pob_void_ind = "";
                        string pob_opt_loc_ind = "";
                        foreach (System.Xml.XmlNode bodyPartNode in XmlNode_loopVariable)
                        {
                            string bpStr = bodyPartNode.Name;
                            switch (bpStr.ToLower())
                            {
                                case "pob_cd":
                                    pob_cd = bodyPartNode.InnerText;
                                    break;
                                case "pob_nm":
                                    pob_nm = bodyPartNode.InnerText;
                                    break;
                                case "pob_void_ind":
                                    pob_void_ind = bodyPartNode.InnerText;
                                    break;
                                case "pob_opt_loc_ind":
                                    pob_opt_loc_ind = bodyPartNode.InnerText;
                                    break;
                            }
                        }


                        //Only add body part that have not been voided to datatable
                        if (pob_void_ind == "n")
                        {
                            if ((BodyPartsDT != null))
                            {
                                //Add to temp datatable
                                BodyPartsDT.Rows.Add(pob_nm, "", 0, "", pob_cd, pob_opt_loc_ind);
                            }
                        }
                    }

                    /////////////////////////////////////////////////////////////
                    ///////// DP 07/24/ 2014 ////////////////////////////////////
                    ///////// -Converting data to Json object ///////////////////
                    if ((BodyPartsDT != null))
                    {
                        BodyPartsDT = returnUpdatedDT(BodyPartsDT);
                        System.Data.DataView dv = new System.Data.DataView(BodyPartsDT);
                        //dv.Sort = "ints";
                        System.Data.DataTable dtToDictionary = dv.ToTable();
                        System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                        List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                        Dictionary<string, object> row;
                        foreach (System.Data.DataRow dr in dtToDictionary.Rows)
                        {
                            row = new Dictionary<string, object>();
                            foreach (System.Data.DataColumn col in dtToDictionary.Columns)
                            {
                                row.Add(col.ColumnName, dr[col]);
                            }
                            rows.Add(row);
                        }
                        ReturnStr = serializer.Serialize(rows);
                    }
                    /////////////////////////////////////////////////////////////
                    /////////////////////////////////////////////////////////////

                    Session["bodyPartsOptions"] = ReturnStr;
                }
                catch (Exception ex)
                {
                    Global.ErrorLogger.LogError(ex, Global.AgencyAbbreviation);
                    Response.Write(ReturnErrorMessage(ex));
                }
            }
            else
            {
                ReturnStr = Session["bodyPartsOptions"].ToString();
            }
            return ReturnStr;
        }

        /// <summary>
        /// Function returns Boolean value 
        /// that determines if the current time falls between
        /// start and end values
        /// </summary>
        public Boolean updateTime()
        {
            Boolean returnBool = false;
            TimeSpan start = new TimeSpan(10, 45, 0); //10 o'clock
            TimeSpan end = new TimeSpan(11, 0, 0); //12 o'clock
            TimeSpan now = DateTime.Now.TimeOfDay;

            if ((now > start) && (now < end))
            {
                //match found
                returnBool = true;
            }
            return returnBool;
        }


        /// <summary>
        /// Function returns updated datatable with 
        /// original, text, ints, suffix and values all in
        /// their respectable columns
        /// </summary>
        public string ReturnErrorMessage(Exception ex)
        {
            string ReturnStr = "";
            if (Session["ShowErrors"] != null)
            {
                if (Session["ShowErrors"].ToString() == "true")
                {
                    ReturnStr = "ex.tostring=" + ex.ToString();
                }
            }
            else
            {
                ReturnStr = "<h2 style=\"color:red;font-size: 11 pt;\">Error loading xml</h2>";
            }
            return ReturnStr;
        }


        /// <summary>
        /// Function returns updated datatable with 
        /// original, text, ints, suffix and values all in
        /// their respectable columns
        /// </summary>
        public System.Data.DataTable returnUpdatedDT(System.Data.DataTable DT)
        {
            System.Data.DataTable ReturnDT = new System.Data.DataTable();
            string Suffix = "";
            foreach (System.Data.DataRow row in DT.Rows)
            {
                if (isNumber(row["original"].ToString()))
                {
                    row["ints"] = Convert.ToInt32(row["original"].ToString().Substring(0, row["original"].ToString().Length - 2));
                    if ((row["original"].ToString().Length >= 3))
                    {
                        Suffix = row["original"].ToString().Substring(row["original"].ToString().Length - 2);
                    }
                    row["suffix"] = Suffix;
                }
                else
                {
                    row["text"] = row["original"];
                }
            }
            if ((DT != null))
            {
                DT.DefaultView.Sort = "ints";
                ReturnDT = DT;
            }
            return ReturnDT;
        }

        /// <summary>
        /// Function returns boolean value that determines
        /// whether parsed value is numeric
        /// </summary>
        public bool isNumber(string str)
        {
            bool ReturnBool = false;
            if (!string.IsNullOrEmpty(str))
            {
                if (str.Length >= 3)
                {
                    string LastTwo = str.Substring(0, str.Length - 2);
                    ReturnBool = Information.IsNumeric(LastTwo);
                }
            }
            return ReturnBool;
        }

        /// <summary>
        /// Function returns datatable for sorting Body Part Locations 
        /// </summary>
        public System.Data.DataTable returnBodyPartsLocDT()
        {
            System.Data.DataTable DT = new System.Data.DataTable();
            DT.Columns.Add("original", typeof(string));
            DT.Columns.Add("text", typeof(string));
            DT.Columns.Add("ints", typeof(int));
            DT.Columns.Add("suffix", typeof(string));
            DT.Columns.Add("value", typeof(string));
            DT.Columns.Add("pob_opt_loc_ind", typeof(string));
            return DT;
        }





        //    public void ShowErrors()
        //    {
        //        string ShowErrorsStr = Request.QueryString("showerrors");
        //        if (!string.IsNullOrEmpty(ShowErrorsStr))
        //        {
        //            if (Session["ShowErrors"] == null)
        //            {
        //                Session["ShowErrors"] = "true";
        //            }
        //        }
        //    }


        /// <summary>
        /// Subroutine updates the ~/App_Data/PartsOfBody.xml with data from CMSDEV
        /// </summary>
        public OnlineServicesModel.BodyPartUpdate updateBodyPartsXML()
        {
            OnlineServicesModel.BodyPartUpdate BPU = new OnlineServicesModel.BodyPartUpdate();
            try
            {
                m_pobList = new OnlineServicesModel.PartOfBodyList();
                System.Data.DataTable dt = (System.Data.DataTable)m_pobList.ReturnTempDT();
                if ((dt != null))
                {
                    //Probably not the best way to do this but it will work for now
                    System.IO.StringWriter writer = new System.IO.StringWriter();
                    dt.WriteXml(writer, true);
                    string dateStr = writer.ToString().Replace("<NewDataSet>", "<NewDataSet date=\"" + System.DateTime.Today.ToString("MM/dd/yyyy") + "\">");
                    dynamic xdoc = new XmlDocument();
                    xdoc.LoadXml(dateStr);
                    xdoc.Save(Server.MapPath("~/App_Data/PartsOfBody.xml"));
                    BPU.success = true;
                    return BPU;
                    //return true;
                    //Response.Write("Print the xml=" & dateStr)
                }
            }
            catch (Exception ex)
            {
                Global.ErrorLogger.LogError(ex, Global.AgencyAbbreviation);
                if (Session["ShowErrors"] != null)
                {
                    Response.Write(ReturnErrorMessage(ex));
                }
                BPU.errMsg = ex.ToString();
            }
            BPU.success = false;
            return BPU;
        }



        /// <summary>
        /// Subroutine updates the ~/App_Data/PartsOfBodyLocation.xml with data from CMSDEV
        /// </summary>
        public OnlineServicesModel.BodyPartUpdate updateBodyPartsLocationXML()
        {
            OnlineServicesModel.BodyPartUpdate BPU = new OnlineServicesModel.BodyPartUpdate();
            try
            {
                OnlineServicesModel.PartOfBodyList m_pobLocationList = new OnlineServicesModel.PartOfBodyList();
                System.Data.DataTable dt = (System.Data.DataTable)m_pobLocationList.ReturnBodyPartLocationDT();
                if ((dt != null))
                {
                    //Probably not the best way to do this but it will work for now
                    System.IO.StringWriter writer = new System.IO.StringWriter();
                    dt.WriteXml(writer, true);
                    string dateStr = writer.ToString().Replace("<NewDataSet>", "<NewDataSet date=\"" + System.DateTime.Today.ToString("MM/dd/yyyy") + "\">");
                    dynamic xdoc = new XmlDocument();
                    xdoc.LoadXml(dateStr);
                    xdoc.Save(Server.MapPath("~/App_Data/PartsOfBodyLocation.xml"));
                    BPU.success = true;
                    return BPU;
                    //Response.Write("Print the xml=" & dateStr)
                }
            }
            catch (Exception ex)
            {
                Global.ErrorLogger.LogError(ex, Global.AgencyAbbreviation);
                if (Session["ShowErrors"] != null)
                {
                    Response.Write(ReturnErrorMessage(ex));
                }
                BPU.errMsg = ex.ToString();
            }
            BPU.success = false;
            return BPU;
        }


        /// <summary>
        /// Function Returns a string with link to javascript for autofills if
        /// the showautofills request variable is set to true
        /// </summary>
        public string returnAutoFills()
        {
            string returnStr = "";
            string showautofills = Request.QueryString["showautofills"];
            //if (showautofills != "") {
            if (string.IsNullOrEmpty(showautofills) != true)
            {
                if (showautofills.ToLower() == "true")
                {
                    returnStr = "<script language=\"JavaScript\" src=\"scripts/autofills_irentry.js\" type=\"text/javascript\"></script>";
                }
            }
            return returnStr;
        }


        #endregion


















    }
}