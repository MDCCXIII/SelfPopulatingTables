using NUnit.Framework;
using SqlDataAdapter.Communications;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace UnitTests
{
    [TestFixture]
   public class TestCommand
    {
        [Test]
        public void TestNewCommandObject()
        {
            /// <summary>
            /// Test Command constructor.
            /// </summary>
            string expectedResult = "Test";
            Command cmd = new Command(expectedResult);
            string actualResult = cmd.command.CommandText;
            Assert.That(Equals(expectedResult, actualResult));
            Assert.That(Equals(cmd.command.CommandType, System.Data.CommandType.StoredProcedure));
        }

        /// <summary>
        /// Test command constructor trows Null Reference Exception
        /// </summary>
        [Test]
        public void TestNewCommandObjectWithNull()
        {
            string expectedResult = null;
            Assert.Throws<NullReferenceException>(() => new Command(expectedResult));
        }

        /// <summary>
        /// Test command constructor with empty string
        /// Test the command constructor trows Aggregate Exception. 
        /// </summary>
        [Test]
        public void TestNewCommandObjectWithEmptyString()
        {
            string expectedResult = "";
            Assert.Throws<AggregateException>(() => new Command(expectedResult));
        }

        /// <summary>
        /// Test adding parameters to the command object.
        /// </summary>
        [Test]
        public void AddParameterToCommandObject()
        {
            string testParameterName = "Test1";
            string testParameterValue = "Test2";

            Command cmd = new Command("Test");
            cmd.AddParameter(testParameterName, testParameterValue);
            Assert.AreEqual(cmd.command.Parameters[0].ParameterName, testParameterName);
            Assert.AreEqual(cmd.command.Parameters[0].Value, testParameterValue);
        }

        /// <summary>
        /// Test command object with empty and null parameters
        /// </summary>
        [Test]
        public void TestNullAndEmptyParameters()
        {
            string testValue = "Test";

            Command cmd = new Command(testValue);
            Assert.DoesNotThrow(() => cmd.AddParameter(null, null));
            Assert.DoesNotThrow(() => cmd.AddParameter(null, testValue));

            Assert.DoesNotThrow(() => cmd.AddParameter("", testValue));
            Assert.DoesNotThrow(() => cmd.AddParameter("", ""));

            Assert.DoesNotThrow(() => cmd.AddParameter(testValue, null));
            Assert.DoesNotThrow(() => cmd.AddParameter(testValue, ""));
        }

        /// <summary>
        /// Test adding dictionary Parameters to the command object 
        /// </summary>
        [Test]
        public void AddParametersToCommandObject()
        {
            Dictionary<string, object> testDictionary = new Dictionary<string, object>();
            testDictionary.Add("TestParametersName1", "TestParametersValue1");
            testDictionary.Add("TestParametersName2", "TestParametersValue2");
            Command cmd = new Command("Test");
            cmd.AddParameters(testDictionary);
            int index = 0;
            foreach (KeyValuePair<string, object> kvp in testDictionary)
            {
                Assert.AreEqual(cmd.command.Parameters[index].ParameterName, kvp.Key);
                Assert.AreEqual(cmd.command.Parameters[index].Value, kvp.Value);
                index++;
            }
        }

        /// <summary>
        /// Test adding Sql parameters to the command object
        /// </summary>
        [Test]
        public void AddSqlParameterToCommandObject()
        {
            SqlParameter testParameter = new SqlParameter();
            Command cmd = new Command("Test");
            testParameter.ParameterName = "Test";
            testParameter.Value = "TestValue";
            cmd.AddParameter(testParameter);
            Assert.AreEqual(cmd.command.Parameters[0].ParameterName, testParameter.ParameterName);
            Assert.AreEqual(cmd.command.Parameters[0].Value, testParameter.Value);

        }

        /// <summary>
        /// Test adding null value Sql parameters into the command object
        /// </summary>
        [Test]
        public void testNullSqlParameter()
        {
            SqlParameter testParameter = new SqlParameter();
            Command cmd = new Command("Test");
            Assert.Throws<ArgumentException>(() => cmd.AddParameter(testParameter));
        }

        /// <summary>
        /// Test adding Sql parameters to the command object
        /// </summary>
        [Test]
        public void AddSqlParametersToCommandObject()
        {
            Command cmd = new Command("Test");
            SqlParameter[] testParameters = new SqlParameter[2];
            testParameters[0] = new SqlParameter("Test1", null);
            testParameters[1] = new SqlParameter("Test2", "TestValue2");
            cmd.AddParameters(testParameters);
            Assert.AreEqual(cmd.command.Parameters[0], testParameters[0]);
            Assert.AreEqual(cmd.command.Parameters[1], testParameters[1]);
        }

        /// <summary>
        /// Test the build connection without parameters
        /// </summary>
        [Test]
        public void TestBuildConnectionWithoutParameter()
        {
            Command cmd = new Command("Test");
            string testConnectionString = "Data Source=10.137.242.57;Initial Catalog=Test;Integrated Security=True";
            cmd.BuildConnection(testConnectionString);
            Assert.AreEqual(cmd.command.Connection.ConnectionString, testConnectionString);
        }


    }
}
