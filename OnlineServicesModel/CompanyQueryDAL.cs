using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using OdpMDataAccess;
using Oracle.ManagedDataAccess.Client;

namespace IncidentReportModel
{
  public class CompanyQueryDAL : DALBase
  {
    private const string companyLookupDataConnectString = "IncidentReportData";

    public CompanyQueryDAL()
      : base(companyLookupDataConnectString, true)
    { }

    public DataTable ExecuteSearch(string name, int acct)
    {
      return this.ExecuteSearchPics(name, acct);
    }

        public DataTable ExecuteSearchPics(string name, int acct)
        {
            string picsDbLink = ConfigurationManager.AppSettings["PicsDbLink"];
            string sql = "SELECT emp.ACCT_NO, emp.BUSN_NM, emp.LGL_NM, emp.CITY, emp.STT, emp.ACCT_STS_CD  FROM OEL_EMPLOYER_LOOKUP_VIEW@{0} emp ";
            sql = string.Format(sql, picsDbLink);
            DataTable result = null;
            string whereSql = string.Empty;
            int arrayDimCount = 0;
            bool searchByName = false;
            bool searchByAcct = false;
            if (name.Trim().Length > 0)
            {
                searchByName = true;
                arrayDimCount += 2;
                whereSql = " UPPER(emp.busn_nm) like :1 OR UPPER(emp.LGL_NM) like :2 ";
            }
            if (acct > 0)
            {
                searchByAcct = true;
                arrayDimCount++;
                if (whereSql.Length > 0)
                {
                    whereSql = string.Format("{0} OR emp.ACCT_NO = :3 ", whereSql);
                }
                else
                {
                    whereSql = " emp.ACCT_NO = :acct ";
                }
            }
            if (whereSql.Length <= 0)
            {
                return result;
            }
            string excludeExpiredString = string.Empty;
            sql = string.Format("{0} WHERE ({1}) and rownum < 1000 {2} ORDER BY busn_nm ", sql, whereSql, excludeExpiredString);
            string[] parmNames = new string[arrayDimCount];
            object[] parms = new object[arrayDimCount];
            ParameterDirection[] parmDirections = new ParameterDirection[arrayDimCount];
            int nextArrayIndex = 0;
            if (searchByName)
            {
                parmNames[nextArrayIndex] = ":1";
                parms[nextArrayIndex] = string.Format("{0}%", name.Trim().ToUpper());
                parmDirections[nextArrayIndex] = ParameterDirection.Input;
                nextArrayIndex++;
                parmNames[nextArrayIndex] = ":2";
                parms[nextArrayIndex] = string.Format("{0}%", name.Trim().ToUpper());
                parmDirections[nextArrayIndex] = ParameterDirection.Input;
                nextArrayIndex++;
            }
            if (searchByAcct)
            {
                parmNames[nextArrayIndex] = ":3";
                parms[nextArrayIndex] = acct;
                parmDirections[nextArrayIndex] = ParameterDirection.Input;
                nextArrayIndex++;
            }
            return this.ExecuteSelect(sql, parmNames, parms, parmDirections).Tables[0];
        }

        public Dictionary<string, string> GetNatureOfInjuryValidValues()
    {
      Dictionary<string, string> results = new Dictionary<string, string>();
      string environ = ConfigurationManager.AppSettings["TargetApplication"].ToString().ToUpper();
      string strSql = "SELECT noi_cd, noi_nm FROM nature_of_injury WHERE noi_void_ind <> 'y' order by noi_nm";
      DataSet resultDataSet = base.ExecuteSelect(strSql);
      if (resultDataSet.Tables.Count > 0)
      {
        foreach (DataRow row in resultDataSet.Tables[0].Rows)
        {
          results.Add(row["noi_cd"].ToString(), row["noi_nm"].ToString());
        }
      }
      return results;
    }

  }
}
