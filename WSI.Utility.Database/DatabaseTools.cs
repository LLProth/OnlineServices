using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace WSI.Utility.Database
{
    public static class DatabaseTools
    {
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
        public static object ValueOrDbNull(object inValue)
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
        public static object YNfromBool(bool? inputValue)
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
        public static object YNfromBool(bool inputValue)
        {
            if (inputValue)
                return "Y";
            else
                return "N";
        }

        public static void AddParmsToCommand(OracleCommand oraCmd, string[] parmNames, object[] parmValues)
        {

            if (parmNames.Length != parmValues.Length)
                throw new ArgumentException("Number of parm names did not match number of values or direction");

            // create the parm directions array with all input
            List<ParameterDirection> listDirs = new List<ParameterDirection>();
            for (int n = 0; n < parmNames.Length; n++)
                listDirs.Add(ParameterDirection.Input);

            List<OracleDbType> listParms = new List<OracleDbType>();
            for (int n = 0; n < parmNames.Length; n++)
                listParms.Add(OracleDbType.Varchar2);

            // call master add method
            AddParmsToCommand(oraCmd, parmNames, parmValues, listDirs.ToArray(), listParms.ToArray());

        }

        public static void AddParmsToCommand(OracleCommand oraCmd, string[] parmNames, object[] parmValues, ParameterDirection[] parmDirections)
        {

            if ((parmNames.Length != parmValues.Length) || (parmNames.Length != parmDirections.Length))
                throw new ArgumentException("Number of parm names did not match number of values or direction");

            List<OracleDbType> listType = new List<OracleDbType>();
            for (int n = 0; n < parmNames.Length; n++)
                listType.Add(OracleDbType.Varchar2);
            
            AddParmsToCommand(oraCmd, parmNames, parmValues, parmDirections, listType.ToArray());

        }

        public static void AddParmsToCommand(OracleCommand oraCmd, string[] parmNames, object[] parmValues, OracleDbType[] parmType)
        {

            if ((parmNames.Length != parmValues.Length) || (parmNames.Length != parmType.Length))
                throw new ArgumentException("Number of parm names did not match number of values or types");

            List<ParameterDirection> listDirs = new List<ParameterDirection>();
            for (int n = 0; n < parmNames.Length; n++)
                listDirs.Add(ParameterDirection.Input);

            AddParmsToCommand(oraCmd, parmNames, parmValues, listDirs.ToArray(), parmType);

        }

        public static void AddParmsToCommand(OracleCommand oraCmd, string[] parmNames, object[] parmValues, ParameterDirection[] parmDirections, OracleDbType[] parmType)
        {

            if ((parmNames.Length != parmValues.Length) || (parmNames.Length != parmDirections.Length))
                throw new ArgumentException("Number of parm names did not match number of values, directions, or types");

            for (int i = 0; i < parmNames.Length; i++)
            {
                OracleParameter parm = new OracleParameter
                {
                    ParameterName = parmNames[i],
                    Value = parmValues[i],
                    Direction = parmDirections[i],
                    OracleDbType = parmType[i]
                };
                oraCmd.Parameters.Add(parm);
            }

        }
    }
}
