using NUnit.Framework;
using SqlDataAdapter.Attributes;
using System;


namespace UnitTests
{
    [TestFixture]
    class TestColumnMap
    {
        /// <summary>
        /// Test has columns method
        /// </summary>
        [Test]
        public void TestHasColumn()
        {
            TestingClass tc = new TestingClass();

            Assert.True(new ColumnMap().HasColumn(tc, "Test"));
            Assert.False(new ColumnMap().HasColumn(tc, "tt"));
            Assert.False(new ColumnMap().HasColumn(tc, ""));
            Assert.False(new ColumnMap().HasColumn(tc, null));
        }

        /// <summary>
        /// Tests set value method with ints
        /// </summary>
        [Test]
        public void TestSetValueWithInt()
        {
            TestingClass tc = new TestingClass();

            new ColumnMap().SetValue(tc, "intTest", 21);
            Assert.AreEqual(tc.intTest, 21);

            new ColumnMap().SetValue(tc, "intTest", DBNull.Value);
            Assert.AreEqual(tc.intTest, 0);
        }

        /// <summary>
        /// Tests set value method with ints
        /// </summary>
        [Test]
        public void TestSetValueWithString()
        {
            TestingClass tc = new TestingClass();

            new ColumnMap().SetValue(tc, "stringTest", "test_String");
            Assert.AreEqual(tc.stringTest, "test_String");

            new ColumnMap().SetValue(tc, "stringTest", DBNull.Value);
            Assert.AreEqual(tc.stringTest, "");
        }

        /// <summary>
        /// Tests set value method with bools
        /// </summary>
        [Test]
        public void TestSetValueWithBool()
        {
            TestingClass tc = new TestingClass();

            new ColumnMap().SetValue(tc, "boolTest", true);
            Assert.AreEqual(tc.boolTest, true);

            new ColumnMap().SetValue(tc, "boolTest", DBNull.Value);
            Assert.AreNotEqual(tc.boolTest, true);
        }

        /// <summary>
        /// Tests set value method with decimals
        /// </summary>
        [Test]
        public void TestSetValueWithDecimal()
        {
            TestingClass tc = new TestingClass();

            new ColumnMap().SetValue(tc, "decimalTest", DBNull.Value);
            Assert.AreEqual(tc.decimalTest, 0.0M);

            new ColumnMap().SetValue(tc, "decimalTest", DBNull.Value);
            Assert.AreNotEqual(tc.decimalTest, "");
        }

        /// <summary>
        /// Tests set value method with doubles
        /// </summary>
        [Test]
        public void TestSetValueWithDouble()
        {
            TestingClass tc = new TestingClass();

            new ColumnMap().SetValue(tc, "doubleTest", DBNull.Value);
            Assert.AreEqual(tc.doubleTest, 0.0d);

            new ColumnMap().SetValue(tc, "doubleTest", DBNull.Value);
            Assert.AreNotEqual(tc.doubleTest, "");
        }

        /// <summary>
        /// Tests set value method with floats
        /// </summary>
        [Test]
        public void TestSetValueWithFloat()
        {
            TestingClass tc = new TestingClass();

            new ColumnMap().SetValue(tc, "floatTest", DBNull.Value);
            Assert.AreEqual(tc.floatTest, 0.0f);

            new ColumnMap().SetValue(tc, "floatTest", DBNull.Value);
            Assert.AreNotEqual(tc.floatTest, "");
        }
    }
}

