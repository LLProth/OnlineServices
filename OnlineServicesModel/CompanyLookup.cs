using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace IncidentReportModel
{
  public class CompanyLookup
  {

    public static DataTable CompanySearch(string name, string acct)
    {
      DataTable result = null;
      int acctInt = 0;
      int.TryParse(acct, out acctInt);
      if ((name.Length > 0) | (acctInt > 0))
      {
        CompanyQueryDAL DAL = new CompanyQueryDAL();
        result = DAL.ExecuteSearch(name, acctInt);
      }
      else
      {
        result = new DataTable();
      }
      return result;
    }
  }
}
