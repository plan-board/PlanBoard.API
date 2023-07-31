using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PB_API.DL
{
    public class DataManager
    {
        /// <summary>
        /// The _o OLE connection
        /// </summary>
        private OleDbConnection _oOleConnection;
        /// <summary>
        /// The _o OLE adapter
        /// </summary>
        private OleDbDataAdapter _oOleAdapter;
        /// <summary>
        /// The _o SQL connection
        /// </summary>
        private SqlConnection _oSQLConnection;
        /// <summary>
        /// The _o SQL command
        /// </summary>
        private SqlCommand _oSqlCommand;

        /// <summary>
        /// The _s connection string
        /// </summary>
        private readonly string _sConnectionString = string.Empty;
        /// <summary>
        /// The _e provider
        /// </summary>
        private readonly DbProvider _eProvider = DbProvider.SQLServer;

        /// <summary>
        /// The command timeout integer
        /// </summary>
        protected int CommandTimeoutInteger = 0;
        /// <summary>
        /// Gets or sets the command timeout.
        /// </summary>
        /// <value>
        /// The command timeout.
        /// </value>
        public int CommandTimeout
        {
            get { return CommandTimeoutInteger; }
            set { CommandTimeoutInteger = value; }
        }

        #region "### Constructors ###"

        /// <summary>
        /// Initializes a new instance of the <see cref="DataManager"/> class.
        /// </summary>
        public DataManager()
        {
            _sConnectionString = DbConnectionString;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataManager"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public DataManager(string connectionString)
        {
            _sConnectionString = connectionString;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataManager"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="providerType">Type of the provider.</param>
        public DataManager(string connectionString, DbProvider providerType)
        {
            _sConnectionString = connectionString;
            _eProvider = providerType;
        }

        #endregion

        #region "### Connection Procedures ###"

        /// <summary>
        /// Connects this instance.
        /// </summary>
        /// <returns></returns>
        public bool Connect()
        {
            bool bReturnVal = false;
            try
            {
                switch (_eProvider)
                {
                    case DbProvider.SQLServer:
                        _oSQLConnection = new SqlConnection(_sConnectionString);
                        _oSQLConnection.Open();
                        if (_oSQLConnection.State == ConnectionState.Open)
                        {
                            bReturnVal = true;
                        }
                        break;
                    case DbProvider.OLEDB:
                        _oOleConnection = new OleDbConnection(_sConnectionString);
                        _oOleConnection.Open();
                        if (_oOleConnection.State == ConnectionState.Open)
                        {
                            bReturnVal = true;
                        }
                        break;
                }
            }
            catch
            {
                //Logger.WriteLog(ex);
            }
            return bReturnVal;
        }

        /// <summary>
        /// Checks the state of the connection.
        /// </summary>
        /// <returns></returns>
        public bool CheckConnectionState()
        {
            bool bReturnVal = false;
            try
            {
                switch (_eProvider)
                {
                    case DbProvider.SQLServer:
                        if (_oSQLConnection.State == ConnectionState.Open)
                        {
                            bReturnVal = true;
                        }
                        break;
                    case DbProvider.OLEDB:
                        if (_oOleConnection.State == ConnectionState.Open)
                        {
                            bReturnVal = true;
                        }
                        break;
                }
            }
            catch (Exception)
            {
                // Logger.WriteLog(ex);
            }
            return bReturnVal;
        }

        /// <summary>
        /// Closes this instance.
        /// </summary>
        public void Close()
        {
            try
            {
                switch (_eProvider)
                {
                    case DbProvider.SQLServer:
                        _oSQLConnection.Close();
                        _oSQLConnection.Dispose();
                        break;
                    case DbProvider.OLEDB:
                        _oOleConnection.Close();
                        _oOleConnection.Dispose();
                        break;
                }
            }
            catch (Exception ex)
            {
                //  Logger.WriteLog(ex);
            }
        }

        #endregion

        /// <summary>
        /// Executes the non query.
        /// </summary>
        /// <param name="sqlQuery">The SQL query.</param>
        public void ExecuteNonQuery(string sqlQuery)
        {
            try
            {
                switch (_eProvider)
                {
                    case DbProvider.SQLServer:
                        var oSqlCommand1 = new SqlCommand(sqlQuery, _oSQLConnection)
                        {
                            CommandTimeout = CommandTimeoutInteger
                        };
                        oSqlCommand1.ExecuteNonQuery();
                        oSqlCommand1.Dispose();
                        break;
                    case DbProvider.OLEDB:
                        var oSqlCommand = new OleDbCommand(sqlQuery, _oOleConnection);
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

        /// <summary>
        /// Executes the dt.
        /// </summary>
        /// <param name="sqlQuery">The SQL query.</param>
        /// <returns></returns>
        public DataTable ExecuteDt(string sqlQuery)
        {
            return ExecuteDt(sqlQuery, null, null);
        }

        /// <summary>
        /// Executes the dt.
        /// </summary>
        /// <param name="sqlQuery">The SQL query.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public DataTable ExecuteDt(string sqlQuery, string[] parameters, string[] values)
        {
            var oSqlDataTable = new DataTable();
            //Try
            if (((parameters != null)) && ((values != null)) && (parameters.Length != values.Length))
            {
                //Parameter count does not equal value count
                oSqlDataTable = null;
            }
            else
            {
                switch (_eProvider)
                {
                    case DbProvider.SQLServer:
                        var oSqlCommand1 = new SqlCommand(sqlQuery, _oSQLConnection)
                        {
                            CommandType = CommandType.StoredProcedure,
                            CommandTimeout = CommandTimeoutInteger
                        };
                        if (((parameters != null)) && ((values != null)))
                        {
                            for (var iLoop = 0; iLoop <= parameters.Length - 1; iLoop++)
                            {
                                oSqlCommand1.Parameters.AddWithValue(parameters[iLoop], values[iLoop]);
                            }
                        }
                        dynamic oSqlDataAdapter = new SqlDataAdapter(oSqlCommand1);
                        oSqlDataAdapter.Fill(oSqlDataTable);
                        oSqlCommand1.Dispose();
                        break;
                    case DbProvider.OLEDB:
                        var oSqlCommand = new OleDbCommand(sqlQuery, _oOleConnection);
                        _oOleAdapter = new OleDbDataAdapter(oSqlCommand);
                        _oOleAdapter.Fill(oSqlDataTable);
                        oSqlCommand.Dispose();
                        break;
                }
            }
            //Catch ex As Exception
            //    oSqlDataTable = Nothing
            //End Try
            return oSqlDataTable;
        }

        /// <summary>
        /// Executes the specified SQL query.
        /// </summary>
        /// <param name="sqlQuery">The SQL query.</param>
        /// <returns></returns>
        public DataSet Execute(string sqlQuery)
        {
            return Execute(sqlQuery, null, null);
        }
        /// <summary>
        /// Executes the proc without parameter.
        /// </summary>
        /// <param name="sqlQuery">The SQL query.</param>
        /// <returns></returns>
        public DataSet ExecuteProcWithoutParam(string sqlQuery)
        {
            var oSqlDataSet = new DataSet();
            try
            {

                switch (_eProvider)
                {
                    case DbProvider.SQLServer:
                        var oSqlCommand1 = new SqlCommand(sqlQuery, _oSQLConnection)
                        {
                            CommandType = CommandType.StoredProcedure,
                            CommandTimeout = CommandTimeoutInteger
                        };

                        dynamic oSqlDataAdapter = new SqlDataAdapter(oSqlCommand1);
                        oSqlDataAdapter.Fill(oSqlDataSet);
                        oSqlCommand1.Dispose();
                        break;
                    case DbProvider.OLEDB:
                        var oSqlCommand = new OleDbCommand(sqlQuery, _oOleConnection);
                        _oOleAdapter = new OleDbDataAdapter(oSqlCommand);
                        _oOleAdapter.Fill(oSqlDataSet);
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

        /// <summary>
        /// Executes the specified SQL query.
        /// </summary>
        /// <param name="sqlQuery">The SQL query.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public DataSet Execute(string sqlQuery, string[] parameters, string[] values)
        {
            var oSqlDataSet = new DataSet();
            try
            {
                if (((parameters != null)) && ((values != null)) && (parameters.Length != values.Length))
                {
                    //Parameter count does not equal value count
                    oSqlDataSet = null;
                }
                else
                {
                    switch (_eProvider)
                    {
                        case DbProvider.SQLServer:
                            var oSqlCommand1 = new SqlCommand(sqlQuery, _oSQLConnection)
                            {
                                CommandType = CommandType.StoredProcedure,
                                CommandTimeout = CommandTimeoutInteger
                            };
                            if (((parameters != null)) && ((values != null)))
                            {
                                for (int iLoop = 0; iLoop <= parameters.Length - 1; iLoop++)
                                {
                                    oSqlCommand1.Parameters.AddWithValue(parameters[iLoop], values[iLoop]);
                                }
                            }
                            dynamic oSqlDataAdapter = new SqlDataAdapter(oSqlCommand1);
                            oSqlDataAdapter.Fill(oSqlDataSet);
                            oSqlCommand1.Dispose();
                            break;
                        case DbProvider.OLEDB:
                            var oSqlCommand = new OleDbCommand(sqlQuery, _oOleConnection);
                            _oOleAdapter = new OleDbDataAdapter(oSqlCommand);
                            _oOleAdapter.Fill(oSqlDataSet);
                            oSqlCommand.Dispose();
                            break;
                    }
                }
            }
            catch
            {
                //oSqlDataSet = null;
                throw;
                // Logger.WriteLog(ex);
            }
            return oSqlDataSet;
        }

        public DataSet ExecuteDtTable(string query, string[] parameters, object[] values)
        {
            var oSqlDataSet = new DataSet();
            //Try
            if (((parameters != null)) && ((values != null)) && (!(parameters.Length == values.Length)))
            {

                oSqlDataSet = null;
            }
            else
            {
                switch (_eProvider)
                {
                    case DbProvider.SQLServer:
                        var oSqlCommand1 = new SqlCommand(query, _oSQLConnection);
                        oSqlCommand1.CommandType = System.Data.CommandType.StoredProcedure;
                        oSqlCommand1.CommandTimeout = CommandTimeoutInteger;
                        if (((parameters != null)) && ((values != null)))
                        {
                            for (int iLoop = 0; iLoop <= parameters.Length - 1; iLoop++)
                            {
                                oSqlCommand1.Parameters.AddWithValue(parameters[iLoop], values[iLoop]);
                            }
                        }
                        dynamic oSqlDataAdapter = new SqlDataAdapter(oSqlCommand1);
                        oSqlDataAdapter.Fill(oSqlDataSet);
                        oSqlCommand1.Dispose();
                        break;
                    case DbProvider.OLEDB:
                        var oSqlCommand = new OleDbCommand(query, _oOleConnection);
                        _oOleAdapter = new OleDbDataAdapter(oSqlCommand);
                        _oOleAdapter.Fill(oSqlDataSet);
                        oSqlCommand.Dispose();
                        break;
                }
            }
            //Catch ex As Exception
            //    oSqlDataTable = Nothing
            //End Try
            return oSqlDataSet;
        }

        /// <summary>
        /// Begins the execute asynchronous.
        /// </summary>
        /// <param name="sqlQuery">The SQL query.</param>
        /// <param name="cb">The cb.</param>
        /// <param name="state">The state.</param>
        /// <returns></returns>
        public IAsyncResult BeginExecuteAsync(string sqlQuery, AsyncCallback cb, object state)
        {
            return BeginExecuteAsync(sqlQuery, null, null, cb, state);
        }

        public int ExecuteScalarValue(string sqlQuery)
        {
            _oSqlCommand = new SqlCommand(sqlQuery, _oSQLConnection)
            {
                CommandType = CommandType.StoredProcedure
            };
            string value = Convert.ToString(_oSqlCommand.ExecuteScalar());
            return string.IsNullOrEmpty(value) ? 0 : int.Parse(value);
        }
        /// <summary>
        /// Begins the execute asynchronous.
        /// </summary>
        /// <param name="sqlQuery">The SQL query.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="values">The values.</param>
        /// <param name="cb">The cb.</param>
        /// <param name="state">The state.</param>
        /// <returns></returns>
        public IAsyncResult BeginExecuteAsync(string sqlQuery, string[] parameters, string[] values, AsyncCallback cb, object state)
        {
            _oSqlCommand = new SqlCommand(sqlQuery, _oSQLConnection)
            {
                CommandType = CommandType.StoredProcedure
            };
            if (((parameters == null)) || ((values == null)) || (parameters.Length != values.Length))
                return _oSqlCommand.BeginExecuteReader(cb, state);

            for (var iLoop = 0; iLoop <= parameters.Length - 1; iLoop++)
            {
                _oSqlCommand.Parameters.AddWithValue(parameters[iLoop], values[iLoop]);
            }
            return _oSqlCommand.BeginExecuteReader(cb, state);
        }

        /// <summary>
        /// Ends the execute asynchronous.
        /// </summary>
        /// <param name="ar">The ar.</param>
        /// <returns></returns>
        public SqlDataReader EndExecuteAsync(IAsyncResult ar)
        {
            return _oSqlCommand.EndExecuteReader(ar);
        }

        /// <summary>
        /// 
        /// </summary>
        public enum DbProvider
        {
            /// <summary>
            /// The SQL server
            /// </summary>
            SQLServer,
            /// <summary>
            /// The oledb
            /// </summary>
            OLEDB
        }
        /// <summary>
        /// Gets or sets the database connection string.
        /// </summary>
        /// <value>
        /// The database connection string.
        /// </value>
        public static string DbConnectionString
        {
            get;
            set;
        }
    }


    /// <summary>
    /// Praveen
    /// Converts Datatable type to object type
    /// </summary>
    public static class DataTableExtensions
    {
        /// <summary>
        /// To the object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table">The table.</param>
        /// <returns></returns>
        public static T ToObject<T>(this DataTable table) where T : new()
        {
            IList<PropertyInfo> properties = typeof(T).GetProperties().ToList();
            return CreateItemFromRow<T>(table.Rows[0], properties);
        }

        /// <summary>
        /// To the list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table">The table.</param>
        /// <returns></returns>
        public static List<T> ToList<T>(this DataTable table) where T : new()
        {
            IList<PropertyInfo> properties = typeof(T).GetProperties().ToList();
            return (from object row in table.Rows select CreateItemFromRow<T>((DataRow)row, properties)).ToList();
        }

        /// <summary>
        /// Creates the item from row.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="row">The row.</param>
        /// <param name="properties">The properties.</param>
        /// <returns></returns>
        private static T CreateItemFromRow<T>(DataRow row, IEnumerable<PropertyInfo> properties) where T : new()
        {
            var item = new T();
            foreach (var property in properties.Where(property => property != null && row[property.Name] != DBNull.Value))
            {
                var t = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                object safeValue = (row[property.Name] == null) ? null : Convert.ChangeType(row[property.Name], t);

                property.SetValue(item, safeValue, null);
            }
            return item;
        }

        public static DataTable ToDataTable<T>(this List<T> items)
        {
            var dataTable = new DataTable(typeof(T).Name);
            var propertyInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var prop in propertyInfos)
            {
                dataTable.Columns.Add(prop.Name);
            }
            foreach (var item in items)
            {
                var values = new object[propertyInfos.Length];
                for (var i = 0; i < propertyInfos.Length; i++)
                {
                    values[i] = propertyInfos[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }

        public static DataTable ToDataTable<T>(this T item)
        {
            var dataTable = new DataTable(typeof(T).Name);

            var propertyInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var prop in propertyInfos)
            {
                dataTable.Columns.Add(prop.Name);
            }

            var values = new object[propertyInfos.Length];
            for (var i = 0; i < propertyInfos.Length; i++)
            {
                values[i] = propertyInfos[i].GetValue(item, null);
            }
            dataTable.Rows.Add(values);

            return dataTable;
        }
    }

}
