using Dapper;
using System.Data;
using static Dapper.SqlMapper;
using System.Data.SqlClient;

namespace DapperQuery.Builder.Builders.Query
{
    public class Query 
    {
        // The SQL parameters needed in the array
        private DynamicParameters? mQueryParameters;
        // The SQL connection string
        private string? mSqlConnection;
        // The text of the command
        private string? mSQLCommand;
        // The command type of the sql query
        private CommandType? mQueryType;

        private int? mTimeout = null;

        private bool mUseTransaction = false;

        /* This is the constructor of the class. */
        public Query(string? SqlConnection = null, DynamicParameters? QueryParameters = null, string? StoredProcedureName = null, CommandType? QueryType = CommandType.Text)
        {
            this.mSqlConnection = SqlConnection;
            this.mQueryParameters = QueryParameters;
            this.mSQLCommand = StoredProcedureName;
            this.mQueryType = QueryType;
        }

        public void SetTimeout(int TimeOut)
        {
            this.mTimeout = TimeOut;
        }

        public int? GetTimeout()
        {
            return this.mTimeout;
        }

        public void SetTransaction(bool UseTransaction)
        {
            this.mUseTransaction = UseTransaction;
        }

        public bool GetTransaction()
        {
            return this.mUseTransaction;
        }

        public void SetCommandType(CommandType type)
        {
            this.mQueryType = type;
        }
        public CommandType? GetQueryType()
        {
            return this.mQueryType;
        }
        /// <summary>
        /// This function sets the connection string for the SQL Server database
        /// </summary>
        /// <param Name="connstring">The connection string to the database</param>
        public void SetSqlConnection(string connstring)
        {
            this.mSqlConnection = connstring;
        }
        /// <summary>
        /// This function returns the value of the mSqlConnection property
        /// </summary>
        /// <returns>
        /// The mSqlConnection property is being returned.
        /// </returns>
        public string? GetSqlConnection()
        {
            return this.mSqlConnection;
        }
        /// <summary>
        /// This function sets the Name of the stored procedure to be executed
        /// </summary>
        /// <param Name="storedprocedurename">The Name of the stored procedure to be executed.</param>
        public void SetName(string storedprocedurename)
        {
            this.mSQLCommand = storedprocedurename;
        }
        /// <summary>
        /// This function returns the Name of the stored procedure
        /// </summary>
        /// <returns>
        /// The Name of the stored procedure.
        /// </returns>
        public string? GetName()
        {
            return this.mSQLCommand;
        }
        /// <summary>
        /// This function adds a parameter to the mQueryParameters object
        /// </summary>
        /// <param Name="ParamName">The Name of the parameter</param>
        /// <param Name="T">The type of the parameter</param>
        public void AddParameter<T>(string ParamName, T Value, DbType? DataType = null, ParameterDirection? ParameterDirection = null)
        {
            /* This is a null-coalescing operator. It is used to check if the mQueryParameters object is
            null. If it is null, it will create a new DynamicParameters object and add the parameter
            to it. If it is not null, it will add the parameter to the existing object. */
            if (this.mQueryParameters is null)
            {
                this.mQueryParameters = new DynamicParameters();
                this.mQueryParameters.Add(ParamName, Value, direction:  ParameterDirection , dbType: DataType);
            }
            else
            {
                this.mQueryParameters.Add(ParamName, Value, direction: ParameterDirection, dbType: DataType);
            }

        }

        public void AddTableParameter(string ParamName, string DataTypeString, DataTable? Table, ParameterDirection? Direction = null)
        {
            if (this.mQueryParameters is null)
            {
                this.mQueryParameters = new DynamicParameters();
                this.mQueryParameters.Add(ParamName, Table.AsTableValuedParameter(DataTypeString), direction: Direction);
            }
            else
            {
                this.mQueryParameters.Add(ParamName, Table.AsTableValuedParameter(DataTypeString), direction: Direction);
            }
        }

        public void AddListParameter<T>(string ParamName, string DataTypeString, string ColumnName, T[] Values, ParameterDirection ParamDirection = ParameterDirection.Input)
        {
            // Create a Data Table for the list parameter
            DataTable Table = new DataTable();
            Table.Columns.Add(ColumnName);
            foreach (T Value in Values)
            {
                DataRow Row = Table.NewRow();
                Row[ColumnName] = Value;
                Table.Rows.Add(Row);
            }

            if (this.mQueryParameters is null)
            {
                this.mQueryParameters = new DynamicParameters();
                this.mQueryParameters.Add(ParamName, Table.AsTableValuedParameter(DataTypeString), direction: ParamDirection);
            }
            else
            {
                this.mQueryParameters.Add(ParamName, Table.AsTableValuedParameter(DataTypeString), direction: ParamDirection);
            }

        }
        /// <summary>
        /// This function returns the query parameters that are used in the query
        /// </summary>
        /// <returns>
        /// DynamicParameters?
        /// </returns>
        public DynamicParameters? GetParameters()
        {
            return this.mQueryParameters;
        }
        /// <summary>
        /// This function is used to check if the mSqlConnection string is defined
        /// </summary>
        public void SqlConnAuth()
        {
            if (this.mSqlConnection == null)
            {
                throw new InvalidOperationException("The SqlConnection string are'nt definied.");
            }
        }
        /// <summary>
        /// It returns a list of objects of type T obtained from the executed stored procedure
        /// </summary>
        /// <returns>
        /// A list of T
        /// </returns>
        public async Task<List<T>?> GetListResult<T>()
        {
            return await ExecAsyncQuery<T>();
        }
        /// <summary>
        /// > The function returns a list of objects that are the result of executing a stored procedure
        /// </summary>
        /// <returns>
        /// A list of objects.
        /// </returns>
        public async Task<List<object>?> GetListResult()
        {
            return await ExecAsyncQuery<object>();
        }

