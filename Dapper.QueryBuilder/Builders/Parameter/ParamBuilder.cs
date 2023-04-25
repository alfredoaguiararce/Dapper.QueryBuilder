using System.Data;

namespace Dapper.QueryBuilder.Builders.Parameter
{
    public class ParamBuilder<T>
    {
        private Parameter<T> _parameter;

        public ParamBuilder()
        {
            _parameter = new Parameter<T>();
        }

        public ParamBuilder<T> Name(string ParameterName)
        {
            _parameter.Name = ParameterName;
            return this;
        }
        public ParamBuilder<T> Value(T ParameterValue)
        {
            _parameter.Value = ParameterValue;
            return this;
        }
        public ParamBuilder<T> Direction(ParameterDirection Direction)
        {
            _parameter.Direction = Direction;
            return this;
        }

        public ParamBuilder<T> Type(DbType? dbType)
        {
            _parameter.Type = dbType;
            return this;
        }

        public Parameter<T> Build() => _parameter;
    }
}
