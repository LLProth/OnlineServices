namespace ClaimLookupModel
{
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.Configuration;
  using System.Data;
  using WSI.DataAccess.ClaimLookupDAL;


  public class ClaimList
  {
    private DataSet m_data = null;
    private List<ClaimInfo> m_List = new List<ClaimInfo>();

    public ClaimList(string ssn, string claimNumber, string firstName, string lastName, DateTime dob, DateTime injuryDate)
    {
      this.m_data = null;
      if (claimNumber.Length > 0)
      {
        this.m_data = this.LookupClaimByNumber(claimNumber);
      }
      if ((this.m_data == null) & (ssn.Length > 0))
      {
        this.m_data = this.LookupClaimBySsn(ssn, injuryDate);
      }
      if ((this.m_data == null) & (DateTime.Compare(dob, DateTime.MinValue) > 0))
      {
        this.m_data = this.LookupClaimByNameAndDOB(lastName, dob, injuryDate);
      }
      if ((this.m_data == null) & (firstName.Length > 0))
      {
        this.m_data = this.LookupFirstNameLastName(firstName, lastName, injuryDate);
      }
      if (this.m_data.Tables.Count > 0)
      {
        IEnumerator enumerator = null;
        try
        {
          enumerator = this.m_data.Tables[0].Rows.GetEnumerator();
          while (enumerator.MoveNext())
          {
            DataRow current = (DataRow)enumerator.Current;
            this.m_List.Add(new ClaimInfo(current));
          }
        }
        finally
        {
          if (enumerator != null &&  enumerator is IDisposable)
          {
            (enumerator as IDisposable).Dispose();
          }
        }
      }
    }

    private DataSet LookupClaimByNameAndDOB(string lastName, DateTime dob, DateTime injuryDate)
    {
      DataSet set2 = new ClaimLookupDataAccess().LookupClaimByNameAndDob(lastName, dob, injuryDate, this.PlusMinusDayRange());
      return set2;
    }

    private DataSet LookupClaimByNumber(string claimNumber)
    {
      DataSet claimByNumber = new ClaimLookupDataAccess().LookupClaimByNumber(claimNumber, this.PlusMinusDayRange());
      return claimByNumber;
    }

    private DataSet LookupClaimBySsn(string ssn, DateTime injuryDate)
    {
      DataSet set2 = new ClaimLookupDataAccess().LookupClaimBySsn(ssn, injuryDate, this.PlusMinusDayRange());
      return set2;
    }

    private DataSet LookupFirstNameLastName(string firstName, string lastName, DateTime injuryDate)
    {
      DataSet set2 = new ClaimLookupDataAccess().LookupFirstNameLastName(firstName, lastName, injuryDate, this.PlusMinusDayRange());
      return set2;
    }

    private int PlusMinusDayRange()
    {
      return int.Parse(ConfigurationManager.AppSettings["ClaimLookupPlusMinusDaysRange"]);
    }

    public List<ClaimInfo> List
    {
      get
      {
        return this.m_List;
      }
    }
  }
}