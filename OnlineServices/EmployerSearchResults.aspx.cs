using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OnlineServices
{
  public partial class EmployerSearchResults : System.Web.UI.Page
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      //this.anchorcontactus.HRef = ConfigurationManager.AppSettings["Link_ContactUs"].ToString();
      DataTable table = (DataTable)this.Session["EmployeeSearchData"];
       //A quick test
       //table = null;

        if (table == null) {
            //If session expires then display the session expired message
            this.panelSessionExpired.Visible = true;
            this.panelNoResults.Visible = false;
            this.panelGridResults.Visible = false;
            this.lblnumberEmployers.Text = "";
        }
        else
        {
            this.panelSessionExpired.Visible = false;
            if (table.Rows.Count == 0)
            {
                this.panelNoResults.Visible = true;
                this.panelGridResults.Visible = false;
            }
            else
            {
                this.panelNoResults.Visible = false;
                this.panelGridResults.Visible = true;
                this.lblnumberEmployers.Text = table.Rows.Count.ToString();
                this.grdEmployerSearchResult.DataSource = table;
                this.grdEmployerSearchResult.DataBind();
            }
        }


    }

    protected void btnNewSearch_Click(object sender, EventArgs e)
    {
      string url = ConfigurationManager.AppSettings["Link_EmployerSearch"].ToString();
      this.Response.Redirect(url);
    }
  }
}