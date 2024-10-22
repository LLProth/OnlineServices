namespace WSI.DataAccess.ClaimLookupDAL
{
    using System;
    using System.Configuration;
    using System.Data;
    using OdpMDataAccess;

    public class ClaimLookupDataAccess : DALBase
    {
        public ClaimLookupDataAccess() : base("OwsiConnection", true)
        {
        }

        private string ClaimLookupDbLink()
        {
            return ConfigurationManager.AppSettings["ClaimLookupDbLinkName"];
        }

        private string GetSelectSql()
        {
            return string.Format("SELECT prsn_hist_nm_fst, prsn_hist_nm_lst, prsn_hist_brth_dt, prsn_hist_ssn, trunc(injr_dtm) injr_dtm,  busn_hist_nm, clm_no,  DECODE(clm_sts_cd, 'cldny', 'Denied-Closed','opacc','Accepted-Active','opdny','Denied','pend','Pending', 'pendcov','Pending','preclacc','Presumed Closed','precldny','Denied','Contact WSI') clm_sts_nm, pob,  DECODE(injr_pob_pri_ind,null,'n',injr_pob_pri_ind) injr_pob_pri_ind, clm_sts_cd  FROM ocl_claim_lookup_view@{0} ", this.ClaimLookupDbLink());
        }

        public DataSet LookupClaimByNameAndDob(string lastName, DateTime dob, DateTime injuryDate, int plusMinusDayRange)
        {
            string selectSql = this.GetSelectSql();
            string str3 = string.Format(" WHERE upper(prsn_hist_nm_lst) = :lastName and prsn_hist_brth_dt = :dob and :injuryDate between trunc(INJR_DTM - {0}) and  trunc(INJR_DTM + {0}) ", plusMinusDayRange);
            string sql = string.Format("{0} {1}  Order by injr_dtm, injr_pob_pri_ind desc", selectSql, str3);
            string[] parmNames = new string[] { ":lastName", ":dob", ":injuryDate" };
            object[] parmValues = new object[] { lastName.ToUpper(), dob, injuryDate };
            ParameterDirection[] parmDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input };
            return base.ExecuteSelect(sql, parmNames, parmValues, parmDirections);
        }

        public DataSet LookupClaimByNumber(string claimNumber, int plusMinusDayRange)
        {
            string selectSql = this.GetSelectSql();
            string str3 = " WHERE clm_no = :clmNo ";
            string sql = string.Format("{0} {1}  Order by injr_dtm, injr_pob_pri_ind desc", selectSql, str3);
            string[] parmNames = new string[] { ":clmNo" };
            object[] parmValues = new object[] { claimNumber };
            ParameterDirection[] parmDirections = new ParameterDirection[] { ParameterDirection.Input };
            return base.ExecuteSelect(sql, parmNames, parmValues, parmDirections);
        }

        public DataSet LookupClaimBySsn(string ssn, DateTime injuryDate, int plusMinusDayRange)
        {
            string selectSql = this.GetSelectSql();
            string str3 = string.Format(" WHERE prsn_hist_ssn = :ssn and :injuryDate between trunc(INJR_DTM - {0}) and  trunc(INJR_DTM + {0}) ", plusMinusDayRange);
            string sql = string.Format("{0} {1}  Order by injr_dtm, injr_pob_pri_ind desc", selectSql, str3);
            string[] parmNames = new string[] { ":ssn", ":injuryDate" };
            object[] parmValues = new object[] { ssn, injuryDate };
            ParameterDirection[] parmDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input };
            return base.ExecuteSelect(sql, parmNames, parmValues, parmDirections);
        }

        public DataSet LookupFirstNameLastName(string firstName, string lastName, DateTime injuryDate, int plusMinusDayRange)
        {
            string selectSql = this.GetSelectSql();
            string str3 = string.Format(" WHERE upper(prsn_hist_nm_lst) = :last and upper(prsn_hist_nm_fst) = :first and :injuryDate between trunc(INJR_DTM - {0}) and  trunc(INJR_DTM + {0}) ", plusMinusDayRange);
            string sql = string.Format("{0} {1}  Order by injr_dtm, injr_pob_pri_ind desc", selectSql, str3);
            string[] parmNames = new string[] { ":last", ":first", ":injuryDate" };
            object[] parmValues = new object[] { lastName.ToUpper(), firstName.ToUpper(), injuryDate };
            ParameterDirection[] parmDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input };
            return base.ExecuteSelect(sql, parmNames, parmValues, parmDirections);
        }
    }
}

