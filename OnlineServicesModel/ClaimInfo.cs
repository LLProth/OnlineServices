namespace ClaimLookupModel
{
    using System;
    using System.Data;
    using System.Runtime.CompilerServices;

    public class ClaimInfo
    {
        private const string BodyPartColumn = "POB";
        private const string BodyPartPrimaryIndicatorColumn = "INJR_POB_PRI_IND";
        private const string ClaimNumberColumn = "CLM_NO";
        private const string ClaimStatusCodeColumn = "clm_sts_cd";
        private const string ClaimStatusColumn = "CLM_STS_NM";
        private const string DoiColumn = "INJR_DTM";
        private const string EmployerColumn = "BUSN_HIST_NM";
        private const string FirstNameColumn = "prsn_hist_nm_fst";
        private const string LastNameColumn = "prsn_hist_nm_lst";
        private DataRow m_claimInfoRow = null;

        public ClaimInfo(DataRow rowData)
        {
            this.m_claimInfoRow = rowData;
        }

        public bool IsOpen()
        {
            string str = this.m_claimInfoRow["clm_sts_cd"].ToString().ToUpper().Trim();
            return ((str == "OPACC") | (str == "OPDNY"));
        }

        public bool IsPresumedClosed()
        {
            string str = this.m_claimInfoRow["clm_sts_cd"].ToString().ToUpper().Trim();
            return ((str == "PRECLACC") | (str == "PRECLDNY"));
        }

        public string BodyPart
        {
            get
            {
                string str3 = this.m_claimInfoRow["POB"].ToString();
                string str2 = "<span class='loud'>*</span>";
                if (this.m_claimInfoRow["INJR_POB_PRI_IND"].ToString().ToUpper() == "Y")
                {
                    str3 = string.Format("{0}{1}", str2, str3);
                }
                return str3;
            }
        }

        public string ClaimNumber
        {
            get
            {
                return this.m_claimInfoRow["CLM_NO"].ToString();
            }
        }

        public string ClaimStatus
        {
            get
            {
                return this.m_claimInfoRow["CLM_STS_NM"].ToString();
            }
        }

        public string Employer
        {
            get
            {
                return this.m_claimInfoRow["BUSN_HIST_NM"].ToString();
            }
        }

        public string FirstName
        {
            get
            {
                return this.m_claimInfoRow["prsn_hist_nm_fst"].ToString();
            }
        }

        public string InjuryDate
        {
            get
            {
                return Convert.ToDateTime(RuntimeHelpers.GetObjectValue(this.m_claimInfoRow["INJR_DTM"])).ToShortDateString();
            }
        }

        public string LastName
        {
            get
            {
                return this.m_claimInfoRow["prsn_hist_nm_lst"].ToString();
            }
        }
    }
}

