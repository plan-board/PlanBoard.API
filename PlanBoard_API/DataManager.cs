using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;

namespace PlanBoard_API
{
    public class DataManager
    {
        private OleDbConnection oOleConnection;
        private OleDbDataAdapter oOleAdapter;
        private SqlConnection oSQLConnection;
        private SqlCommand oSqlCommand;

        private DataSet oSqlAsyncDataSet;
        private string sConnectionString = string.Empty;
        private DBProvider eProvider = DBProvider.SQLServer;

        protected int _CommandTimeout_Integer = 0;
        public int CommandTimeout
        {
            get { return _CommandTimeout_Integer; }
            set { _CommandTimeout_Integer = value; }
        }

        #region "### Constructors ###"

        public DataManager()
        {
            sConnectionString = DBConnectionString;
        }

        public DataManager(string ConnectionString)
        {
            sConnectionString = ConnectionString;
        }

        public DataManager(string ConnectionString, DBProvider ProviderType)
        {
            sConnectionString = ConnectionString;
            eProvider = ProviderType;
        }

        #endregion

        #region "### Connection Procedures ###"

        public bool Connect()
        {
            bool bReturnVal = false;
            try
            {
                switch (eProvider)
                {
                    case DBProvider.SQLServer:
                        oSQLConnection = new SqlConnection(sConnectionString);
                        oSQLConnection.Open();
                        if (oSQLConnection.State == System.Data.ConnectionState.Open)
                        {
                            bReturnVal = true;
                        }
                        break;
                    case DBProvider.OLEDB:
                        oOleConnection = new OleDbConnection(sConnectionString);
                        oOleConnection.Open();
                        if (oOleConnection.State == System.Data.ConnectionState.Open)
                        {
                            bReturnVal = true;
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                //Logger.WriteLog(ex);
            }
            return bReturnVal;
        }

        public bool CheckConnectionState()
        {
            bool bReturnVal = false;
            try
            {
                switch (eProvider)
                {
                    case DBProvider.SQLServer:
                        if (oSQLConnection.State == System.Data.ConnectionState.Open)
                        {
                            bReturnVal = true;
                        }
                        break;
                    case DBProvider.OLEDB:
                        if (oOleConnection.State == System.Data.ConnectionState.Open)
                        {
                            bReturnVal = true;
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                // Logger.WriteLog(ex);
            }
            return bReturnVal;
        }

        public void Close()
        {
            try
            {
                switch (eProvider)
                {
                    case DBProvider.SQLServer:
                        oSQLConnection.Close();
                        oSQLConnection.Dispose();
                        break;
                    case DBProvider.OLEDB:
                        oOleConnection.Close();
                        oOleConnection.Dispose();
                        break;
                }
            }
            catch (Exception ex)
            {
                //  Logger.WriteLog(ex);
            }
        }

        #endregion

        public void ExecuteNonQuery(string SQLQuery)
        {
            try
            {
                switch (eProvider)
                {
                    case DBProvider.SQLServer:
                        SqlCommand oSqlCommand1 = new SqlCommand(SQLQuery, oSQLConnection);
                        oSqlCommand1.CommandTimeout = _CommandTimeout_Integer;
                        oSqlCommand1.ExecuteNonQuery();
                        oSqlCommand1.Dispose();
                        break;
                    case DBProvider.OLEDB:
                        OleDbCommand oSqlCommand = new OleDbCommand(SQLQuery, oOleConnection);
                        oSqlCommand.ExecuteNonQuery();
                        oSqlCommand.Dispose();
                        break;
                }
            }
            catch (Exception ex)
            {
                //  Logger.WriteLog(ex);
            }
        }

        public DataTable ExecuteDT(string SQLQuery)
        {
            return ExecuteDT(SQLQuery, null, null);
        }

        public DataTable ExecuteDT(string SQLQuery, string[] Parameters, string[] Values)
        {
            DataTable oSqlDataTable = new DataTable();
            //Try
            if (((Parameters != null)) && ((Values != null)) && (!(Parameters.Length == Values.Length)))
            {
                //Parameter count does not equal value count
                oSqlDataTable = null;
            }
            else
            {
                switch (eProvider)
                {
                    case DBProvider.SQLServer:
                        SqlCommand oSqlCommand1 = new SqlCommand(SQLQuery, oSQLConnection);
                        oSqlCommand1.CommandType = System.Data.CommandType.StoredProcedure;
                        oSqlCommand1.CommandTimeout = _CommandTimeout_Integer;
                        if (((Parameters != null)) && ((Values != null)))
                        {
                            for (int iLoop = 0; iLoop <= Parameters.Length - 1; iLoop++)
                            {
                                oSqlCommand1.Parameters.AddWithValue(Parameters[iLoop], Values[iLoop]);
                            }
                        }
                        dynamic oSqlDataAdapter = new SqlDataAdapter(oSqlCommand1);
                        oSqlDataAdapter.Fill(oSqlDataTable);
                        oSqlCommand1.Dispose();
                        break;
                    case DBProvider.OLEDB:
                        OleDbCommand oSqlCommand = new OleDbCommand(SQLQuery, oOleConnection);
                        oOleAdapter = new OleDbDataAdapter(oSqlCommand);
                        oOleAdapter.Fill(oSqlDataTable);
                        oSqlCommand.Dispose();
                        break;
                }
            }
            //Catch ex As Exception
            //    oSqlDataTable = Nothing
            //End Try
            return oSqlDataTable;
        }

        public DataSet Execute(string SQLQuery)
        {
            return Execute(SQLQuery, null, null);
        }
        public DataSet ExecuteProcWithoutParam(string SQLQuery)
        {
            DataSet oSqlDataSet = new DataSet();
            try
            {

                switch (eProvider)
                {
                    case DBProvider.SQLServer:
                        SqlCommand oSqlCommand1 = new SqlCommand(SQLQuery, oSQLConnection);
                        oSqlCommand1.CommandType = System.Data.CommandType.StoredProcedure;
                        oSqlCommand1.CommandTimeout = _CommandTimeout_Integer;

                        dynamic oSqlDataAdapter = new SqlDataAdapter(oSqlCommand1);
                        oSqlDataAdapter.Fill(oSqlDataSet);
                        oSqlCommand1.Dispose();
                        break;
                    case DBProvider.OLEDB:
                        OleDbCommand oSqlCommand = new OleDbCommand(SQLQuery, oOleConnection);
                        oOleAdapter = new OleDbDataAdapter(oSqlCommand);
                        oOleAdapter.Fill(oSqlDataSet);
                        oSqlCommand.Dispose();
                        break;
                }

            }
            catch (Exception ex)
            {
                oSqlDataSet = null;
                //Logger.WriteLog(ex);
            }
            return oSqlDataSet;
        }

        public DataSet Execute(string SQLQuery, string[] Parameters, string[] Values)
        {
            DataSet oSqlDataSet = new DataSet();
            try
            {
                if (((Parameters != null)) && ((Values != null)) && (!(Parameters.Length == Values.Length)))
                {
                    //Parameter count does not equal value count
                    oSqlDataSet = null;
                }
                else
                {
                    switch (eProvider)
                    {
                        case DBProvider.SQLServer:
                            SqlCommand oSqlCommand1 = new SqlCommand(SQLQuery, oSQLConnection);
                            oSqlCommand1.CommandType = System.Data.CommandType.StoredProcedure;
                            oSqlCommand1.CommandTimeout = _CommandTimeout_Integer;
                            if (((Parameters != null)) && ((Values != null)))
                            {
                                for (int iLoop = 0; iLoop <= Parameters.Length - 1; iLoop++)
                                {
                                    oSqlCommand1.Parameters.AddWithValue(Parameters[iLoop], Values[iLoop]);
                                }
                            }
                            dynamic oSqlDataAdapter = new SqlDataAdapter(oSqlCommand1);
                            oSqlDataAdapter.Fill(oSqlDataSet);
                            oSqlCommand1.Dispose();
                            break;
                        case DBProvider.OLEDB:
                            OleDbCommand oSqlCommand = new OleDbCommand(SQLQuery, oOleConnection);
                            oOleAdapter = new OleDbDataAdapter(oSqlCommand);
                            oOleAdapter.Fill(oSqlDataSet);
                            oSqlCommand.Dispose();
                            break;
                    }
                }
            }
            catch
            {
                oSqlDataSet = null;
                throw;
                // Logger.WriteLog(ex);
            }
            return oSqlDataSet;
        }

        public IAsyncResult BeginExecuteAsync(string SQLQuery, AsyncCallback cb, object state)
        {
            return BeginExecuteAsync(SQLQuery, null, null, cb, state);
        }

        public IAsyncResult BeginExecuteAsync(string SQLQuery, string[] Parameters, string[] Values, AsyncCallback cb, object state)
        {
            oSqlCommand = new SqlCommand(SQLQuery, oSQLConnection);
            oSqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            if (((Parameters != null)) && ((Values != null)) && (Parameters.Length == Values.Length))
            {
                for (int iLoop = 0; iLoop <= Parameters.Length - 1; iLoop++)
                {
                    oSqlCommand.Parameters.AddWithValue(Parameters[iLoop], Values[iLoop]);
                }
            }
            return oSqlCommand.BeginExecuteReader(cb, state);
        }

        public SqlDataReader EndExecuteAsync(IAsyncResult ar)
        {
            return oSqlCommand.EndExecuteReader(ar);
        }

        public enum DBProvider
        {
            SQLServer,
            OLEDB
        }
        public static string DBConnectionString
        {
            get;
            set;

        }
    }
}