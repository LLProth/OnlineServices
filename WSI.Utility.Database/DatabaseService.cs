using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;

namespace WSI.Utility.Database
{
    public class DatabaseService : IDatabaseService, IDisposable
    {
        public DatabaseService(string connectionString)
        {
            ConnectionString = ConfigurationManager.ConnectionStrings[connectionString].ConnectionString;
        }


        public DatabaseService() { }

        public string ConnectionString
        {
            get
            {
                return connectionString;
            }
            set
            {
                connectionString = value;
                connection = new OracleConnection(value);
            }
        }

        private string connectionString;
        private OracleTransaction transaction;
        private OracleConnection connection;

        public bool ExecuteDelete(string sql)
        {
            string[] parameterNames = { };
            object[] parameterValues = { };

            return ExecuteDelete(sql, parameterNames, parameterValues);
        }

        public bool ExecuteDelete(string sql, string[] parameterNames, object[] parameterValues)
        {
            List<ParameterDirection> parameterDirections = new List<ParameterDirection>();
            foreach (string parameterName in parameterNames)
            {
                parameterDirections.Add(ParameterDirection.Input);
            }

            ExecuteDelete(sql, parameterNames, parameterValues, parameterDirections.ToArray());
            return true;

        }

        public Dictionary<string, object> ExecuteDelete(string sql, string[] parameterNames, object[] parameterValues, ParameterDirection[] parameterDirections)
        {
            return ExecuteQuery(sql, parameterNames, parameterValues, parameterDirections);
        }

        public bool ExecuteInsert(string sql)
        {
            string[] parameterNames = { };
            object[] parameterValues = { };

            return ExecuteInsert(sql, parameterNames, parameterValues);
        }

        public bool ExecuteInsert(string sql, string[] parameterNames, object[] parameterValues)
        {
            List<ParameterDirection> parameterDirections = new List<ParameterDirection>();
            foreach (string parameterName in parameterNames)
            {
                parameterDirections.Add(ParameterDirection.Input);
            }

            ExecuteInsert(sql, parameterNames, parameterValues, parameterDirections.ToArray());
            return true;
        }

        public Dictionary<string, object> ExecuteInsert(string sql, string[] parameterNames, object[] parameterValues, ParameterDirection[] parameterDirections)
        {
            return ExecuteQuery(sql, parameterNames, parameterValues, parameterDirections);
        }

        public Dictionary<string, object> ExecuteProcedure(string procedureName)
        {
            string[] parameterNames = { };
            object[] parameterValues = { };

            return ExecuteProcedure(procedureName, parameterNames, parameterValues);
        }

        public Dictionary<string, object> ExecuteProcedure(string procedureName, string[] parameterNames, object[] parameterValues)
        {
            List<ParameterDirection> parameterDirections = new List<ParameterDirection>();
            foreach (string parameterName in parameterNames)
            {
                parameterDirections.Add(ParameterDirection.Input);
            }

            return ExecuteProcedure(procedureName, parameterNames, parameterValues, parameterDirections.ToArray());
        }

        public Dictionary<string, object> ExecuteProcedure(string procedureName, string[] parameterNames, object[] parameterValues, ParameterDirection[] parameterDirections)
        {
            Dictionary<string, object> output = new Dictionary<string, object>();


            using (OracleCommand oraCmd = new OracleCommand(procedureName, connection))
            {
                oraCmd.CommandType = CommandType.StoredProcedure;
                oraCmd.BindByName = true;
                DatabaseTools.AddParmsToCommand(oraCmd, parameterNames, parameterValues, parameterDirections);
                OpenConnection();
                oraCmd.ExecuteNonQuery();

                // set output values
                foreach (OracleParameter parm in oraCmd.Parameters)
                {
                    if (parm.Direction == ParameterDirection.Output)
                        output.Add(parm.ParameterName, parm.Value);
                }
            }

            return output;
        }

        public Dictionary<string, object> ExecuteProcedure(string procedureName, string[] parameterNames, object[] parameterValues, OracleDbType[] parameterTypes)
        {
            Dictionary<string, object> output = new Dictionary<string, object>();


            using (OracleCommand oraCmd = new OracleCommand(procedureName, connection))
            {
                oraCmd.CommandType = CommandType.StoredProcedure;
                DatabaseTools.AddParmsToCommand(oraCmd, parameterNames, parameterValues, parameterTypes);
                OpenConnection();
                oraCmd.ExecuteNonQuery();

                // set output values
                foreach (OracleParameter parm in oraCmd.Parameters)
                {
                    if (parm.Direction == ParameterDirection.Output)
                        output.Add(parm.ParameterName, parm.Value);
                }
            }

            return output;
        }

        public Dictionary<string, object> ExecuteProcedure(string procedureName, string[] parameterNames, object[] parameterValues, ParameterDirection[] parameterDirections, OracleDbType[] parameterTypes)
        {
            Dictionary<string, object> output = new Dictionary<string, object>();


            using (OracleCommand oraCmd = new OracleCommand(procedureName, connection))
            {
                oraCmd.CommandType = CommandType.StoredProcedure;
                DatabaseTools.AddParmsToCommand(oraCmd, parameterNames, parameterValues, parameterDirections, parameterTypes);
                OpenConnection();
                oraCmd.ExecuteNonQuery();

                // set output values
                foreach (OracleParameter parm in oraCmd.Parameters)
                {
                    if (parm.Direction == ParameterDirection.Output)
                        output.Add(parm.ParameterName, parm.Value);
                }
            }

            return output;
        }

