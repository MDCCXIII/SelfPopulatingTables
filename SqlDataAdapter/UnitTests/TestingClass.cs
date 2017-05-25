using SqlDataAdapter.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
        class TestingClass : ColumnMap
        {
            [ColumnMap("Test")]
            public string Test;

            public string test2;

            [ColumnMap("stringTest")]
            public string stringTest;

            [ColumnMap("intTest")]
            public int intTest;

            [ColumnMap("boolTest")]
            public bool boolTest;

            [ColumnMap("doubleTest")]
            public double doubleTest;

            [ColumnMap("floatTest")]
            public float floatTest;

            [ColumnMap("decimalTest")]
            public decimal decimalTest;
        }
    }

