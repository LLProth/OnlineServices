using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WsiWebHelpers;

namespace OnlineServices
{
  public partial class EmployerSearch : System.Web.UI.Page
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
      this.txtfein.Text = "";
      this.txtbusinessName.Text = "";
      this.txtlegalName.Text = "";
      this.txtcity.Text = "";
      this.txtstate.Text = "";
      this.errorMessageRow.Visible = false;
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
      EmployerSearchModel.EmployerSearchModel model = new EmployerSearchModel.EmployerSearchModel();
      if ((this.txtfein.Text.Trim() + this.txtbusinessName.Text.Trim() + this.txtlegalName.Text.Trim() + this.txtcity.Text.Trim() + this.txtstate.Text.Trim()).Length == 0)
      {
        this.errorMessageRow.Visible = true;
      }
      else
      {
                try
                {
                    EmployerSearchModel.EmployerSearchModel model2 = model;
                    model2.Fein = this.txtfein.Text;
                    model2.BusinessName = this.txtbusinessName.Text;
                    model2.LegalName = this.txtlegalName.Text;
                    model2.City = this.txtcity.Text;
                    model2.State = this.txtstate.Text;
                    DataTable employeeSearchData = model2.GetEmployeeSearchData();
                    model2 = null;
                    this.Session["EmployeeSearchData"] = employeeSearchData;
                    this.Response.Redirect("EmployerSearchResults.aspx");
                }
                catch(Exception ex)
                {
                    this.errorMessageRow.Visible = true;
                    this.errorMessage.Text = ex.Message;
                }
      }
    }

   
  }
}