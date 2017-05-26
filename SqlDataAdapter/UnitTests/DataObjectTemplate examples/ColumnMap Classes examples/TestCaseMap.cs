using SqlDataAdapter.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.DataObjectTemplate.ColumnMap_Classes
{
    class TestCaseMap : ColumnMap
    {
        [ColumnMap(columnName: "TestCaseId")]
        public int Id;
        [ColumnMap(columnName: "TestCaseTestCaseName")]
        public new string Name;
        [ColumnMap(columnName: "TestCaseTestCaseDescription")]
        public string Description;
        [ColumnMap(columnName: "TestCaseTestTypeId")]
        public new int TypeId;
        [ColumnMap(columnName: "TestTypeTestType")]
        public string Type;
        [ColumnMap(columnName: "TestTypeTestTypeDescription")]
        public string TypeDescription;
        [ColumnMap(columnName: "TestCaseEnabled")]
        public bool Enabled;

        public TestCaseMap()
        {

        }

        public TestCaseMap(int Id = 0, string Name = null, string Description = null, int TypeId = 0, string Type = null,
            string TypeDescription = null, bool Enabled = false)
        {
            this.Id = Id;
            this.Name = Name;
            this.Description = Description;
            this.TypeId = TypeId;
            this.Type = Type;
            this.TypeDescription = TypeDescription;
            this.Enabled = Enabled;
        }
    }
}
