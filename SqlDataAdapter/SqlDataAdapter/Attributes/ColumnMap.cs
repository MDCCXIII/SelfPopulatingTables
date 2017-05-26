using SqlDataAdapter.Communications;
using System;
using System.Collections.Generic;
using System.Reflection;
using SqlDataAdapter.Attributes;
using SqlDataAdapter.Configurations;

namespace SqlDataAdapter.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ColumnMap : Attribute
    {
        private string ColumnName;
        private string ParameterMap;
        private object ColumnValue;



        /// <summary>
        /// Empty ColumnMap constructor
        /// </summary>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public ColumnMap()
        {

        }

        /// <summary>
        /// ColumnMap constructor passes columnName and parameterMap as string variables
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="parameterMap"></param>
        public ColumnMap(string columnName = "", string parameterMap = "")
        {
            ColumnName = columnName;
            ParameterMap = parameterMap;
        }

        /// <summary>
        /// ColumnName property
        /// Gets a value that specifies the name of the database column
        /// </summary>
        public virtual string Name
        {
            get { return ColumnName; }
        }

        /// <summary>
        /// Parameter property
        /// Gets a value that specifies the Parameter
        /// </summary>
        public virtual string parameter
        {
            get { return ParameterMap; }
        }

        /// <summary>
        /// Value property
        /// Gets and sets a value that specifies the value of the database object
        /// </summary>
        public virtual object Value
        {
            get { return ColumnValue; }
            set { ColumnValue = value; }
        }

        /// <summary>
        /// Implements private method ColumnMatch  
        /// returns true or false 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="c"></param>
        /// <param name="ColumnName"></param>
        /// <returns></returns>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public bool HasColumn<T>(T c, string ColumnName) where T : ColumnMap
        {
            bool result = false;
            if (ColumnName != null)
            {
                result = HasField(c, ColumnName, result);
                result = HasProperty(c, ColumnName, result);
            }
            return result;
        }

        private bool HasProperty<T>(T c, string ColumnName, bool result) where T : ColumnMap
        {
            foreach (PropertyInfo p in typeof(T).GetProperties())
            {
                if (PropertyMatch(c, ColumnName, p.Name))
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        private bool HasField<T>(T c, string ColumnName, bool result) where T : ColumnMap
        {
            foreach (FieldInfo f in typeof(T).GetFields())
            {
                if (FieldMatch(c, ColumnName, f.Name))
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Sets value of a field by its attributed column name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="c"></param>
        /// <param name="columnName"></param>
        /// <param name="val"></param>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public void SetValue<T>(T c, string columnName, object val) where T : ColumnMap
        {
            Type type = val.GetType();
            SetField(c, columnName, val, type);
            SetProperty(c, columnName, val, type);
        }

        private void SetProperty<T>(T c, string columnName, object val, Type type) where T : ColumnMap
        {
            foreach (PropertyInfo p in typeof(T).GetProperties())
            {
                if (PropertyMatch(c, columnName, p.Name))
                {
                    val = DefaultDBNullValues(val, type, p.PropertyType);
                    p.SetValue(c, val);
                    break;
                }
            }
        }

        private void SetField<T>(T c, string columnName, object val, Type type) where T : ColumnMap
        {
            foreach (FieldInfo f in typeof(T).GetFields())
            {
                if (FieldMatch(c, columnName, f.Name))
                {
                    val = DefaultDBNullValues(val, type, f.FieldType);
                    f.SetValue(c, val);
                    break;
                }
            }
        }

        private static object DefaultDBNullValues(object val, Type type, Type variableType)
        {
            if (type.Equals(typeof(DBNull)))
            {
                switch (Type.GetTypeCode(variableType))
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

            return val;
        }

        /// <summary>
        /// checks the column name passed to the method matches the field name passed to the method
        /// returns the column name set to lower
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="c"></param>
        /// <param name="ColumnName"></param>
        /// <param name="fieldName"></param>
        private bool FieldMatch<T>(T c, string ColumnName, string fieldName)
        {
            bool result = false;
            FieldInfo fieldInfo = typeof(T).GetField(fieldName);
            ColumnMap attribute = ((ColumnMap)GetCustomAttribute(fieldInfo, typeof(ColumnMap)));
            if (attribute != null)
            {
                result = attribute.Name.Equals(ColumnName);
            }
            return result;
        }

        private bool PropertyMatch<T>(T c, string ColumnName, string propertyName)
        {
            bool result = false;
            PropertyInfo propertyInfo = typeof(T).GetProperty(propertyName);
            ColumnMap attribute = ((ColumnMap)GetCustomAttribute(propertyInfo, typeof(ColumnMap)));
            if (attribute != null)
            {
                result = attribute.Name.Equals(ColumnName);
            }
            return result;
        }
    }

    public static class ColumnMapExtensions
    {
        /// <summary>
        ///  Populates a list of class T using the results of the executed stored procedure
        /// <para>
        /// Optional: Sets the custom configuration name <seealso cref="SQLAdapterConfiguration.SetConfig(string, string)"/>
        /// </para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="clazz"></param>
        /// <param name="storedProcedureName"></param>
        /// <param name="adapterConfigurationFileName"></param>
        /// <returns></returns>
        public static List<T> PopulateAll<T>(this T clazz, string storedProcedureName, string adapterConfigurationFileName = "SqlDataAdapter.config", string adapterConfigurationFilePath = "") where T : ColumnMap
        {
            SQLAdapterConfiguration.SetConfig(adapterConfigurationFileName, adapterConfigurationFilePath);
            Command cmd = new Command(storedProcedureName);
            List<T> result = DAO.ExecuteQuery(cmd.ToCommand(clazz)).ToColumnMapList(clazz);
            cmd.Dispose();
            return result;
        }

        /// <summary>
        /// Populates a single instance of class T using the first record of the results of the executed stored procedure 
        /// <para>
        /// Optional: Sets the custom configuration name <seealso cref="SQLAdapterConfiguration.SetConfig(string, string)"/>
        /// </para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="clazz"></param>
        /// <param name="storedProcedureName"></param>
        /// <param name="adapterConfigurationFileName"></param>
        /// <returns></returns>
        public static T Populate<T>(this T clazz, string storedProcedureName, string adapterConfigurationFileName = "SqlDataAdapter.config", string adapterConfigurationFilePath = "") where T : ColumnMap
        {
            SQLAdapterConfiguration.SetConfig(adapterConfigurationFileName, adapterConfigurationFilePath);
            Command cmd = new Command(storedProcedureName);
            T result = DAO.ExecuteQuery(cmd.ToCommand(clazz)).ToColumnMap(clazz);
            cmd.Dispose();
            return result;
        }

        /// <summary>
        /// Executes a nonquery stored procedure and returns the number of rows affected in the database
        /// <para>
        /// Optional: Sets the custom configuration name <seealso cref="SQLAdapterConfiguration.SetConfig(string, string)"/>
        /// </para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="clazz"></param>
        /// <param name="storedProcedureName"></param>
        /// <param name="adapterConfigurationFileName"></param>
        /// <returns></returns>
        public static int Push<T>(this T clazz, string storedProcedureName, string adapterConfigurationFileName = "SqlDataAdapter.config", string adapterConfigurationFilePath = "") where T : ColumnMap
        {
            SQLAdapterConfiguration.SetConfig(adapterConfigurationFileName, adapterConfigurationFilePath);
            Command cmd = new Command(storedProcedureName);
            return DAO.ExecuteNonQuery(cmd.ToCommand(clazz));
        }
    }
}
