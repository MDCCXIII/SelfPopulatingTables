using NUnit.Framework;
using SqlDataAdapter.Attributes;
using System.Collections.Generic;
using UnitTests.DataObjectTemplate.ColumnMap_Classes;
using UnitTests.DataObjectTemplate.Data_Object;

namespace UnitTests
{
    [TestFixture]
    public class Class1
    {
        [Test]
        public void testing()
        {
            Project testInsertSuite = new Project(Url: "https://aarp-test.kanacloud.com/GTConnect/UnifiedAcceptor/AARPDesktop.Main",
                ProjectName: "Kana");
            testInsertSuite.Insert("insertProject", "SqlDataAdapter.Config");

            List<Project> suites = new Project(ProjectName: "Kana").PopulateAll("getProjectByName", "MyAdapter.Config");

            Project testDeleteProject = new Project(ProjectName: "Updated Project Name").Populate("getProjectByName");
            testDeleteProject.Delete("deleteProjectById", "SqlDataAdapter.Config");

            Project testUpdateProject = new Project(ProjectName: "Kana").Populate("getProjectByName");
            testUpdateProject.ProjectName = "Updated Project Name";
            testUpdateProject.Url = "Updated URL";
            testUpdateProject.Update("updateProjectByID");

            TestRunner testRunner = new TestRunner();
            testRunner.ProjectInformation = new ProjectMap();
            testRunner = new TestRunner();


        }
    }
}
