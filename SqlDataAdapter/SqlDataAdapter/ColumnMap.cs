using System;
using System.Reflection;

namespace SqlDataAdapter
{
    [AttributeUsage(AttributeTargets.Field)]
    public class ColumnMap : Attribute
    {
        private string ColumnName;
        private string ParameterMap;
        private object ColumnValue;

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public ColumnMap()
        {
        }

        public ColumnMap(string columnName)
        {
            ColumnName = columnName;
        }
        public ColumnMap(string columnName, string parameterMap)
        {
            ColumnName = columnName;
            ParameterMap = parameterMap;
        }

        public virtual string Name
        {
            get { return ColumnName; }
        }
        public virtual string parameter
        {
            get { return ParameterMap; }
        }

        public virtual object Value
        {
            get { return ColumnValue; }
            set { ColumnValue = value; }
        }

        public bool HasColumn<T>(T c, string ColumnName) where T : ColumnMap
        {
            bool result = false;
            foreach (FieldInfo f in typeof(T).GetFields())
            {
                if (columnMatch(c, ColumnName, f.Name))
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        public void SetValue<T>(T c, string columnName, object val) where T : ColumnMap
        {
            Type type = val.GetType();
            foreach (FieldInfo f in typeof(T).GetFields())
            {
                if (columnMatch(c, columnName, f.Name))
                {
                    if (type.Equals(typeof(DBNull)))
                    {
                        switch (Type.GetTypeCode(f.FieldType))
                        {
                            case TypeCode.String:
                                val = "";
                                break;
                            case TypeCode.Boolean:
                                val = false;
                                break;
                            default:
                                val = 0;
                                break;
                        }
                    }
                    f.SetValue(c, val);
                    break;
                }
            }
        }

        public bool columnMatch<T>(T c, string ColumnName, string fieldName)
        {
            FieldInfo fieldInfo = typeof(T).GetField(fieldName);
            string columnName = ((ColumnMap)GetCustomAttribute(fieldInfo, typeof(ColumnMap))).Name;
            return columnName.Equals(ColumnName);
        }
    }
}
