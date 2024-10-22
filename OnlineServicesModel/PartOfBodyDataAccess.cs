namespace WSI.DataAccess.PartOfBodyDAL
{
    using System;
    using System.Configuration;
    using System.Data;
    using OdpMDataAccess;

    public class PartOfBodyDataAccess : DALBase
    {
        private const string _POB_SQL = "SELECT * FROM PART_OF_BODY@CMS_ONLN.WSI.ND.GOV ORDER BY POB_NM";
        private const string _POB_LOC_SQL = "SELECT * FROM PART_OF_BODY_LOCATION@CMS_ONLN.WSI.ND.GOV where pob_loc_void_ind = 'n' ORDER BY POB_LOC_CD DESC";

        private string claimSystemKey;

        public PartOfBodyDataAccess(string connectionStringKey, string claimSystemKey)
            : base("IncidentReportData", true)
        {
            this.claimSystemKey = claimSystemKey;
        }


        //private string PartOfBodyDbLink()
        //{
        //    //return ConfigurationManager.AppSettings["ClaimLookupDbLinkName"];
        //    string returnStr = "";
        //    return returnStr;
        //}

        /// <summary>
        /// Function returns datatable with bodyparts list
        /// </summary>
        public DataTable BodyPartsList()
        {
            DataSet resultsDataSet = default(DataSet);
            resultsDataSet = base.ExecuteSelect(_POB_SQL);
            return resultsDataSet.Tables[0];
        }

        /// <summary>
        /// Function returns datatable with bodypartlocation list
        /// </summary>
        public DataTable BodyPartLocationList()
        {
            DataSet resultsDataSet = default(DataSet);
            resultsDataSet = base.ExecuteSelect(_POB_LOC_SQL);
            return resultsDataSet.Tables[0];
        }
    }
}