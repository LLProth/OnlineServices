using IncidentReportModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WsiWebHelpers;

namespace OnlineServices
{
    public partial class IRCompanySelect : System.Web.UI.Page
    {
        private DataTable m_data;

        protected void Page_Load(object sender, EventArgs e)
        {
            // btnSearch.Attributes.Add("onclick", "setTimeout(\"UpdateImg('processingLeft','/images/ProcessingHexagonsLeft.gif');\",30);setTimeout(\"UpdateImg('processingRight','/images/ProcessingHexagonsRight.gif');\",50);");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            HtmlHelper.SetupLinkFromConfigForPopupAnchor(linkFroi, "Link_FROI");
            
            txtCompanyName.Focus();
            displayShowExButton();
        }

        protected void displayShowExButton()
        {
            string reqDisplayShowExButton = Request.QueryString["displayShowExButton"];
            if (reqDisplayShowExButton == "true")
            {
                TestEx.Visible = true;
            }
            else
            {
                TestEx.Visible = false;
            }
        }

        protected void showEx_Click(object sender, EventArgs e)
        {
            //txtCompanyName.Visible = false;
            throw new DivideByZeroException();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string companyName = txtCompanyName.Text;
            string companyAcctNo = txtCompanyAccountNumber.Text;

            // must have at least one input value to proceed
            if ((companyAcctNo.Length + companyName.Length) == 0)
            {
                this.setSearchFailMessage("Please enter either a Business Name, or an Account Number.");
            }
            else
            {
                try
                {
                    this.searchCompany(companyName, companyAcctNo);
                }
                catch (Exception ex)
                {
                    Global.ErrorLogger.LogError(ex, Global.AgencyAbbreviation, companyAcctNo);
                    WsiWebHelpers.emailHelper.mailException(System.Configuration.ConfigurationManager.AppSettings["SMTPServer"].ToString(),
                    System.Configuration.ConfigurationManager.AppSettings["Web_Master_Email_Address"].ToString(),
                    ex, Request);
                    this.setSearchFailMessage("Incident Reporting Company Select is currently not available.  Please try again later or contact customer support at the number below.  Error: " + ex.Message);
                }
            }
        }

        protected void grdSearchResults_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (((Button)e.CommandSource).CommandName.ToUpper() == "SELECTBUTTON")
            {
                Console.WriteLine(e.Item.ToString());
                DataGridItem currentGridItem = e.Item;
                string companyName = currentGridItem.Cells[2].Text;
                string companyAcctNumber = currentGridItem.Cells[1].Text;
                if ((companyAcctNumber.Length > 0) & (companyName.Length > 0))
                {
                    this.Session.Contents["employer_act_no"] = companyAcctNumber;
                    this.Session.Contents["employer_name"] = companyName;
                    string basePath = "IREntry.aspx";
                    string newUrl = string.Format("{1}?CompanyName={0}", companyName, basePath);
                    this.Response.Redirect(newUrl);
                }
            }
        }

        private void clearSearchFailMessage()
        {
            this.searchResultsDiv.Visible = true;
            this.lblSearchErrorMsg.Visible = false;
        }

        private void searchCompany(string companyName, string companyAcctNo)
        {
            this.m_data = CompanyLookup.CompanySearch(companyName, companyAcctNo);
            if (this.m_data.Rows.Count > 0)
            {
                this.clearSearchFailMessage();
                this.grdSearchResults.DataSource = this.m_data;
                this.grdSearchResults.DataBind();
                this.grdSearchResults.Visible = true;
                int gridContentHeight = (this.m_data.Rows.Count * 40) + 40;
                if (gridContentHeight > 300)
                {
                    gridContentHeight = 300;
                }
                if (this.m_data.Rows.Count == 0x3e7)
                {
                    this.lblResultsMessage.Text = "Search returned more than 1,000 businesses. Only 1,000 are displayed.";
                }
                else
                {
                    this.lblResultsMessage.Text = string.Empty;
                }
                this.searchResultsDiv.Style["height"] = string.Format("{0}px", gridContentHeight);
            }
            else
            {
                this.setSearchFailMessage(this.noDataMessage(companyName, companyAcctNo));
            }
        }

        private string noDataMessage(string name, string acct)
        {
            string result = "There were no businesses found for ";
            string forText = string.Empty;
            if (name.Length > 0)
            {
                forText = string.Format(" {0} = {1} ", this.lblCompanyName.Text, name);
            }
            if (acct.Length > 0)
            {
                if (forText.Length > 0)
                {
                    return string.Format("{0} {1}, OR {2} = {3}", new object[] { result, forText, this.lblCompanyAccountNumber.Text, acct });
                }
                return string.Format("{0} {1} = {2}", result, this.lblCompanyAccountNumber.Text, acct);
            }
            return string.Format("{0} {1}", result, forText);
        }

        private void setSearchFailMessage(string message)
        {
            searchResultsDiv.Visible = false;
            lblSearchErrorMsg.Text = message;
            lblSearchErrorMsg.Visible = true;
        }

        protected void btnSearch_Click1(object sender, EventArgs e)
        {
            string companyName = txtCompanyName.Text;
            string companyAcctNo = txtCompanyAccountNumber.Text;

            // must have at least one input value to proceed
            if ((companyAcctNo.Length + companyName.Length) == 0)
            {
                this.setSearchFailMessage("Please enter either a Business Name, or an Account Number.");
            }
            else
            {
                try
                {
                    this.searchCompany(companyName, companyAcctNo);
                }
                catch (Exception ex)
                {
                    Global.ErrorLogger.LogError(ex, Global.AgencyAbbreviation, companyAcctNo);
                    WsiWebHelpers.emailHelper.mailException(System.Configuration.ConfigurationManager.AppSettings["SMTPServer"].ToString(),
                    System.Configuration.ConfigurationManager.AppSettings["Web_Master_Email_Address"].ToString(),
                    ex, Request);
                    this.setSearchFailMessage("Incident Reporting Company Select is currently not available.  Please try again later or contact customer support at the number below.  Error: " + ex.Message);
                }
            }
        }
    }
}