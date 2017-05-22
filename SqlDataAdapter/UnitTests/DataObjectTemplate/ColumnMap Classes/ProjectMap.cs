using SqlDataAdapter.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.DataObjectTemplate.ColumnMap_Classes
{
    public class ProjectMap : ColumnMap
    {
        [ColumnMap("ProjectID")]
        public int ProjectId;

        [ColumnMap("ProjectName")]
        public string ProjectName;

        [ColumnMap("ProjectUrl", "projectUrl")]
        public string ProjectUrl;

        public ProjectMap()
        {

        }

        public ProjectMap(int ProjectId = 0, string ProjectName = null, string ProjectUrl = null)
        {
            this.ProjectId = ProjectId;
            this.ProjectName = ProjectName;
            this.ProjectUrl = ProjectUrl;
        }
    }
}
