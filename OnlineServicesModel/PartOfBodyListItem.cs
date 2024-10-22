using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OnlineServicesModel
{
    public class PartOfBodyListItem
    {
        string m_code;
        string m_name;
        string m_opt_loc_ind;

        public PartOfBodyListItem(System.Data.DataRow rowOfData)
        {
            m_code = rowOfData["POB_CD"].ToString();
            m_name = rowOfData["POB_NM"].ToString();
            m_opt_loc_ind = rowOfData["POB_OPT_LOC_IND"].ToString();

        }

        public PartOfBodyListItem()
        {
            m_code = string.Empty;
            m_name = string.Empty;
            m_opt_loc_ind = string.Empty;
        }

        public string Code
        {
            get { return m_code; }
        }

        public string Name
        {
            get { return m_name; }
        }

        public bool HasAssociatedLocation
        {
            get { return (m_opt_loc_ind.ToUpper() == "Y"); }
        }
    }
}
