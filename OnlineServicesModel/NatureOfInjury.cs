using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace IncidentReportModel
{
  public class NatureOfInjury
  {
    // Fields
    private string _environment = "";

    // Methods
    public NatureOfInjury(string environment)
    {
        if (environment.ToUpper() == "TEST")
        {
            this._environment = "iFaceConnectionTEST";
        }
        else if (environment.ToUpper() == "PROD")
        {
            this._environment = "iFaceConnectionPROD";
        }
        else
        {
          this._environment = "IncidentReportData";
        }
    }

    public Dictionary<string, string> GetNatureOfInjuryValidValues()
    {
        Dictionary<string, string> results = new Dictionary<string, string>();
        DataTable data = new NatureOfInjuryDAL(this._environment).FetchNatureOfInjury();
        if (data.Rows.Count > 0)
        {
          results.Add("-1", "Select One");
          foreach (DataRow row in data.Rows)
          {
            results.Add(row["noi_cd"].ToString(), row["noi_nm"].ToString());
          }
        }
        return results;
    }

  }


}
