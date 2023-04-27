# Dapper.QueryBuilder

![https://img.shields.io/badge/license-MIT-blue.svg](https://img.shields.io/badge/license-MIT-blue.svg)

## **Description**

This project is a .NET library that provides a set of builders to simplify the creation of Dapper queries for SQL Server databases. The library includes builders for building SELECT, INSERT, UPDATE and DELETE queries, as well as builders for building table structures from lists of objects. The library is designed to make it easier to write efficient and maintainable data access code for SQL Server databases using Dapper.

## **Installation**


```bash
dotnet add package Dapper.Query.Builders --version 1.0.0
```

## **Usage**

Here is an example of how to use the library to retrieve a list of process areas from a SQL Server database using Dapper:

```csharp
public override async Task<List<TResult>?> GetProcessAreas<TResult>(string nSite, string? nPanum, int? nLanguage)
{
    string StoredProcedureName = "GSProcessAreaGetList";
    Query Procedure = new QueryBuilder()
                      .WithCommanTypeAsStoredProcedure()
                      .WithSQLCommand(StoredProcedureName)
                      .WithSqlConnection(mConnectionString)
                      .WithParameter("@SITE"                , nSite)
                      .WithOptionalParameter("@PANUM"       , nPanum)
                      .WithOptionalParameter("@LANGUAGEPK"  , nLanguage)
                      .Build();

    return await Procedure.GetListResult<TResult>();
}

```

## QueryBuilder methods

| Method | Description |
| --- | --- |
| AddListParameter<T>(string ParamName, string DataTypeString, string ColumnName, T[] Values, ParameterDirection ParamDirection = ParameterDirection.Input) | Adds a list parameter to the query with the given name, data type string, column name, values, and parameter direction. |
| WithTableParameter(string ParamName, string DataTypeString, DataTable? Table, ParameterDirection? Direction = null) | Adds a table parameter to the query with the given name, data type string, table, and parameter direction. |
| WithParameter<T>(string ParamName, T Value, DbType? DataType = null, ParameterDirection? Direction = null) | Adds a parameter to the query with the given name, value, data type, and parameter direction. |
| WithOutputParameter<T>(string ParamName, T Value) | Adds an output parameter to the query with the given name and value. |
| WithOptionalParameter<T>(bool condition, string ParamName, T Value, DbType? DataType = null, ParameterDirection? Direction = null) | Adds an optional parameter to the query with the given name, value, data type, and parameter direction if the specified condition is true. |
| WithOptionalParameter<T>(string ParamName, T Value, DbType? DataType = null, ParameterDirection? Direction = null) | Adds an optional parameter to the query with the given name, value, data type, and parameter direction if the value is not null. |
| WithCommandType(CommandType type) | Sets the command type of the query to the given type. |
| WithSQLCommand(string storedprocedurename) | Sets the name of the stored procedure to be executed by the query. |
| WithName(string storedprocedurename) | Sets the name of the stored procedure to be executed by the query. |
| WithCommanTypeAsStoredProcedure() | Sets the command type of the query to stored procedure. |
| WithCommanTypeAsFunction() | Sets the command type of the query to function. |
| WithCommanTypeAsSimpleQuery() | Sets the command type of the query to simple query. |
| WithSqlConnection(string connstring) | Sets the connection string of the query. |
| WithTransactionExecution() | Sets the query to execute within a transaction. |
| WithTimeout(int Seconds) | Sets the timeout of the query to the given number of seconds. |
| WithParameterBuilder<T>(Func<ParamBuilder<T>, ParamBuilder<T>> builderDirector) | Under construction. |
| WithParameter<T>(Parameter<T> param) | Under construction. |
| Build() | Builds and returns the query object. |

## Basic usage

**Functions**

```csharp
string functionName = "SELECT * FROM [dbo].[GetUsersByRole](@Role)";
Query getUsersByRoleQuery = new QueryBuilder()
    .WithSqlConnection(mConnectionString)
    .WithCommanTypeAsFunction()
    .WithSQLCommand(functionName)
    .WithParameter("@Role", "Admin")
    .Build();

List<User> users = await getUsersByRoleQuery.GetListResult<User>();
```

**Querys**

```csharp
string SQL_Command = "SELECT * FROM Customers WHERE Country = @Country";
Query GetCustomersQuery = new QueryBuilder()
                        .WithSqlConnection(connectionString)
                        .WithCommanTypeAsSimpleQuery()
                        .WithSQLCommand(SQL_Command)
                        .WithParameter("@Country", "Mexico")
                        .Build();

return await GetCustomersQuery.GetListResult<Customer>();
```

**Stored procedures**

```csharp
// Note you can access the stored procedure by using the schema name like dbo, dbo is the default schema
string SP_Name = "[schema].sp_GetCustomerByID";
Query GetCustomerByIDSP = new QueryBuilder()
                        .WithSqlConnection(connectionString)
                        .WithCommandTypeAsStoredProcedure()
                        .WithSQLCommand(SP_Name)
                        .WithParameter("@CustomerID", 1)
                        .Build();

return await GetCustomerByIDSP.GetListResult<Customer>();
```

**IMPORTANT**

Let's say we want to retrieve a list of customers from our database. We can create the query like this:

```csharp

string sqlCommand = "SELECT * FROM Customers";
Query query = new QueryBuilder()
.WithSqlConnection(connectionString)
.WithCommanTypeAsSimpleQuery()
.WithSQLCommand(sqlCommand)
.Build();
```

Then, to retrieve the results, we can use the GetListResult() method like this:

```csharp
List<Customer>? customers = await query.GetListResult<Customer>();
```

Note that we use the question mark after the list type List<Customer>? to indicate that the list may be null. This happens when the query does not find any results in the database.

It's also important to note that if we don't specify a result type with the GetListResult() method, the results will be returned as a list of generic objects (List<object>). For example:

```csharp
List<object>? result = await query.GetListResult();
```

In this case, each row of the table returned by the query will be represented by a generic object, which can make it difficult to handle the data if we need to work with specific properties of each row.

## **DataTableBuilder** methods

| Method | Description |
| --- | --- |
| WithColumn(string ColumnName) | Adds a new column to the DataTable with the specified column name. |
| WithColumnsFromData<T>(List<T> Data) | Adds columns to the DataTable based on the properties of the objects in the provided list Data. The column names are set to the names of the properties. |
| WithData<T>(List<T> Data) | Adds data to the DataTable based on the objects in the provided list Data. Each object in the list corresponds to a row in the DataTable, and each property of the object corresponds to a column. |
| FromList<T>(List<T> Data) | Calls WithColumnsFromData<T> and WithData<T> in sequence to populate the DataTable with the provided list Data. |
| Build() | Returns the completed DataTable object. |

## **Basic Usage**

```csharp

var list = new List<User>
					{
					  new User { Id = 1, Name = "Alice", Age = 30 },
					  new User { Id = 2, Name = "Bob", Age = 40 },
					  new User { Id = 3, Name = "Charlie", Age = 50 }
					};

var dataTable = new DataTableBuilder()
                  .WithColumnsFromData(list)
                  .WithData(list)
                  .Build();

// Display the contents of the DataTable
foreach (DataRow row in dataTable.Rows)
{
  Console.WriteLine($"Id: {row["Id"]}, Name: {row["Name"]}, Age: {row["Age"]}");
}

class User
{
	public int Id { get; set; }
	public string Name { get; set; }
	public int Age { get; set; }
}

```

## **License**

This library is released under the MIT License. See **[LICENSE.md](https://chat.openai.com/LICENSE.md)** for more information.

## **Contributing**

Contributions to this project are welcome! If you would like to contribute code, please create a pull request on GitHub. If you would like to report a bug or suggest a feature, please open an issue on GitHub.

## **Version History**

- 1.0.0 (2023-04-26):  First release.
