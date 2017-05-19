using SqlDataAdapter.Communications;
using System;
using System.Collections.Generic;
using System.Reflection;
using SqlDataAdapter.Attributes;
using SqlDataAdapter.Configurations;

namespace SqlDataAdapter.Attributes
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
                            case TypeCode.Decimal:
                                val = 0.0M;
                                break;
                            case TypeCode.Double:
                                val = 0.0d;
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
    public static class ColumnMapExtensions
    {
        public static List<T> PopulateAll<T>(this T clazz, string storedProcedureName, string adapterConfigurationFileName = "SqlDataAdapter.config") where T : ColumnMap
        {
            SQLAdapterConfiguration.SetConfig(adapterConfigurationFileName);
            Command cmd = new Command(storedProcedureName);
            List<T> result = DAO.ExecuteQuery(cmd.ToCommand(clazz)).ToColumnMapList(clazz);
            cmd.Dispose();
            return result;
        }

        public static T Populate<T>(this T clazz, string storedProcedureName, string adapterConfigurationFileName = "SqlDataAdapter.config") where T : ColumnMap
        {
            SQLAdapterConfiguration.SetConfig(adapterConfigurationFileName);
            Command cmd = new Command(storedProcedureName);
            T result = DAO.ExecuteQuery(cmd.ToCommand(clazz)).ToColumnMap(clazz);
            cmd.Dispose();
            return result;
        }

        public static int Insert<T>(this T clazz, string storedProcedureName, string adapterConfigurationFileName = "SqlDataAdapter.config") where T : ColumnMap
        {
            return NonQueryFromClass(clazz, storedProcedureName, adapterConfigurationFileName);
        }

        public static int Update<T>(this T clazz, string storedProcedureName, string adapterConfigurationFileName = "SqlDataAdapter.config") where T : ColumnMap
        {
            return NonQueryFromClass(clazz, storedProcedureName, adapterConfigurationFileName);
        }

        public static int Delete<T>(this T clazz, string storedProcedureName, string adapterConfigurationFileName = "SqlDataAdapter.config") where T : ColumnMap
        {
            return NonQueryFromClass(clazz, storedProcedureName, adapterConfigurationFileName);
        }

        private static int NonQueryFromClass<T>(T clazz, string storedProcedureName, string adapterConfigurationFileName) where T : ColumnMap
        {
            SQLAdapterConfiguration.SetConfig(adapterConfigurationFileName);
            Command cmd = new Command(storedProcedureName);
            return DAO.ExecuteNonQuery(cmd.ToCommand(clazz));
        }
    }
}
