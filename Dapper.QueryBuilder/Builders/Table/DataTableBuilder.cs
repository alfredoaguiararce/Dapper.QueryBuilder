using System.Data;
using System.Reflection;

namespace DapperQuery.Builder.Builders.Table
{
    public class DataTableBuilder
    {
        // Create a new DataTable.
        private DataTable _data_table = new DataTable("ParentTable");

        public DataTableBuilder WithColumn(string ColumnName)
        {
            this._data_table.Columns.Add(ColumnName);
            return this;
        }

        public DataTableBuilder WithColumnsFromData<T>(List<T> Data)
        {
            PropertyInfo[] Props = GetPropertiesInfo<T>();
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                this._data_table.Columns.Add(prop.Name);
            }
            return this;  
        }

        public DataTableBuilder WithData<T>(List<T> Data)
        {
            PropertyInfo[] Props = GetPropertiesInfo<T>();

            foreach (T Row in Data)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(Row, null);
                }
                this._data_table.Rows.Add(values);

            }

            return this;
        }

        public DataTableBuilder FromList<T>(List<T> Data)
        {
            this.WithColumnsFromData<T>(Data);
            this.WithData<T>(Data);

            return this;
        }


        private static PropertyInfo[] GetPropertiesInfo<T>() => typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        public DataTable Build() => this._data_table;

    }
}
