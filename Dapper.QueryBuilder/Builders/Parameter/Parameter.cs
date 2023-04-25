using System.Data;

namespace Dapper.QueryBuilder.Builders.Parameter
{
    public class Parameter<T>
    {

        public string Name { get; set; }
        public T Value { get; set; }
        public ParameterDirection? Direction { get; set; }
        public DbType? Type { get; set; }

        public Parameter(string name, T value, ParameterDirection? direction, DbType? type)
        {
            Name = name;
            Value = value;
            Direction = direction;
            Type = type;
        }

        public Parameter()
        {
        }
    }
}
