using System;
using System.Reflection;

namespace SelfPopulatingTableFromAnnotation.Sql_Adapter {
    public class TableMap {

        public bool HasColumn<T>(T c, string ColumnName) where T : TableMap {
            foreach (FieldInfo f in typeof(T).GetFields()) {
                if (c.columnMatch(ColumnName, f.Name)) {
                    return true;
                }
            }
            return false;
        }

        public void SetValue<T>(T c, string columnName, object val) where T : TableMap {
            bool x = false;
            int y = 0;
            if (Boolean.TryParse((string)val, out x)) {
                val = x;
            } else if (Int32.TryParse((string)val, out y)) {
                val = y;
            }

            foreach (FieldInfo f in typeof(T).GetFields()) {
                if (c.columnMatch(columnName, f.Name)) {
                    if (f.Name.Equals("parameters")) {
                        f.SetValue(c, val.ToString());
                    } else if (f.GetType().ToString().Equals("String")) {
                        f.SetValue(c, val.ToString());
                    } else {
                        f.SetValue(c, val);
                    }

                    break;
                }
            }
        }
    }

    public static class TableExtensions {
        public static bool columnMatch<T>(this T c, string ColumnName, string fieldname) where T : TableMap {
            return c.GetNameAttribute(fieldname).Equals(ColumnName);
        }

        private static string GetNameAttribute<T>(this T c, string fieldName) where T : TableMap {
            var fieldInfo = typeof(T).GetField(fieldName);
            return ((ColumnMap)Attribute.GetCustomAttribute(fieldInfo, typeof(ColumnMap))).Name;
        }
    }
}