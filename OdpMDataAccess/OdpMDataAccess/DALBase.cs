using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace OdpMDataAccess
{
    public class DALBase
  {

    /// <summary>
    /// constructor
    /// </summary>
    /// <param name="connectionStringsConfigKey">key in appsettings that has connect string</param>
    /// <param name="isWeb">true if web app, false if not..  not used anymore</param>
    /// <remarks>Use this constructor if you have a ConnectionsStrings section key name.</remarks>
    protected DALBase(string connectionStringsConfigKey, bool isWeb)
    {
      try
      {
        ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings[connectionStringsConfigKey].ConnectionString;

      }
      catch (Exception)
      {
        string message = string.Format("ConnectionString key {0} not found in configuration", connectionStringsConfigKey);
        throw new ArgumentException(message);
      }
    }

    /// <summary>
    /// constructor
    /// </summary>
    /// <param name="connectionString">oracle connection string to use to connect</param>
    /// <remarks>Use this constructor if you have the entire connection string to pass in</remarks>
    protected DALBase(string connectionString)
    {
      ConnectionString = connectionString;
    }

    /// <summary>
    /// current oracle connect string
    /// </summary>
    protected string ConnectionString { get; set; }

    /// <summary>
    /// Execute a non-query statement (insert/update/delete/)
    /// </summary>
    /// <param name="sql">valid non query pl sql</param>
    /// <returns>true if no exceptions thrown</returns>
    protected bool ExecuteNonQuery(string sql)
    {
      using (OracleConnection oraConn = new OracleConnection(ConnectionString))
      {
        using (OracleCommand oraCmd = new OracleCommand(sql, oraConn))
        {
          oraCmd.CommandType = System.Data.CommandType.Text;
          oraConn.Open();
          oraCmd.ExecuteNonQuery();
        }
                oraConn.Close();

            }

            return true;
    }

    /// <summary>
    /// Execute a non-query statement (insert/update/delete/) with parameters
    /// </summary>
    /// <param name="sql">valid non query pl sql</param>
    /// <param name="parmNames">an string array of parameter names</param>
    /// <param name="parmValues">an object array of parameter values</param>
    /// <returns>true if no exceptions thrown</returns>
    protected bool ExecuteNonQuery(string sql, string[] parmNames, object[] parmValues)
    {

        using (OracleConnection oraConn = new OracleConnection(ConnectionString))
        {
            using (OracleCommand oraCmd = new OracleCommand(sql, oraConn))
            {
                oraCmd.CommandType = System.Data.CommandType.Text;
                AddParmsToCommand(oraCmd, parmNames, parmValues);
                oraConn.Open();
                int s = oraCmd.ExecuteNonQuery();
            }
            oraConn.Close();
        }

        return true;
    }

    /// <summary>
    /// Execute a non-query statement (insert/update/delete/) with parameters including optionally out parms
    /// </summary>
    /// <param name="sql">valid non query pl sql</param>
    /// <param name="parmNames">an string array of parameter names</param>
    /// <param name="parmValues">an object array of parameter values</param>
    /// <param name="parmDirections">an ParameterDirection array specifying if each parm is in out or inout</param>
    /// <returns>true if no exceptions thrown</returns>
    protected Dictionary<string, object> ExecuteNonQuery(string sql, string[] parmNames, object[] parmValues, ParameterDirection[] parmDirections)
    {
      Dictionary<string, object> output = new Dictionary<string, object>();

      using (OracleConnection oraConn = new OracleConnection(ConnectionString))
      {
        using (OracleCommand oraCmd = new OracleCommand(sql, oraConn))
        {
          oraCmd.CommandType = System.Data.CommandType.Text;
          AddParmsToCommand(oraCmd, parmNames, parmValues, parmDirections);
          oraConn.Open();
          oraCmd.ExecuteNonQuery();

          // set output values
          foreach (OracleParameter parm in oraCmd.Parameters)
          {
            if (parm.Direction == ParameterDirection.Output)
              output.Add(parm.ParameterName, parm.Value);
          }
        }
                oraConn.Close();
      }

      return output;
    }

    /// <summary>
    /// Execute a stored procedure or package procedure.
    /// </summary>
    /// <param name="procName">procedure name or pkg.procedureName</param>
    /// <param name="parmNames">an string array of parameter names</param>
    /// <param name="parmValues">an object array of parameter values</param>
    /// <returns>Dictionary keyed by output parm name or null if no output parms</returns>
    protected Dictionary<string, object> ExecuteProc(string procName, string[] parmNames, object[] parmValues)
    {
      List<ParameterDirection> parameterDirectionList = new List<ParameterDirection>();
      // since no parmDirs provided with this overload - assume all are input
      for (int n = 0; n < parmNames.Length; n++)
      {
        parameterDirectionList.Add(ParameterDirection.Input);
      }

      return ExecuteProc(procName, parmNames, parmValues, parameterDirectionList.ToArray());
    }

    /// <summary>
    /// Execute a stored procedure or package procedure.
    /// </summary>
    /// <param name="procName">procedure name or pkg.procedureName</param>
    /// <param name="parmNames">an string array of parameter names</param>
    /// <param name="parmValues">an object array of parameter values</param>
    /// <param name="parmDirections">an ParameterDirection array specifying if each parm is in out or inout</param>
    /// <returns>Dictionary keyed by output parm name or null if no output parms</returns>
    protected Dictionary<string, object> ExecuteProc(string procName, string[] parmNames, object[] parmValues, ParameterDirection[] parmDirections)
    {
      Dictionary<string, object> output = new Dictionary<string, object>();

      using (OracleConnection oraConn = new OracleConnection(ConnectionString))
      {
        using (OracleCommand oraCmd = new OracleCommand(procName, oraConn))
        {
          oraCmd.CommandType = System.Data.CommandType.StoredProcedure;
          AddParmsToCommand(oraCmd, parmNames, parmValues, parmDirections);
          oraConn.Open();
          oraCmd.ExecuteNonQuery();

          // set output values
          foreach (OracleParameter parm in oraCmd.Parameters)
          {
            if (parm.Direction == ParameterDirection.Output)
              output.Add(parm.ParameterName, parm.Value);
          }
        }
                oraConn.Close();

            }

            return output;

    }

    /// <summary>
    /// Execute specified procedure which will return specifed number of refcursor out parms
    /// </summary>
    /// <param name="procName">name of package.procedure</param>
    /// <param name="inParmNames">input parameter names or an empty array if no inputs</param>
    /// <param name="inParmValues">input parmater values or an empty array if no inputs</param>
    /// <param name="numberOfOutputCursors">number of expected output cursors which will be returned as datatables in the returned dataset</param>
    /// <returns>Dataset with a table for each refCursor that the proc returns</returns>
    protected DataSet ExecuteProc_RefCursorResults(string procName, string[] inParmNames, object[] inParmValues, int numberOfOutputCursors)
    {
      DataSet output = new DataSet();

      using (OracleConnection oraConn = new OracleConnection(ConnectionString))
      {
        using (OracleCommand oraCmd = new OracleCommand(procName, oraConn))
        {
          oraCmd.CommandType = System.Data.CommandType.StoredProcedure;

          // set up parameters - any out will be ref cursors
          if (inParmNames.Length != inParmValues.Length)
            throw new ArgumentException("Number of parm names did not match number of values or direction");

          for (int i = 0; i < inParmNames.Length; i++)
          {
            OracleParameter parm = new OracleParameter();
            parm.ParameterName = inParmNames[i];
            parm.Value = inParmValues[i];
            parm.Direction = ParameterDirection.Input;
            oraCmd.Parameters.Add(parm);
          }

          for (int i = 1; i <= numberOfOutputCursors; i++)
          {
            oraCmd.Parameters.Add(string.Format("tab{0}", i), OracleDbType.RefCursor, ParameterDirection.Output);
          }

          oraConn.Open();
          using (OracleDataAdapter oraAdapter = new OracleDataAdapter(oraCmd))
          {
            oraAdapter.Fill(output);
          }
        }
                oraConn.Close();

            }
            return output;
    }

    /// <summary>
    /// Execute the specified sql statement
    /// </summary>
    /// <param name="sql">Oracle Select statement</param>
    /// <returns>Dataset with one table containing the results of the select</returns>
    protected DataSet ExecuteSelect(string sql)
    {
      string[] pNames = { };
      object[] pVals = { };
      ParameterDirection[] pDirs = { };
      return ExecuteSelect(sql, pNames, pVals, pDirs);

    }

    /// <summary>
    /// Execute the specified sql statement
    /// </summary>
    /// <param name="sql">Oracle Select statement</param>
    /// <param name="pNames">string array of parameter names</param>
    /// <param name="pVals">object array of parameter values</param>
    /// <returns>Dataset with one table containing the results of the select</returns>
    protected DataSet ExecuteSelect(string sql, string[] pNames, object[] pVals)
    {

      // create the parm directions array with all input
      List<ParameterDirection> listDirs = new List<ParameterDirection>();
      for (int n = 0; n < pNames.Length; n++)
        listDirs.Add(ParameterDirection.Input);
      return ExecuteSelect(sql, pNames, pVals, listDirs.ToArray());

    }

    /// <summary>
    /// Execute the specified sql statement
    /// </summary>
    /// <param name="sql">Oracle Select statement</param>
    /// <param name="pNames">string array of parameter names</param>
    /// <param name="pVals">object array of parameter values</param>
    /// <param name="pDirs">an ParameterDirection array specifying if each parm is in out or inout</param>
    /// <returns>Dataset with one table containing the results of the select</returns>
    protected DataSet ExecuteSelect(string sql, string[] pNames, object[] pVals, ParameterDirection[] pDirs)
    {

      DataSet output = new DataSet();

      using (OracleConnection oraConn = new OracleConnection(ConnectionString))
      {
        using (OracleCommand oraCmd = new OracleCommand(sql, oraConn))
        {
          oraCmd.CommandType = CommandType.Text;
          AddParmsToCommand(oraCmd, pNames, pVals, pDirs);
          oraConn.Open();
          using (OracleDataAdapter oraAdapter = new OracleDataAdapter(oraCmd))
          {
            oraAdapter.Fill(output);
          }
        }
                oraConn.Close();

            }

            return output;

    }

    private void AddParmsToCommand(OracleCommand oraCmd, string[] parmNames, object[] parmValues)
    {

      if (parmNames.Length != parmValues.Length)
        throw new ArgumentException("Number of parm names did not match number of values or direction");

      // create the parm directions array with all input
      List<ParameterDirection> listDirs = new List<ParameterDirection>();
      for (int n = 0; n < parmNames.Length; n++)
        listDirs.Add(ParameterDirection.Input);

      // call master add method
      AddParmsToCommand(oraCmd, parmNames, parmValues, listDirs.ToArray());

    }

    private void AddParmsToCommand(OracleCommand oraCmd, string[] parmNames, object[] parmValues, ParameterDirection[] parmDirections)
    {

      if ((parmNames.Length != parmValues.Length) || (parmNames.Length != parmDirections.Length))
        throw new ArgumentException("Number of parm names did not match number of values or direction");

      for (int i = 0; i < parmNames.Length; i++)
      {
        OracleParameter parm = new OracleParameter();
        parm.ParameterName = parmNames[i];
        parm.Value = parmValues[i];
        parm.Direction = parmDirections[i];
        oraCmd.Parameters.Add(parm);
      }

    }

        public static DateTime? NulDateFromDbValue(object inValue)
        {
            DateTime? retValue = null;
            if (!inValue.Equals(DBNull.Value))
            {
                if (DateTime.TryParse(inValue.ToString(), out DateTime test))
                {
                    retValue = test;
                }
            }
            return retValue;
        }

    /// <summary>
    /// helper to convert possible null references to dbnull.value
    /// </summary>
    /// <param name="inValue">a nullable class object</param>
    /// <returns>if input is null then returns dbnull.value</returns>
    protected static object ValueOrDbNull(object inValue)
    {

      if (inValue == null)
        return DBNull.Value;
      else
        return inValue;

    }

    /// <summary>
    /// helper to convert bool? to "Y", "N", or dbnull.value
    /// </summary>
    /// <param name="inputValue">bool value to convert</param>
    /// <returns>"Y" if true, "N" if false, dbnull.value if null</returns>
    protected static object YNfromBool(bool? inputValue)
    {
      if (inputValue == null)
        return DBNull.Value;
      else
        return YNfromBool(inputValue.Value);
    }

    /// <summary>
    /// helper to convert bool to "Y" or "N"
    /// </summary>
    /// <param name="inputValue">bool value to convert</param>
    /// <returns>"Y" if true, "N" if false</returns>
    protected static object YNfromBool(bool inputValue)
    {
      if (inputValue)
        return "Y";
      else
        return "N";
    }

  }
}
