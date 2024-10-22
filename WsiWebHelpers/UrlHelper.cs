using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using OdpMDataAccess;

namespace WsiWebHelpers
{
  public class UrlHelper : DALBase
  {
    public UrlHelper(string connectStringName)
      : base(connectStringName, true)
    {
    }


    public string GetUrl(string urlId)
    {
      string str2 = string.Empty;
      string sql = "SELECT URL  FROM WSI_WEB_URLS  WHERE URL_ID = :urlId ";
      string[] parmNames = new string[] { ":urlId" };
      object[] parmValues = new object[] { urlId };
      DataTable table = base.ExecuteSelect(sql, parmNames, parmValues).Tables[0];
      if (table.Rows.Count > 0)
      {
        str2 = table.Rows[0]["URL"].ToString();
      }
      return str2;
    }



  }
}
