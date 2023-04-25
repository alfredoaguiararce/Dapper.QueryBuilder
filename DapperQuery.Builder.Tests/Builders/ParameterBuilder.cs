using Dapper.QueryBuilder.Builders.Parameter;
using DapperQuery.Builder.Builders.Query;

namespace DapperQuery.Builder.test.Builders
{
    [TestFixture]
    public class ParameterBuilderTest
    {
        /// <summary>
        /// This function tests the building of a query with a parameter using a mock query builder.
        /// </summary>
        [Test]
        public void BuildTests()
        {
            Parameter<int> builder = new ParamBuilder<int>()
                                      .Name("@Name")
                                      .Value(2)
                                      .Build();

            Query MockQuery = new QueryBuilder()
                              .WithSqlConnection("Sample string")
                              .WithParameter(builder)
                              .Build();


            Assert.That(MockQuery.GetParameters(), Is.Not.Null);
            Assert.That(MockQuery.GetParameters().ParameterNames.First, Is.EqualTo("Name"));
        }
    }
}
