using ClaimLookupModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OnlineServices
{
  public partial class _Default : Page
  {

    private DateTime m_BirthDate;
    private string m_ClaimNumber = string.Empty;
    private ClaimList m_currentClaimLookupResults;
    private string m_FirstName = string.Empty;
    private DateTime m_InjuryDate = DateTime.Now;
    private string m_InputSsn = string.Empty;
    private string m_LastName = string.Empty;
    private string m_lookupDescription = string.Empty;
    
    protected void Page_Load(object sender, EventArgs e)
    {
      this.Response.Cache.SetCacheability(HttpCacheability.NoCache);

    }


    protected void btnPrint_Click(object sender, EventArgs e)
    {
      this.ClientScript.RegisterStartupScript(this.GetType(), "onclick", "<script language=javascript>window.open('ClaimLookupPrint.aspx','PrintMe','height=600px,width=600px,scrollbars=1');</script>");
    }


    /// <summary>
    /// lookup (search) button click postback handler
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {

      string validationMessage = this.checkInputValues();
      if (validationMessage.Length > 0)
      {
        this.lblErrorMessage.Text = validationMessage;
      }
      else
      {
                try
                {
                    this.m_currentClaimLookupResults = new ClaimList(this.m_InputSsn, this.m_ClaimNumber, this.m_FirstName, this.m_LastName, this.m_BirthDate, this.m_InjuryDate);
                    if (this.m_currentClaimLookupResults.List.Count > 0)
                    {
                        this.lblLookupCriteria.Text = this.m_lookupDescription;
                        this.lblErrorMessage.Text = string.Empty;
                        this.lblNoResultMessage.Text = this.m_lookupDescription;
                        this.panelResults.Visible = true;
                        this.panelNoResults.Visible = false;
                        this.panelLookupEntry.Visible = false;
                        this.resultGrid.DataSource = this.m_currentClaimLookupResults.List;
                        this.resultGrid.DataBind();
                        foreach (ClaimInfo info in this.m_currentClaimLookupResults.List)
                        {
                            if (info.IsPresumedClosed())
                            {
                                this.lblPresumedClosedOtherMessage.Text = ConfigurationManager.AppSettings["ClaimLookupPresumedClosedOtherMessageText"].ToString();
                                this.panelPresumedClosedStatusMessage.Visible = true;
                                this.panelClosedStatusMessage.Visible = false;
                            }
                            else if (!info.IsOpen())
                            {
                                this.lblClosedStatusMessage.Text = ConfigurationManager.AppSettings["ClaimLookupClosedOtherMessageText"].ToString();
                                this.panelClosedStatusMessage.Visible = true;
                                this.panelPresumedClosedStatusMessage.Visible = false;
                            }
                        }
                        this.Session.Contents.Add("lookup.List", this.m_currentClaimLookupResults);
                        this.Session.Contents.Add("lookup.Description", this.m_lookupDescription);
                    }
                    else
                    {
                        this.lblErrorMessage.Text = string.Empty;
                        this.lblNoResultMessage.Text = this.m_lookupDescription;
                        this.lblLookupCriteria.Text = string.Empty;
                        this.panelLookupEntry.Visible = false;
                        this.panelResults.Visible = false;
                        this.panelNoResults.Visible = true;
                    }
                }
                catch(Exception ex) 
                {
                    this.lblErrorMessage.Text = ex.Message;
                }
            }
    }

    private bool stringIsNumeric(string inputNum)
    {

      double numValue;
      return double.TryParse(inputNum, out numValue);

    }

    private bool stringIsDate(string inputDate)
    {
      
      DateTime tryDate;
      if (DateTime.TryParse(inputDate, out tryDate))
        return true;
      else
        return false;

    }

    private string checkInputValues()
    {
      bool flag = false;
      string validMessage = string.Empty;
      this.m_InputSsn = this.txtSsn.Text.Trim();
      this.m_FirstName = this.txtFirstName.Text.Trim();
      this.m_LastName = this.txtLastName.Text.Trim();
      string birthDateStr = this.txtBirthDate.Text.Trim();
      this.m_ClaimNumber = this.txtClaimNumber.Text.Trim();
      if (this.txtInjuryDate.Text.Trim().Length > 0)
      {
        if (!DateTime.TryParse(this.txtInjuryDate.Text, out this.m_InjuryDate))
        {
          validMessage = "The injury date you entered is invalid.  Please enter the injury date in the form MM/DD/YYYY..  ";
        }
      }
      else
      {
        this.m_InjuryDate = DateTime.Now;
      }
      if ((this.m_LastName.Length > 0) & (birthDateStr.Length > 0))
      {
        if (birthDateStr.Length == 0)
        {
          validMessage = string.Format("{0}Date of birth is required when searching by name & date of birth.  ", validMessage);
          this.txtBirthDate.Focus();
        }
        else if (!stringIsDate(birthDateStr))
        {
          validMessage = string.Format("{0}Date of birth must be a valid date entered as MM/DD/YYYY.  ", validMessage);
          this.txtBirthDate.Focus();
        }
        else
        {
          this.m_BirthDate = DateTime.Parse(birthDateStr);
        }
        if (this.m_LastName.Length == 0)
        {
          validMessage = string.Format("{0}Last Name is required when searching by name & date of birth. ", validMessage);
          this.txtLastName.Focus();
        }
        if (validMessage.Length == 0)
        {
          flag = true;
          this.m_lookupDescription = string.Format("Last name of <b>{0}</b>, date of birth of <b>{1}</b> and an injury date of <b>{2}</b> ", this.m_LastName, this.m_BirthDate.ToShortDateString(), this.m_InjuryDate.ToShortDateString());
        }
      }
      if (this.m_InputSsn.Length > 0)
      {
        if ((this.m_InputSsn.Length != 9) | !stringIsNumeric(this.m_InputSsn))
        {
          validMessage = "The social security number you entered is invalid.  Please enter the social security number in the form 999999999.  ";
          this.txtSsn.Focus();
        }
        else
        {
          flag = true;
          this.m_lookupDescription = string.Format("SSN of <b>{0}</b> and an injury date of <b>{1}</b>", this.m_InputSsn, this.m_InjuryDate.ToShortDateString());
        }
      }
      if (this.m_ClaimNumber.Length > 0)
      {
        if (!stringIsNumeric(this.m_ClaimNumber))
        {
          validMessage = string.Format("{0}The claim number must be a number. ", validMessage);
          this.txtClaimNumber.Focus();
        }
        else
        {
          flag = true;
          this.m_lookupDescription = string.Format("claim number of <b>{0}</b> ", this.m_ClaimNumber);
        }
      }
      if ((this.m_FirstName.Length > 0) & (this.m_LastName.Length > 0))
      {
        flag = true;
        this.m_lookupDescription = string.Format("first name of <b>{0}</b>, last name of <b>{1}</b>, and injury date of <b>{2}</b>", this.m_FirstName, this.m_LastName, this.m_InjuryDate.ToShortDateString());
      }
      if ((validMessage.Length == 0) && !flag)
      {
        validMessage = "Either a Worker Social Security Number, or  a Worker's Name and Date of Birth, or a Claim Number must be entered.";
        this.txtSsn.Focus();
      }
      return validMessage;
    }


    protected void btnNewLookupFromNoData_Click(object sender, EventArgs e)
    {
      this.lblNoResultMessage.Text = string.Empty;
      this.panelLookupEntry.Visible = true;
      this.panelNoResults.Visible = false;
      this.clearInputControls();
    }

    private void clearInputControls()
    {
      this.txtSsn.Text = string.Empty;
      this.txtClaimNumber.Text = string.Empty;
      this.txtFirstName.Text = string.Empty;
      this.txtLastName.Text = string.Empty;
      this.txtBirthDate.Text = string.Empty;
      this.txtInjuryDate.Text = string.Empty;
    }

    protected void btnNewLookupFromResults_Click(object sender, EventArgs e)
    {
      this.lblNoResultMessage.Text = string.Empty;
      this.panelLookupEntry.Visible = true;
      this.panelNoResults.Visible = false;
      this.panelResults.Visible = false;
      this.clearInputControls();
    }
  }
}