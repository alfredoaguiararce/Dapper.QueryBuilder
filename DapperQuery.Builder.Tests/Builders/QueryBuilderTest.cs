using DapperQuery.Builder.Builders.Query;

namespace DapperQuery.Builder.test.Builders
{
    [TestFixture]
    public class QueryBuilderTest
    {
        /// <summary>
        /// This function tests the QueryBuilder class by creating a new instance of the QueryBuilder
        /// class and then calling the Build method
        /// </summary>
        [Test]
        public void TestCreational()
        {
            Query MockQuery = new QueryBuilder().Build();
            Assert.That(MockQuery, Is.Not.Null);
        }

        /// <summary>
        /// This function tests the SetSqlConnectionString method of the Query class
        /// </summary>
        [Test]
        public void SetSqlConnectionStringTest()
        {
            Query MockQuery = new QueryBuilder()
                              .WithSqlConnection("Sample string")
                              .Build();
            Assert.That(MockQuery.GetSqlConnection(), Is.EqualTo("Sample string"));
        }

        /// <summary>
        /// This function tests the QueryBuilder class to ensure that the SQL command is set correctly
        /// </summary>
        [Test]
        public void SetCommanTest()
        {
            Query MockQuery = new QueryBuilder()
                .WithSQLCommand("TestName")
                .Build();

            Assert.That(MockQuery.GetName(), Is.EqualTo("TestName"));
        }

        /// <summary>
        /// This function tests that the QueryBuilder class can add parameters to the query
        /// </summary>
        [Test]
        public void AddParametersTest()
        {
            Query MockQuery = new QueryBuilder()
                            .WithParameter("@Param1", 1)
                            .Build();

            Assert.That(MockQuery.GetParameters(), Is.Not.Null);
            Assert.That(MockQuery.GetParameters().ParameterNames.First, Is.EqualTo("Param1"));
        }

        /// <summary>
        /// > This function tests that the QueryBuilder can add parameters to a query using a parameter
        /// builder
        /// </summary>
        [Test]
        public void AddParametersWithBuilderTest()
        {
            Query MockQuery = new QueryBuilder()
                            .WithParameterBuilder<int>(param =>
                                                            param.Name("@Param1")
                                                                  .Value(1))
                            .Build();

            Assert.That(MockQuery.GetParameters(), Is.Not.Null);
            Assert.That(MockQuery.GetParameters().ParameterNames.First, Is.EqualTo("Param1"));
        }

        /// <summary>
        /// This function tests that the Execute() method of the Query class throws an
        /// InvalidOperationException when the ConnectionString property is null
        /// </summary>
        [Test]
        public void NullConnectionStringExceptionTest()
        {
            Query MockQuery = new QueryBuilder().Build();

            Assert.Catch<InvalidOperationException>(() => MockQuery.Execute());
        }

        /// <summary>
        /// > Get the parameter by Name and return it as the specified type
        /// </summary>
        [Test]
        public void GetParamByNameTest()
        {
            Query MockQuery = new QueryBuilder()
                                .WithParameter("@Param1", 1)
                                .Build();

            Assert.That(MockQuery.GetParameterByName<int>("@Param1"), Is.EqualTo(1));
        }

        [Test]
        public void AddListParameters()
        {
            int[] a = { 1, 2, 4 };
            Query MockQuery = new QueryBuilder()
                                .AddListParameter("@Param1", "dbo.id_list", "Column1", a, System.Data.ParameterDirection.Input)
                                .Build();
            Assert.That(MockQuery.GetParameters(), Is.Not.Null);
        }
    }
}