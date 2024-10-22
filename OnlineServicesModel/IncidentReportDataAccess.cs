using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using OdpMDataAccess;
using Oracle.ManagedDataAccess.Client;
//using System.Data.OracleClient;

namespace IncidentReportModel
{
  public class IncidentReportDataAccess : DALBase
  {

    public IncidentReportDataAccess()
      : base("IncidentReportData", true)
    { }

    //Original before removing bodyPartNm and bodyLocNm
    public void AddInjuredWorkerRecord(string firstName, string middleInit, string lastName, string ssn, string bodyPartNm, string employerName, string employerAcctNo, string employerSignature, DateTime submittedDate, string incidentReportId, DateTime injuryDate, DateTime birthDate, string noi, string descriptionOfInjury)
    {
      string fieldsToInsert = "w_first_name, w_middle_initial, w_last_name, soc_sec_no, body_part_nm, employer_name,  employer_act_no,  employer_signature, submitted_dt, incident_report_id, injury_dt, birth_dt, nature_of_injury_nm, description_of_injury ";
      string parameterNameList = ":w_first_name, :w_middle_initial, :w_last_name, :soc_sec_no, :body_part_nm,   :employer_name, :employer_act_no,  :employer_signature, :submitted_dt, :incident_report_id, :injury_dt, :birth_dt, :nature_of_injury_nm, :description_of_injury ";
      string sql = string.Format("INSERT INTO IR_WORKER ({0}) VALUES ({1})", fieldsToInsert, parameterNameList);
      string[] parmNames = new string[] { ":w_first_name", ":w_middle_initial", ":w_last_name", ":soc_sec_no", ":body_part_nm", ":employer_name", ":employer_act_no", ":employer_signature", ":submitted_dt", ":incident_report_id", ":injury_dt", ":birth_dt", ":nature_of_injury_nm", ":description_of_injury" };
      object[] parmValues = new object[] { firstName, middleInit, lastName, ssn, bodyPartNm, employerName, employerAcctNo, employerSignature, submittedDate, incidentReportId, injuryDate, birthDate, noi, descriptionOfInjury };
      ParameterDirection[] parmDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input };
      base.ExecuteNonQuery(sql, parmNames, parmValues, parmDirections);
    }

