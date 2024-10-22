namespace EmployerSearchModel
{
    using EmployerSearchDAL;
    using System;
    using System.Data;

    public class EmployerSearchModel
    {
        private string m_businessName;
        private string m_city;
        private string m_fein;
        private string m_legalName;
        private string m_state;

        public DataTable GetEmployeeSearchData()
        {
            EmployerSearchDataAccess access = new EmployerSearchDataAccess();
            return access.EmployerSearch(this.m_fein, this.m_businessName, this.m_legalName, this.m_city, this.m_state);
        }

        public string BusinessName
        {
            get
            {
                return this.m_businessName;
            }
            set
            {
                this.m_businessName = value;
            }
        }

        public string City
        {
            get
            {
                return this.m_city;
            }
            set
            {
                this.m_city = value;
            }
        }

        public string Fein
        {
            get
            {
                return this.m_fein;
            }
            set
            {
                this.m_fein = value;
            }
        }

        public string LegalName
        {
            get
            {
                return this.m_legalName;
            }
            set
            {
                this.m_legalName = value;
            }
        }

        public string State
        {
            get
            {
                return this.m_state;
            }
            set
            {
                this.m_state = value;
            }
        }
    }
}

