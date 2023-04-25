using DapperQuery.Builder.Builders.Table;
using System.Data;

namespace DapperQuery.Builder.test.Builders
{
    [TestFixture]
    public class DataTableBuilderTest
    {
        [Test]
        public void TestCreational()
        {
            DataTable? MockTable = null;
            MockTable = new DataTableBuilder()
                        .Build();
            Assert.That(MockTable, Is.Not.Null);
        }

        /// <summary>
        /// This function tests the creation of a DataTable from a list of objects.
        /// </summary>
        [Test]
        public void CreateDataTableFromListObjectTests()
        {

            DataTable? MockTable = null;
            //Create Data
            List<TestObject> MockList = new List<TestObject>();
            MockList.Add(new TestObject() { Prop1 = 1, Prop2 = true });
            MockList.Add(new TestObject() { Prop1 = 2, Prop2 = false });
            MockList.Add(new TestObject() { Prop1 = 3, Prop2 = true, Prop3 = 5 });

            MockTable = new DataTableBuilder()
                            .FromList(MockList)
                            .Build();

            Assert.That(MockTable, Is.Not.Null);
            Assert.That(MockTable.Columns[0].ColumnName, Is.EqualTo("Prop1"));
            Assert.That(MockTable.Columns[1].ColumnName, Is.EqualTo("Prop2"));
            Assert.That(MockTable.Columns[2].ColumnName, Is.EqualTo("Prop3"));

            Assert.That(MockTable.Rows.Count, Is.EqualTo(3));
        }

        /// <summary>
        /// This function tests the creation of a DataTable from a list of objects.
        /// </summary>
        [Test]
        public void GetDataFromListObjectTests()
        {

            DataTable? MockTable = null;
            //Create Data
            List<TestObject> MockList = new List<TestObject>();
            MockList.Add(new TestObject() { Prop1 = 1, Prop2 = true });
            MockList.Add(new TestObject() { Prop1 = 2, Prop2 = false });
            MockList.Add(new TestObject() { Prop1 = 3, Prop2 = true, Prop3 = 5 });

            MockTable = new DataTableBuilder()
                            .WithColumn("Column_1")
                            .WithColumn("Column_2")
                            .WithColumn("Column_3")
                            .WithData(MockList)
                            .Build();

            Assert.That(MockTable, Is.Not.Null);
            Assert.That(MockTable.Columns[0].ColumnName, Is.EqualTo("Column_1"));
            Assert.That(MockTable.Columns[1].ColumnName, Is.EqualTo("Column_2"));
            Assert.That(MockTable.Columns[2].ColumnName, Is.EqualTo("Column_3"));

            Assert.That(MockTable.Rows.Count, Is.EqualTo(3));
        }

        /// <summary>
        /// This function tests the creation of a DataTable object with columns derived from a list of
        /// objects.
        /// </summary>
        [Test]
        public void GetColumnsFromListObjectTest()
        {

            DataTable? MockTable = null;
            //Create Data
            List<TestObject> MockList = new List<TestObject>();
            MockList.Add(new TestObject() { Prop1 = 1, Prop2 = true });
            MockList.Add(new TestObject() { Prop1 = 2, Prop2 = false });
            MockList.Add(new TestObject() { Prop1 = 3, Prop2 = true, Prop3 = 5 });

            MockTable = new DataTableBuilder()
                            .WithColumnsFromData(MockList)
                            .Build();

            Assert.That(MockTable, Is.Not.Null);
            Assert.That(MockTable.Columns[0].ColumnName, Is.EqualTo("Prop1"));
            Assert.That(MockTable.Columns[1].ColumnName, Is.EqualTo("Prop2"));
            Assert.That(MockTable.Columns[2].ColumnName, Is.EqualTo("Prop3"));

            Assert.That(MockTable.Rows.Count, Is.EqualTo(0));
        }
    }

    public class TestObject
    {
        public int Prop1 { get; set; }
        public bool Prop2 { get; set; }
        public int? Prop3 { get; set; }
    }
}