    //Modified 08/05/2015
    //DP - Removed bodyPartNm and bodyLocNm
    public void AddInjuredWorkerRecord(string firstName, string middleInit, string lastName, string ssn, string employerName, string employerAcctNo, string employerSignature, DateTime submittedDate, string incidentReportId, DateTime injuryDate, DateTime birthDate, string noi, string descriptionOfInjury)
    {
        string fieldsToInsert = "w_first_name, w_middle_initial, w_last_name, soc_sec_no, employer_name,  employer_act_no,  employer_signature, submitted_dt, incident_report_id, injury_dt, birth_dt, nature_of_injury_nm, description_of_injury ";
        string parameterNameList = ":w_first_name, :w_middle_initial, :w_last_name, :soc_sec_no, :employer_name, :employer_act_no,  :employer_signature, :submitted_dt, :incident_report_id, :injury_dt, :birth_dt, :nature_of_injury_nm, :description_of_injury ";
        string sql = string.Format("INSERT INTO IR_WORKER ({0}) VALUES ({1})", fieldsToInsert, parameterNameList);
        string[] parmNames = new string[] { ":w_first_name", ":w_middle_initial", ":w_last_name", ":soc_sec_no", ":employer_name", ":employer_act_no", ":employer_signature", ":submitted_dt", ":incident_report_id", ":injury_dt", ":birth_dt", ":nature_of_injury_nm", ":description_of_injury" };
        object[] parmValues = new object[] { firstName, middleInit, lastName, ssn, employerName, employerAcctNo, employerSignature, submittedDate, incidentReportId, injuryDate, birthDate, noi, descriptionOfInjury };
        ParameterDirection[] parmDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input };
        base.ExecuteNonQuery(sql, parmNames, parmValues, parmDirections);
    }



    public void AddMasterStatus(string appName, string appId, DateTime appStartTime)
    {
      string sql = "INSERT INTO MASTER_STATUS (application_name, application_id, user_start_time) VALUES (:one, :two, :three)";
      string[] parmNames = new string[] { ":one", ":two", ":three" };
      object[] parmValues = new object[] { appName, appId, appStartTime };
      ParameterDirection[] parmDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input };
      base.ExecuteNonQuery(sql, parmNames, parmValues, parmDirections);
    }

    public DataRow GetIncidentReportWorkerData(int incidentReportId)
    {
      string sql = "Select * from IR_WORKER Where incident_report_id = :incidentReportId";
      string[] parmNames = new string[] { ":incidentReportId" };
      object[] parmValues = new object[] { incidentReportId };
      ParameterDirection[] parmDirs = new ParameterDirection[] { ParameterDirection.Input };
      DataTable resultTable = base.ExecuteSelect(sql, parmNames, parmValues, parmDirs).Tables[0];
      if (resultTable.Rows.Count == 0)
      {
        return null;
      }
      return resultTable.Rows[0];
    }

    public int GetNewIncidentSequenceNumber()
    {
      string strSQL = "SELECT INCIDENT_REPORT_SEQ.NEXTVAL AS ir_seq FROM DUAL";
      DataSet resultDataSet = base.ExecuteSelect(strSQL);
      return Convert.ToInt32(resultDataSet.Tables[0].Rows[0][0] );
    }

    public void UpdateInjuredWorkerRecordWithHtml(int incidentNumber, string html)
    {
      string sql = string.Format("select incident_report_confirmation, incident_report_id from ir_worker where incident_report_id = {0}", incidentNumber);
      WriteStringToOracleLob(sql, html);
    }

    public void UpdateMasterStatus(string newAppId, string oldAppId, string applicationName)
    {
      string sql = "UPDATE MASTER_STATUS SET user_end_time = :now, APPLICATION_ID = :newAppID where application_id = TO_CHAR(:oldAppId) and application_name = :applicationName ";
      string[] parmNames = new string[] { ":now", ":newAppId", ":oldAppId", ":applicationName" };
      object[] parmValues = new object[] { DateTime.Now, newAppId, oldAppId, applicationName };
      ParameterDirection[] parmDirs = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input };
      var results = base.ExecuteNonQuery(sql, parmNames, parmValues, parmDirs);
    }

    private void WriteStringToOracleLob(string sql, string lob)
        {
            OracleDataAdapter myAdapter = new OracleDataAdapter(sql, base.ConnectionString);
            DataTable myTable = new DataTable();
            myAdapter.Fill(myTable);
            OracleCommandBuilder mybuilder = new OracleCommandBuilder(myAdapter);
            myTable.Rows[0][0] = lob;
            myAdapter.Update(myTable);
            //DataSet d = base.ExecuteSelect(sql);
            //foreach(DataRow row in d.Tables[0].Rows)
            //{
            //    string updateSql = $"UPDATE ir_worker SET incident_report_confirmation = :lob where incident_report_id = {row[1]}";
            //    //Adding To version to DB
            //    using (OracleConnection con = new OracleConnection(base.ConnectionString))
            //    {
            //        using (OracleCommand cmd = new OracleCommand(updateSql, con))
            //        {

            //            OracleParameter lobParameter = new OracleParameter();
            //            lobParameter.OracleDbType = OracleDbType.Clob;
            //            lobParameter.ParameterName = ":lob";
            //            lobParameter.Value = lob;

            //            //We are passing Name and Blob byte data as Oracle parameters.
            //            cmd.Parameters.Add(lobParameter);

            //            //Open connection and execute insert query.
            //            con.Open();
            //            cmd.ExecuteNonQuery();
            //        }
            //        con.Close();
            //    }
            //}
        }

  }
}
