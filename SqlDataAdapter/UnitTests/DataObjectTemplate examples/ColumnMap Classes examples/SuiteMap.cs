using SqlDataAdapter.Attributes;
using System.Collections.Generic;
using UnitTests.DataObjectTemplate.Data_Object;

namespace UnitTests.DataObjectTemplate.ColumnMap_Classes
{
    public class SuiteMap : ColumnMap
    {
        [ColumnMap("SuiteId")]
        private int SuiteId;

        [ColumnMap("SuiteSuiteName")]
        public new string Name;

        [ColumnMap("SuiteSuiteDescription")]
        public string Description;

        [ColumnMap("SuiteProjectId")]
        public int ProjectId;
        List<SuiteToTestCaseIdMap> TestCaseIds { get; set; }

        public void Id(int Id) 
        {
            SuiteId = Id;
            TestCaseIds = new SuiteToTestCaseIdMap(Id).PopulateAll(StoredProcedures.GetTestCaseIdsBySuiteId);
        }
        public int Id()
        {
            return SuiteId;
        }

        public SuiteMap()
        {

        }

        public SuiteMap(int Id = 0, string Name = null, string Description = null, int ProjectId = 0)
        {
            this.Id(Id);
            this.Name = Name;
            this.Description = Description;
            this.ProjectId = ProjectId;
        }
    }
}
