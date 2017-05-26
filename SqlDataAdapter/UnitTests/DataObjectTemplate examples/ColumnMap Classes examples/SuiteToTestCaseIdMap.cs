using SqlDataAdapter.Attributes;

namespace UnitTests.DataObjectTemplate.ColumnMap_Classes
{
    internal class SuiteToTestCaseIdMap : ColumnMap
    {
        [ColumnMap(columnName: "SuiteToTestCaseSuiteId")]
        public int SuiteId;

        [ColumnMap(columnName: "SuiteToTestCaseTestCaseId")]
        public int Id;

        public SuiteToTestCaseIdMap()
        {

        }

        public SuiteToTestCaseIdMap(int SuiteId = 0, int Id = 0)
        {
            this.SuiteId = SuiteId;
            this.Id = Id;
        }
    }
}