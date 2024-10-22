using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data;

namespace WSI.Utility.Database
{
    public interface IDatabaseService
    {
        string ConnectionString { get; set; }

        bool ExecuteUpdate(string sql);
        bool ExecuteUpdate(string sql, string[] parameterNames, object[] parameterValues);
        Dictionary<string, object> ExecuteUpdate(string sql, string[] parameterNames, object[] parameterValues, ParameterDirection[] parameterDirections);

        bool ExecuteInsert(string sql);
        bool ExecuteInsert(string sql, string[] parameterNames, object[] parameterValues);
        Dictionary<string, object> ExecuteInsert(string sql, string[] parameterNames, object[] parameterValues, ParameterDirection[] parameterDirections);

        bool ExecuteDelete(string sql);
        bool ExecuteDelete(string sql, string[] parameterNames, object[] parameterValues);
        Dictionary<string, object> ExecuteDelete(string sql, string[] parameterNames, object[] parameterValues, ParameterDirection[] parameterDirections);

        DataTable ExecuteSelect(string sql);
        DataTable ExecuteSelect(string sql, string[] parameterNames, object[] parameterValues);
        DataTable ExecuteSelect(string sql, string[] parameterNames, object[] parameterValues, ParameterDirection[] parameterDirections);

        Dictionary<string, object> ExecuteProcedure(string procedureName);
        Dictionary<string, object> ExecuteProcedure(string procedureName, string[] parameterNames, object[] parameterValues);
        Dictionary<string, object> ExecuteProcedure(string procedureName, string[] parameterNames, object[] parameterValues, ParameterDirection[] parameterDirections);
        Dictionary<string, object> ExecuteProcedure(string procedureName, string[] parameterNames, object[] parameterValues, OracleDbType[] parameterTypes);
        Dictionary<string, object> ExecuteProcedure(string procedureName, string[] parameterNames, object[] parameterValues, ParameterDirection[] parameterDirections, OracleDbType[] parameterTypes);

        DataTable ExecuteProcedureOutCursor(string procedureName, string[] parameterNames, object[] parameterValues, int numberOfOutputCursors);

        string ExecuteFunction(string functionName, string[] functionParameters);

        void CreateTransaction();
        void CommitTransaction();
        void RollbackTransaction();

    }
}
