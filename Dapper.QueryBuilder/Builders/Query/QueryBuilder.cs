using System.Data;
using Dapper.QueryBuilder.Builders.Parameter;
using Dapper.QueryBuilder.Utils;

namespace DapperQuery.Builder.Builders.Query
{
    public class QueryBuilder
    {
        /* The above code is declaring a private field named "mQuery" of type "Query" and initializing
        it with a new instance of the "Query" class. The "readonly" keyword indicates that the value
        of this field cannot be changed once it has been initialized. */
        private readonly Query mQuery = new Query();

        public QueryBuilder() => mQuery = new Query();

        /// <summary>
        /// This function adds a list parameter to a query builder object in C#.
        /// </summary>
        /// <param name="ParamName">The name of the parameter being added to the query.</param>
        /// <param name="DataTypeString">The data type of the parameter being added, represented as a
        /// string. Examples include "int", "varchar", "datetime", etc.</param>
        /// <param name="ColumnName">The name of the column in the database table that the parameter
        /// corresponds to.</param>
        /// <param name="Values">Values is an array of type T that contains the values to be added as a
        /// list parameter in the query. The values will be added to the query as a comma-separated list
        /// enclosed in parentheses.</param>
        /// <param name="ParameterDirection">ParameterDirection is an enumeration that specifies whether
        /// a parameter is input-only, output-only, bidirectional, or a return value from a stored
        /// procedure. It is an optional parameter with a default value of ParameterDirection.Input,
        /// which means the parameter is an input parameter.</param>
        /// <returns>
        /// The method is returning an instance of the QueryBuilder class.
        /// </returns>
        public QueryBuilder AddListParameter<T>(string ParamName, string DataTypeString, string ColumnName, T[] Values, ParameterDirection ParamDirection = ParameterDirection.Input)
        {
            this.mQuery.AddListParameter(ParamName, DataTypeString, ColumnName, Values, ParamDirection);
            return this;
        }

        /// <summary>
        /// This function adds a table parameter to a query builder object.
        /// </summary>
        /// <param name="ParamName">The name of the parameter being added to the query.</param>
        /// <param name="DataTypeString">The data type of the parameter being added to the query.
        /// Examples include "int", "varchar", "datetime", etc.</param>
        /// <param name="Table">Table is a parameter of type DataTable that represents the table data to
        /// be passed as a parameter to a SQL query. It can be used to pass a set of data as a parameter
        /// to a stored procedure or a SQL command. The data in the table can be accessed using the Rows
        /// and Columns properties of</param>
        /// <param name="Direction">Direction is an optional parameter of type ParameterDirection that
        /// specifies the direction of the parameter. It can have one of the following values: Input,
        /// Output, InputOutput, ReturnValue. If the direction is not specified, the default value is
        /// Input.</param>
        /// <returns>
        /// The method `WithTableParameter` returns an instance of the `QueryBuilder` class.
        /// </returns>
        public QueryBuilder WithTableParameter(string ParamName, string DataTypeString, DataTable? Table, ParameterDirection? Direction = null)
        {
            this.mQuery.AddTableParameter(ParamName, DataTypeString, Table, Direction);
            return this;
        }

        /// <summary>
        /// This function adds a parameter to a query builder object with optional data type and
        /// direction.
        /// </summary>
        /// <param name="ParamName">The name of the parameter being added to the query.</param>
        /// <param name="T">T is a generic type parameter that can be replaced with any valid data type
        /// at runtime. It represents the type of the parameter value being passed to the
        /// method.</param>
        /// <param name="DataType">DbType is an enumeration that represents the data type of a parameter
        /// in a database query. It is used to specify the data type of the parameter being added to the
        /// query. If the data type is not specified, it will be inferred from the type of the value
        /// being passed.</param>
        /// <param name="Direction">The ParameterDirection enum specifies whether the parameter is
        /// input-only, output-only, bidirectional, or a return value from a stored procedure. It can
        /// have one of the following values: Input, Output, InputOutput, ReturnValue, or None. If
        /// Direction is not specified, it defaults to Input.</param>
        /// <returns>
        /// The method returns an instance of the QueryBuilder class.
        /// </returns>
        public QueryBuilder WithParameter<T>(string ParamName, T Value, DbType? DataType = null, ParameterDirection? Direction = null)
        {
            this.mQuery.AddParameter(ParamName, Value, DataType , Direction);
            return this;
        }

