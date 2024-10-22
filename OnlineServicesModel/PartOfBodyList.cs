using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WSI.DataAccess.PartOfBodyDAL;

namespace OnlineServicesModel
{
    public class PartOfBodyList
    {
        
	    //Public Const _DB_Connection_String_Key = "OwsiConnection"
        public const string _DB_Connection_String_Key = "WSIDEVConnection";
	    private System.Data.DataTable m_POBs = null;
	    private System.Data.DataTable m_POBLocations = null;
	    private List<PartOfBodyListItem> m_PobList;

	    public PartOfBodyList()
	    {
		    //Populate datatable with
		    m_POBs = ReturnTempDT();
		    m_POBLocations = ReturnBodyPartLocationDT();
	    }

	    //<summary>
	    //Function returns datatable with bodyparts list
	    //</summary>
	    public System.Data.DataTable ReturnTempDT()
	    {
		    System.Data.DataTable TempDT = new System.Data.DataTable();
            PartOfBodyDataAccess access = new PartOfBodyDataAccess(_DB_Connection_String_Key, "cms");
            m_POBs = access.BodyPartsList();
            if ((m_POBs != null)) {
                TempDT = m_POBs;
            }
            access = null;
		    return TempDT;
	    }

	    //<summary>
	    //Function returns datatable with bodypartlocation list
	    //</summary>
	    public System.Data.DataTable ReturnBodyPartLocationDT()
	    {
		    System.Data.DataTable TempDT = new System.Data.DataTable();
            PartOfBodyDataAccess access = new PartOfBodyDataAccess(_DB_Connection_String_Key, "cms");
            m_POBLocations = access.BodyPartLocationList();
            if ((m_POBLocations != null)) {
                TempDT = m_POBLocations;
            }
            access = null;
		    return TempDT;
	    }


	    public List<PartOfBodyListItem> BodyPartsList {
		    get {
			    if (m_PobList == null) {
				    m_PobList = new List<PartOfBodyListItem>();
				    m_PobList.Add(new PartOfBodyListItem());
				    foreach (System.Data.DataRow row in m_POBs.Rows) {
					    PartOfBodyListItem x = new PartOfBodyListItem(row);
					    if (x.Name.ToUpper() == "UNKNOWN") {
						    m_PobList.Insert(1, x);
					    } else {
						    m_PobList.Add(x);
					    }

				    }
			    }
			    return m_PobList;
		    }
	    }

	    public List<System.Collections.DictionaryEntry> BodyPartLocations(string pob_cd)
	    {
		    System.Data.DataView resultView = default(System.Data.DataView);
		    if (m_POBLocations == null) {
			    m_POBLocations = ReturnTempDT();
		    }

		    //-- first match by the provided code
		    resultView = new System.Data.DataView(m_POBLocations);
		    resultView.RowFilter = string.Format("POB_CD = '{0}'", pob_cd);

		    //-- if there are no matching rows for the provided code then match with all that 
		    //-- have a code '00'
		    if (pob_cd.Length > 0 & resultView.Count == 0) {
			    resultView.RowFilter = "POB_CD = '00'";
		    }

            List<System.Collections.DictionaryEntry> result = new List<System.Collections.DictionaryEntry>();
            result.Add(new System.Collections.DictionaryEntry("", ""));

		    foreach (System.Data.DataRowView dvr in resultView) {
                result.Add(new System.Collections.DictionaryEntry(dvr[0].ToString(), dvr[1].ToString()));
		    }

		    return result;
	    }
    }
}
