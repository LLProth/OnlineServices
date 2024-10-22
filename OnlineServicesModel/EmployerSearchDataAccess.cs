namespace EmployerSearchDAL
{
  using Microsoft.VisualBasic;
  using System;
  using System.Collections.Generic;
  using System.Configuration;
  using System.Data;
  using OdpMDataAccess;

  public class EmployerSearchDataAccess : DALBase
  {

    public EmployerSearchDataAccess()
      : base("PicsOel", true)
    {
    }

    public DataTable EmployerSearch(string infein, string inbusinessname, string inlegalName, string incity, string instate)
    {
      string dbLinkName = ConfigurationManager.AppSettings["PicsDbLink"];

      string sql = "select l.busn_nm as Business, l.lgl_nm As \"Legal Name\", l.city as \"City\", l.stt As \"ST\", "
                + " decode(e.cert_expr_dt,null,'Contact WSI',to_char(e.cert_expr_dt,'MM/DD/YYYY'))  as \"Expiration Date\" "
                + string.Format(" FROM OEL_EMPLOYER_LOOKUP_VIEW@{0} l ", dbLinkName)
                + string.Format(" Left Outer Join oel_expiration_view@{0} e", dbLinkName)
                  + " ON l.acct_no = e.acct_no";

      List<Object> includedParmValues = new List<Object>();
      List<string> includedParmNames = new List<string>();

      if (infein.Length > 0)
      {
        sql = sql + " where l.fed_tax_id = :inFein ";
        includedParmValues.Add(infein);
        includedParmNames.Add(":infein");
      }
      
      if (inbusinessname.Length > 0)
      {
        if (includedParmValues.Count>0)
        {
          sql = sql + " and ";
        }
        else
        {
          sql = sql + " where ";
        }
        sql = sql + "LOWER(l.busn_nm) like :busn_nm "; 
        includedParmValues.Add(string.Format("%{0}%", inbusinessname.ToLower().Trim()));
        includedParmNames.Add(":busn_nm");
      }
      
      if (inlegalName.Length > 0)
      {
        if (includedParmValues.Count>0)
        {
          sql = sql + " and ";
        }
        else
        {
          sql = sql + " where ";
        }
        sql = sql + "LOWER(l.lgl_nm) like :inlegalName ";
        includedParmValues.Add(string.Format("%{0}%", inlegalName.ToLower().Trim()));
        includedParmNames.Add(":inlegalName");
      }

      if (incity.Length > 0)
      {
        if (includedParmValues.Count > 0)
        {
          sql = sql + " and ";
        }
        else
        {
          sql = sql + " where ";
          
        }
        sql = sql + "LOWER(l.city) like :incity ";
        includedParmValues.Add(string.Format("%{0}%", incity.ToLower().Trim()));
        includedParmNames.Add(":incity");
      }



      if (instate.Length > 0)
      {
        if (includedParmValues.Count > 0)
        {
          sql = sql + " and ";
        }
        else
        {
          sql = sql + " where ";
           
        }
        sql = sql + "LOWER(l.stt) like :instate ";
        includedParmValues.Add(string.Format("%{0}%", instate.ToLower().Trim()));
        includedParmNames.Add(":instate");
      }
      
      return base.ExecuteSelect(sql, includedParmNames.ToArray(), includedParmValues.ToArray()).Tables[0];
    
    }
  }
}