        public async Task<object?> GetRowResult()
        {
            return (await ExecAsyncQuery<object>())?[0];
        }
        public async Task<T?> GetRowResult<T>()
        {
            List<T>? Row = await ExecAsyncQuery<T>();
            if(Row is null)
            {
                return default(T);
            }

            return Row[0];
        }
        /// <summary>
        /// It takes a stored procedure Name and parameters, and returns a list of objects of type T
        /// </summary>
        /// <returns>
        /// A list of objects of type T.
        /// </returns>
        private async Task<List<T>?> ExecAsyncQuery<T>()
        {
            SqlConnAuth();

            
                List<T>? Result = null;
                using (var connection = new SqlConnection(this.mSqlConnection))
                {
                    IDbTransaction? QueryTransaction = null;
                    if (this.GetTransaction()) QueryTransaction = GetTransactionObject(connection);

                        Result = (List<T>?) await connection.QueryAsync<T>(
                            sql: this.GetName(),
                            param: this.GetParameters(),
                            commandType: this.mQueryType,
                            commandTimeout: this.GetTimeout(),
                            transaction: QueryTransaction
                            );

                    if (this.GetTransaction()) CommitTransaction(QueryTransaction);

                }
                return Result;
            
        }


        /// <summary>
        /// It reads the data from the database and stores it in a list, this returns all tables from a query in SQL
        /// </summary>
        /// <returns>
        /// A list of lists of type T.
        /// </returns>
        public async Task<List<List<T>>?> GetMultipleList<T>()
        {
            SqlConnAuth();

            try
            {
                List<List<T>> Tables = new List<List<T>>();

                using (var connection = new SqlConnection(this.mSqlConnection))
                {
                    IDbTransaction? QueryTransaction = null;
                    if (this.GetTransaction()) QueryTransaction = GetTransactionObject(connection);

                    GridReader? Reader = await connection.QueryMultipleAsync(sql: this.GetName(),
                                                                             param: this.GetParameters(),
                                                                             commandType: this.mQueryType,
                                                                             transaction: QueryTransaction);

                    /* Reading the data from the database and storing it in a list. */
                        do
                        {
                            IEnumerable<T>? Table = await Reader.ReadAsync<T>();
                            if (Table?.Count() > 0)
                                Tables.Add(Table.ToList());
                        } 
                    while (!Reader.IsConsumed);

                    if (this.GetTransaction()) CommitTransaction(QueryTransaction);
                }
                return Tables;
            }
            catch (Exception)
            {
                // Something wrong in the query
                return null;
            }
        }

        public void CommitTransaction(IDbTransaction? queryTransaction)
        {
            queryTransaction?.Commit();
        }

        public IDbTransaction? GetTransactionObject(SqlConnection connection)
        {
            if (mUseTransaction)
            {
                return connection.BeginTransaction();
            }

            return null;
        }

        /// <summary>
        /// > Executes a stored procedure without returning a result set
        /// </summary>
        /// <returns>
        /// The number of rows affected by the query.
        /// </returns>
        public async Task<int> ExecWithoutResult()
        {
            SqlConnAuth();
            int RowsAffected = 0;
            using (var connection = new SqlConnection(this.mSqlConnection))
            {

                IDbTransaction? QueryTransaction = null;
                if (this.GetTransaction()) QueryTransaction = GetTransactionObject(connection);

                RowsAffected = await connection.ExecuteAsync(sql: this.GetName(),
                                                             param: this.GetParameters(),
                                                             commandType: this.mQueryType,
                                                             transaction: QueryTransaction);

                if (this.GetTransaction()) CommitTransaction(QueryTransaction);
            }
            return RowsAffected;
        }

        public int Execute()
        {
            SqlConnAuth();
            int RowsAffected = 0;
            using (var connection = new SqlConnection(this.mSqlConnection))
            {


                IDbTransaction? QueryTransaction = null;
                if (this.GetTransaction()) QueryTransaction = GetTransactionObject(connection);

                RowsAffected = connection.Execute(
                    sql: this.GetName(),
                    param: this.GetParameters(),
                    commandType: this.mQueryType,
                    transaction: QueryTransaction);


                if (this.GetTransaction()) CommitTransaction(QueryTransaction);
            }
            return RowsAffected;
        }

        public async Task<int> ExecuteAsync()
        {
            SqlConnAuth();
            int RowsAffected = 0;
            using (var connection = new SqlConnection(this.mSqlConnection))
            {

                IDbTransaction? QueryTransaction = null;
                if (this.GetTransaction()) QueryTransaction = GetTransactionObject(connection);

                RowsAffected = await connection.ExecuteAsync(
                    sql: this.GetName(),
                    param: this.GetParameters(),
                    commandType: this.mQueryType);

                if (this.GetTransaction()) CommitTransaction(QueryTransaction);
            }
            return RowsAffected;
        }

        public async Task Run()
        {
            SqlConnAuth();
            using (var connection = new SqlConnection(this.mSqlConnection))
            {

                IDbTransaction? QueryTransaction = null;
                if (this.GetTransaction()) QueryTransaction = GetTransactionObject(connection);

                await connection.ExecuteAsync(
                    sql: this.GetName(),
                    param: this.GetParameters(),
                    commandType: this.mQueryType,
                    transaction: QueryTransaction);

                if (this.GetTransaction()) CommitTransaction(QueryTransaction);
            }
        }

        public T? GetParameterByName<T>(String Key)
        {
            if (this.mQueryParameters is null) return default(T);
            return this.mQueryParameters.Get<T>(Key);
        }
    }
}
