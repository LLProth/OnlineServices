using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using OdpMDataAccess;

namespace IncidentReportModel
{
  public class NatureOfInjuryDAL : DALBase
  {
    // Methods
    public NatureOfInjuryDAL(string environment)
      : base(environment, true)
    {
    }

    public DataTable FetchNatureOfInjury()
    {
      DataTable results = new DataTable();
      string strSql = "SELECT noi_cd, noi_nm FROM nature_of_injury WHERE noi_void_ind='n' order by noi_nm";
      return base.ExecuteSelect(strSql).Tables[0];
    }
  }


}
