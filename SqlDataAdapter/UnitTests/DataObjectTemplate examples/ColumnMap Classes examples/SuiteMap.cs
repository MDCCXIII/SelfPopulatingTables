using SqlDataAdapter.Attributes;
using System.Collections.Generic;
using UnitTests.DataObjectTemplate.Data_Object;

namespace UnitTests.DataObjectTemplate.ColumnMap_Classes
{
    public class SuiteMap : ColumnMap
    {
        private int f_SuiteId;
        [ColumnMap(columnName: "SuiteId")]
        public int SuiteId {
            get
            {
                return f_SuiteId;
            }
            internal set
            {
                f_SuiteId = value;
                TestCaseIds = new SuiteToTestCaseIdMap(f_SuiteId).PopulateAll(StoredProcedures.GetTestCaseIdsBySuiteId);
            }
        }

        [ColumnMap(columnName: "SuiteSuiteName")]
        public new string Name;

        [ColumnMap(columnName: "SuiteSuiteDescription")]
        public string Description;

        [ColumnMap(columnName: "SuiteProjectId")]
        public int ProjectId;
        List<SuiteToTestCaseIdMap> TestCaseIds { get; set; }

        public SuiteMap()
        {

        }

        public SuiteMap(int SuiteId = 0, string Name = null, string Description = null, int ProjectId = 0)
        {
            this.SuiteId = SuiteId;
            this.Name = Name;
            this.Description = Description;
            this.ProjectId = ProjectId;
        }
    }
}