        /// <summary>
        /// This function adds an output parameter with a specified name and value to a query builder
        /// object.
        /// </summary>
        /// <param name="ParamName">The name of the output parameter being added to the query.</param>
        /// <param name="T">T is a generic type parameter that represents the type of the output
        /// parameter value being added to the query. It can be any valid .NET type.</param>
        /// <returns>
        /// The method `WithOutputParameter` returns an instance of the `QueryBuilder` class.
        /// </returns>
        public QueryBuilder WithOutputParameter<T>(string ParamName, T Value)
        {

            this.mQuery.AddParameter(ParamName, Value, new TypeToDbType().GetType<T>() , ParameterDirection: ParameterDirection.Output);
            return this;
        }

        /// <summary>
        /// This function adds an optional parameter to a query builder object if a certain condition is
        /// met.
        /// </summary>
        /// <param name="condition">A boolean value that determines whether or not to add the parameter
        /// to the query. If it is true, the parameter will be added, otherwise it will not be
        /// added.</param>
        /// <param name="ParamName">The name of the parameter being added to the query. It is used to
        /// identify the parameter in the query and must match the parameter name used in the SQL
        /// statement.</param>
        /// <param name="T">T is a generic type parameter that can be replaced with any valid data type
        /// at runtime. It represents the type of the optional parameter value that can be passed to the
        /// method.</param>
        /// <param name="DataType">The data type of the parameter being added to the query. It is an
        /// optional parameter and if not specified, the default data type will be used. The data type
        /// is used to ensure that the value being passed to the parameter is of the correct type and to
        /// prevent any data type conversion errors. Examples</param>
        /// <param name="Direction">The direction of the parameter, which can be Input, Output,
        /// InputOutput, ReturnValue, or None. If not specified, it defaults to Input.</param>
        /// <returns>
        /// An instance of the QueryBuilder class is being returned.
        /// </returns>
        public QueryBuilder WithOptionalParameter<T>(bool condition, string ParamName, T Value, DbType? DataType = null, ParameterDirection? Direction = null)
        {
            if (condition)
            {
                this.mQuery.AddParameter(ParamName, Value, DataType, Direction);
            }
            return this;
        }

        /// <summary>
        /// This function adds an optional parameter to a query builder object.
        /// </summary>
        /// <param name="ParamName">The name of the parameter being added to the query.</param>
        /// <param name="T">T is a generic type parameter that can be replaced with any type at runtime.
        /// It represents the type of the optional parameter value that is being passed to the
        /// method.</param>
        /// <param name="DataType">DbType is an enumeration that represents the data type of a parameter
        /// in a database query. It is used to specify the type of data that will be stored in the
        /// parameter. Examples of DbType include String, Int32, DateTime, and Boolean. If the DataType
        /// parameter is not specified, the default value is</param>
        /// <param name="Direction">The ParameterDirection enum specifies the direction of the
        /// parameter, whether it is an input parameter, output parameter, input/output parameter, or a
        /// return value parameter. It can have one of the following values: Input, Output, InputOutput,
        /// ReturnValue. If the Direction parameter is not specified, the default value is</param>
        /// <returns>
        /// The method is returning an instance of the QueryBuilder class.
        /// </returns>
        public QueryBuilder WithOptionalParameter<T>(string ParamName, T Value, DbType? DataType = null, ParameterDirection? Direction = null)
        {
            /* This code block is checking if the `Value` parameter is not null. If it is not null, it
            adds a parameter to the `mQuery` object with the specified `ParamName`, `Value`,
            `DataType`, and `Direction`. This is used in the `WithOptionalParameter` method to add
            an optional parameter to the query builder object only if the `Value` is not null. */
            if (Value is not null)
            {
                this.mQuery.AddParameter(ParamName, Value, DataType, Direction);
            }
            return this;
        }

        /// <summary>
        /// This function sets the command type of a query builder object and returns the updated
        /// object.
        /// </summary>
        /// <param name="CommandType">CommandType is an enumeration that specifies how a command string
        /// should be interpreted by a database provider. It can have values such as Text,
        /// StoredProcedure, TableDirect, etc. The CommandType property is used to set the type of
        /// command that will be executed by the database provider.</param>
        /// <returns>
        /// The method `WithCommandType` returns an instance of the `QueryBuilder` class.
        /// </returns>
        public QueryBuilder WithCommandType(CommandType type)
        {
            this.mQuery.SetCommandType(type);
            return this;
        }