        public DataTable ExecuteProcedureOutCursor(string procedureName, string[] parameterNames, object[] parameterValues, int numberOfOutputCursors)
        {
            DataSet output = new DataSet();


            using (OracleCommand oraCmd = new OracleCommand(procedureName, connection))
            {
                oraCmd.CommandType = CommandType.StoredProcedure;

                // set up parameters - any out will be ref cursors
                if (parameterNames.Length != parameterValues.Length)
                    throw new ArgumentException("Number of Parameter Names did not match number of Parameter Values.");

                for (int i = 0; i < parameterNames.Length; i++)
                {
                    OracleParameter parm = new OracleParameter
                    {
                        ParameterName = parameterNames[i],
                        Value = parameterValues[i],
                        Direction = ParameterDirection.Input
                    };
                    oraCmd.Parameters.Add(parm);
                }

                for (int i = 1; i <= numberOfOutputCursors; i++)
                {
                    oraCmd.Parameters.Add(string.Format("tab{0}", i), OracleDbType.RefCursor, ParameterDirection.Output);
                }

                OpenConnection();
                using (OracleDataAdapter oraAdapter = new OracleDataAdapter(oraCmd))
                {
                    oraAdapter.Fill(output);
                }
            }

            return output.Tables[0];
        }

        public DataTable ExecuteSelect(string sql)
        {
            string[] parameterNames = { };
            object[] parameterValues = { };

            return ExecuteSelect(sql, parameterNames, parameterValues);
        }

        public DataTable ExecuteSelect(string sql, string[] parameterNames, object[] parameterValues)
        {
            List<ParameterDirection> parameterDirections = new List<ParameterDirection>();
            foreach (string parameterName in parameterNames)
            {
                parameterDirections.Add(ParameterDirection.Input);
            }

            return ExecuteSelect(sql, parameterNames, parameterValues, parameterDirections.ToArray());

        }

        public DataTable ExecuteSelect(string sql, string[] parameterNames, object[] parameterValues, ParameterDirection[] parameterDirections)
        {
            if (parameterNames != null && parameterValues != null && parameterDirections != null)
            {
                if (parameterNames.Length != parameterValues.Length || parameterNames.Length != parameterDirections.Length)
                {
                    throw new ArgumentException("The number of Parameter Names, Parameter Values, and Parameter Directions are not all equal.");
                }
            }
            DataSet output = new DataSet();

            using (OracleCommand oraCmd = new OracleCommand(sql, connection))
            {
                oraCmd.CommandType = CommandType.Text;
                oraCmd.BindByName = true;
                DatabaseTools.AddParmsToCommand(oraCmd, parameterNames, parameterValues, parameterDirections);
                OpenConnection();
                using (OracleDataAdapter oraAdapter = new OracleDataAdapter(oraCmd))
                {
                    oraAdapter.Fill(output);
                }
            }

            return output.Tables[0];
        }

        public bool ExecuteUpdate(string sql)
        {
            string[] parameterNames = { };
            object[] parameterValues = { };

            return ExecuteUpdate(sql, parameterNames, parameterValues);
        }

        public bool ExecuteUpdate(string sql, string[] parameterNames, object[] parameterValues)
        {
            List<ParameterDirection> parameterDirections = new List<ParameterDirection>();
            foreach (string parameterName in parameterNames)
            {
                parameterDirections.Add(ParameterDirection.Input);
            }

            ExecuteUpdate(sql, parameterNames, parameterValues, parameterDirections.ToArray());
            return true;
        }

        public Dictionary<string, object> ExecuteUpdate(string sql, string[] parameterNames, object[] parameterValues, ParameterDirection[] parameterDirections)
        {
            return ExecuteQuery(sql, parameterNames, parameterValues, parameterDirections);
        }

        private Dictionary<string, object> ExecuteQuery(string sql, string[] parameterNames, object[] parameterValues, ParameterDirection[] parameterDirections)
        {
            Dictionary<string, object> output = new Dictionary<string, object>();
            if (parameterNames != null && parameterValues != null && parameterDirections != null)
            {
                if (parameterNames.Length != parameterValues.Length || parameterNames.Length != parameterDirections.Length)
                {
                    throw new ArgumentException("The number of Parameter Names, Parameter Values, and Parameter Directions are not all equal.");
                }
            }


            using (OracleCommand oraCmd = new OracleCommand(sql, connection))
            {
                oraCmd.CommandType = CommandType.Text;
                DatabaseTools.AddParmsToCommand(oraCmd, parameterNames, parameterValues, parameterDirections);
                OpenConnection();
                oraCmd.ExecuteNonQuery();

                // set output values
                foreach (OracleParameter parm in oraCmd.Parameters)
                {
                    if (parm.Direction == ParameterDirection.Output)
                        output.Add(parm.ParameterName, parm.Value);
                }
            }

            return output;
        }

        public string ExecuteFunction(string functionName, string[] functionParameters)
        {
            string sql = "SELECT {0}{1} FROM dual";

            if (functionParameters?.Length > 0)
            {
                string parameters = "(" + string.Join(", ", functionParameters) + ")";
                sql = string.Format(sql, functionName, parameters);
            }
            else
            {
                sql = string.Format(sql, functionName, "");
            }
            DataRow row = ExecuteSelect(sql).Rows[0];
            return row[0].ToString();
        }

        public void CreateTransaction()
        {
            OpenConnection();
            transaction = connection.BeginTransaction();
        }

        public void CommitTransaction()
        {
            OpenConnection();
            transaction.Commit();
            transaction = null;
        }

        public void RollbackTransaction()
        {
            OpenConnection();
            transaction.Rollback();
            transaction = null;
        }

        private void OpenConnection()
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    connection.Close();
                    transaction = null;
                    connection = null;
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~DatabaseService() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
