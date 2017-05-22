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
        [ColumnMap("TestCaseId")]
        public int Id;
        [ColumnMap("TestCaseTestCaseName")]
        public new string Name;
        [ColumnMap("TestCaseTestCaseDescription")]
        public string Description;
        [ColumnMap("TestCaseTestTypeId")]
        public new int TypeId;
        [ColumnMap("TestTypeTestType")]
        public string Type;
        [ColumnMap("TestTypeTestTypeDescription")]
        public string TypeDescription;
        [ColumnMap("TestCaseEnabled")]
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
