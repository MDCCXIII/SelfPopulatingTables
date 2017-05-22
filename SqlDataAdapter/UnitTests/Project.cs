using SqlDataAdapter.Attributes;
using System;

namespace UnitTests
{
    public class Project : ColumnMap
    {
        [ColumnMap("ProjectID")]
        public int ProjectId;

        [ColumnMap("ProjectName")]
        public string ProjectName;

        [ColumnMap("ProjectUrl")]
        public string Url;

        public Project()
        {

        }

        public Project(int ProjectId = 0, string ProjectName = null, string Url = null)
        {
            this.ProjectId = ProjectId;
            this.ProjectName = ProjectName;
            this.Url = Url;
        }
    }
}