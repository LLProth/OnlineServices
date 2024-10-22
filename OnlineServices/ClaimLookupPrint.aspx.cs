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
  public partial class ClaimLookupPrint : System.Web.UI.Page
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      this.lblDate.Text = string.Format("Claim Lookup Date: {0}", DateTime.Now.ToShortDateString());
      ClaimList list = (ClaimList)this.Session.Contents["lookup.List"];
      this.lblLookupCriteria.Text = this.Session.Contents["lookup.Description"].ToString();
      this.resultGrid.DataSource = list.List;
      this.resultGrid.DataBind();
    }
  }
}