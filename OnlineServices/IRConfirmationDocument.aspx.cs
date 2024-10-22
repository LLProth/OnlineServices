using IncidentReportModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WsiWebHelpers;

namespace OnlineServices
{
  public partial class IRConfirmationDocument : System.Web.UI.Page
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      string inReceipt = Request["receipt"].ToString();
      string inIRID = Request["theIR_ID"].ToString();

      IncidentReportWorker thisWorker = new IncidentReportWorker(Convert.ToInt32(inIRID));

      this.spanFirstName.InnerText = thisWorker.FirstName;
      this.spanMI.InnerText = thisWorker.MiddleInitial;
      this.spanLastName.InnerText = thisWorker.LastName;
      this.spanSsn.InnerText = thisWorker.SocialSecurityNumber;
      this.spanBirthDate.InnerText = thisWorker.DateOfBirth.ToShortDateString();
      this.spanDateOfInjury.InnerText = thisWorker.InjuryDate.ToShortDateString();
      this.spanBodyPartInjured.InnerText = thisWorker.BodyPartInjuredName;
      this.spanLocationOfBodyPartInjured.InnerText = thisWorker.BodyLocationName;
      this.spanNatureOfInjury.InnerText = thisWorker.NatureOfInjuryName;
      this.spanDescriptionOfAccident.InnerText = thisWorker.DescriptionOfInjury;
      this.spanBusinessName.InnerText = thisWorker.EmployerName;
      this.spanSubmittedByName.InnerText = thisWorker.EmployerSignature;
      this.spanSubmittedDateTime.InnerText = string.Format("{0} {1}", thisWorker.SubmittedDate.ToLongDateString(), thisWorker.SubmittedDate.ToShortTimeString());
      thisWorker = null;
    }
  }
}