        /// <summary>
        /// The function sets the name of a stored procedure for a query builder object in C#.
        /// </summary>
        /// <param name="storedprocedurename">a string representing the name of a stored procedure in a
        /// database. This method sets the name of the stored procedure in the QueryBuilder object and
        /// returns the QueryBuilder object itself.</param>
        /// <returns>
        /// An instance of the QueryBuilder class is being returned.
        /// </returns>
        public QueryBuilder WithSQLCommand(string storedprocedurename)
        {
            this.mQuery.SetName(storedprocedurename);
            return this;
        }

        /// <summary>
        /// This function sets the name of a stored procedure in a query builder object and returns the
        /// object.
        /// </summary>
        /// <param name="storedprocedurename">a string parameter that represents the name of a stored
        /// procedure in a database. This method is used to set the name of the stored procedure in a
        /// QueryBuilder object.</param>
        /// <returns>
        /// The method `WithName` returns an instance of the `QueryBuilder` class.
        /// </returns>
        public QueryBuilder WithName(string storedprocedurename)
        {
            this.mQuery.SetName(storedprocedurename);
            return this;
        }

        /// <summary>
        /// This function sets the command type of a query builder object to stored procedure.
        /// </summary>
        /// <returns>
        /// An instance of the QueryBuilder class is being returned.
        /// </returns>
        public QueryBuilder WithCommanTypeAsStoredProcedure()
        {
            this.mQuery.SetCommandType(CommandType.StoredProcedure);
            return this;
        }

        /// <summary>
        /// This function sets the command type of a query builder object to text.
        /// </summary>
        /// <returns>
        /// An instance of the QueryBuilder class is being returned.
        /// </returns>
        public QueryBuilder WithCommanTypeAsFunction()
        {
            this.mQuery.SetCommandType(CommandType.Text);
            return this;
        }

        /// <summary>
        /// This function sets the command type of a query builder object to text.
        /// </summary>
        /// <returns>
        /// An instance of the QueryBuilder class is being returned.
        /// </returns>
        public QueryBuilder WithCommanTypeAsSimpleQuery()
        {
            this.mQuery.SetCommandType(CommandType.Text);
            return this;
        }

        /// <summary>
        /// The function sets the SQL connection string for a query builder object and returns the
        /// object.
        /// </summary>
        /// <param name="connstring">connstring is a string parameter that represents the connection
        /// string to a SQL Server database. It contains information such as the server name, database
        /// name, and authentication details needed to establish a connection to the database.</param>
        /// <returns>
        /// The method `WithSqlConnection` returns an instance of the `QueryBuilder` class.
        /// </returns>
        public QueryBuilder WithSqlConnection(string connstring)
        {
            this.mQuery.SetSqlConnection(connstring);
            return this;
        }

        /// <summary>
        /// This function sets the transaction flag to true for a query builder object.
        /// </summary>
        /// <returns>
        /// An instance of the QueryBuilder class is being returned.
        /// </returns>
        public QueryBuilder WithTransactionExecution()
        {
            this.mQuery.SetTransaction(true);
            return this;
        }

        /// <summary>
        /// This function sets a timeout for a query in a QueryBuilder object.
        /// </summary>
        /// <param name="Seconds">The number of seconds after which the query should timeout if it has
        /// not completed execution. This method sets the timeout value for the query being built using
        /// the QueryBuilder object.</param>
        /// <returns>
        /// An instance of the QueryBuilder class is being returned.
        /// </returns>
        public QueryBuilder WithTimeout(int Seconds)
        {
            this.mQuery.SetTimeout(Seconds);
            return this;
        }

        #region PARAMETER_BUILDER
        // TODO => Implement method with builders are under construction
        public QueryBuilder WithParameterBuilder<T>(Func<ParamBuilder<T>, ParamBuilder<T>> builderDirector)
        {
            Parameter<T> param = builderDirector(new ParamBuilder<T>()).Build();
            this.mQuery.AddParameter(param.Name, param.Value, param.Type, param.Direction);
            return this;
        }

        public QueryBuilder WithParameter<T>(Parameter<T> param)
        {
            this.mQuery.AddParameter(param.Name, param.Value, param.Type, param.Direction);
            return this;
        }
        #endregion

        /// <summary>
        /// This function returns the built query object.
        /// </summary>
        public Query Build() => mQuery;

    }
}
