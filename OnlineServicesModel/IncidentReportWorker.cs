using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace IncidentReportModel
{

  public class IncidentReportWorker
  {
    // Fields
    private string m_BodyLocName;
    private string m_BodyPartName;
    private DateTime m_DateOfBirth;
    private string m_DescriptionOfInjury;
    private string m_EmployerAccountNumber;
    private string m_EmployerName;
    private string m_EmployerSignature;
    private string m_FirstName;
    private string m_IncidentReportConfirmation;
    private int m_IncidentReportId;
    private DateTime m_InjuryDate;
    private string m_LastName;
    private string m_MiddleInitial;
    private string m_NatureOfInjuryName;
    private string m_Ssn;
    private DateTime m_SubmittedDate;

    // Methods
    public IncidentReportWorker(int newIncidentReportId)
    {
      this.m_IncidentReportId = newIncidentReportId;
      DataRow incidentReportWorkerData = new IncidentReportDataAccess().GetIncidentReportWorkerData(newIncidentReportId);
      if (incidentReportWorkerData != null)
      {
        this.m_FirstName = incidentReportWorkerData["W_FIRST_NAME"].ToString();
        this.m_MiddleInitial = incidentReportWorkerData["W_MIDDLE_INITIAL"].ToString();
        this.m_LastName = incidentReportWorkerData["W_LAST_NAME"].ToString();
        this.m_Ssn = incidentReportWorkerData["SOC_SEC_NO"].ToString();
        this.m_BodyPartName = incidentReportWorkerData["BODY_PART_NM"].ToString();
        this.m_BodyLocName = incidentReportWorkerData["BODY_LOC_NM"].ToString();
        this.m_EmployerName = incidentReportWorkerData["EMPLOYER_NAME"].ToString();
        this.m_EmployerAccountNumber = incidentReportWorkerData["EMPLOYER_ACT_NO"].ToString();
        this.m_EmployerSignature = incidentReportWorkerData["EMPLOYER_SIGNATURE"].ToString();
        this.m_SubmittedDate = Convert.ToDateTime(RuntimeHelpers.GetObjectValue(incidentReportWorkerData["SUBMITTED_DT"]));
        this.m_InjuryDate = Convert.ToDateTime(RuntimeHelpers.GetObjectValue(incidentReportWorkerData["INJURY_DT"]));
        this.m_DateOfBirth = Convert.ToDateTime(RuntimeHelpers.GetObjectValue(incidentReportWorkerData["BIRTH_DT"]));
        this.m_NatureOfInjuryName = incidentReportWorkerData["NATURE_OF_INJURY_NM"].ToString();
        this.m_DescriptionOfInjury = incidentReportWorkerData["DESCRIPTION_OF_INJURY"].ToString();
        this.m_IncidentReportConfirmation = incidentReportWorkerData["INCIDENT_REPORT_CONFIRMATION"].ToString();
      }
    }

    // Properties
    public string BodyLocationName
    {
      get
      {
        return this.m_BodyLocName;
      }
      set
      {
        this.m_BodyLocName = value;
      }
    }

    public string BodyPartInjuredName
    {
      get
      {
        return this.m_BodyPartName;
      }
      set
      {
        this.m_BodyPartName = value;
      }
    }

    public DateTime DateOfBirth
    {
      get
      {
        return this.m_DateOfBirth;
      }
      set
      {
        this.m_DateOfBirth = value;
      }
    }

    public string DescriptionOfInjury
    {
      get
      {
        return this.m_DescriptionOfInjury;
      }
      set
      {
        this.m_DescriptionOfInjury = value;
      }
    }

    public string EmployerAccountNumber
    {
      get
      {
        return this.m_EmployerAccountNumber;
      }
      set
      {
        this.m_EmployerAccountNumber = value;
      }
    }

    public string EmployerName
    {
      get
      {
        return this.m_EmployerName;
      }
      set
      {
        this.m_EmployerName = value;
      }
    }

    public string EmployerSignature
    {
      get
      {
        return this.m_EmployerSignature;
      }
      set
      {
        this.m_EmployerSignature = value;
      }
    }

    public string FirstName
    {
      get
      {
        return this.m_FirstName;
      }
      set
      {
        this.m_FirstName = value;
      }
    }

    public string IncidentReportConfirmation
    {
      get
      {
        return this.m_IncidentReportConfirmation;
      }
      set
      {
        this.m_IncidentReportConfirmation = value;
      }
    }

    public int IncidentReportId
    {
      get
      {
        return this.m_IncidentReportId;
      }
      set
      {
        this.m_IncidentReportId = value;
      }
    }

    public DateTime InjuryDate
    {
      get
      {
        return this.m_InjuryDate;
      }
      set
      {
        this.m_InjuryDate = value;
      }
    }

    public string LastName
    {
      get
      {
        return this.m_LastName;
      }
      set
      {
        this.m_LastName = value;
      }
    }

    public string MiddleInitial
    {
      get
      {
        return this.m_MiddleInitial;
      }
      set
      {
        this.m_MiddleInitial = value;
      }
    }

    public string NatureOfInjuryName
    {
      get
      {
        return this.m_NatureOfInjuryName;
      }
      set
      {
        this.m_NatureOfInjuryName = value;
      }
    }

    public string SocialSecurityNumber
    {
      get
      {
        return this.m_Ssn;
      }
      set
      {
        this.m_Ssn = value;
      }
    }

    public DateTime SubmittedDate
    {
      get
      {
        return this.m_SubmittedDate;
      }
      set
      {
        this.m_SubmittedDate = value;
      }
    }
  }
}