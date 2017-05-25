﻿using SqlDataAdapter.Communications;
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

        /// <summary>
        /// Empty ColumnMap constructor
        /// </summary>
        public ColumnMap()
        {

        }

        /// <summary>
        /// ColumnMap constructor passes columnName as a string variable
        /// </summary>
        /// <param name="columnName"></param>
        public ColumnMap(string columnName)
        {
            ColumnName = columnName;
        }

        /// <summary>
        /// ColumnMap constructor passes columnName and parameterMap as string variables
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="parameterMap"></param>
        public ColumnMap(string columnName, string parameterMap)
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

        /// <summary>
        /// Sets value of a field by its attributed column name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="c"></param>
        /// <param name="columnName"></param>
        /// <param name="val"></param>
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

        /// <summary>
        /// checks the column name passed to the method matches the field name passed to the method
        /// returns the column name set to lower
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="c"></param>
        /// <param name="ColumnName"></param>
        /// <param name="fieldName"></param>
        public bool columnMatch<T>(T c, string ColumnName, string fieldName)
        {
            FieldInfo fieldInfo = typeof(T).GetField(fieldName);
            string columnName = ((ColumnMap)GetCustomAttribute(fieldInfo, typeof(ColumnMap))).Name;
            return columnName.Equals(ColumnName);
        }
    }

    public static class ColumnMapExtensions
    {
        /// <summary>
        /// Populates a list of all of the column mapping parameter names found in the Sql Data Adapter config file  
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="clazz"></param>
        /// <param name="storedProcedureName"></param>
        /// <param name="adapterConfigurationFileName"></param>
        /// <returns></returns>
        public static List<T> PopulateAll<T>(this T clazz, string storedProcedureName, string adapterConfigurationFileName = "SqlDataAdapter.config") where T : ColumnMap
        {
            SQLAdapterConfiguration.SetConfig(adapterConfigurationFileName);
            Command cmd = new Command(storedProcedureName);
            List<T> result = DAO.ExecuteQuery(cmd.ToCommand(clazz)).ToColumnMapList(clazz);
            cmd.Dispose();
            return result;
        }

        /// <summary>
        /// Populates a single instance of a column mapping parameter name found in the Sql Data Adapter config file 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="clazz"></param>
        /// <param name="storedProcedureName"></param>
        /// <param name="adapterConfigurationFileName"></param>
        /// <returns></returns>
        public static T Populate<T>(this T clazz, string storedProcedureName, string adapterConfigurationFileName = "SqlDataAdapter.config") where T : ColumnMap
        {
            SQLAdapterConfiguration.SetConfig(adapterConfigurationFileName);
            Command cmd = new Command(storedProcedureName);
            T result = DAO.ExecuteQuery(cmd.ToCommand(clazz)).ToColumnMap(clazz);
            cmd.Dispose();
            return result;
        }

        /// <summary>
        /// Inserts a list of 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="clazz"></param>
        /// <param name="storedProcedureName"></param>
        /// <param name="adapterConfigurationFileName"></param>
        /// <returns></returns>
        public static int Insert<T>(this T clazz, string storedProcedureName, string adapterConfigurationFileName = "SqlDataAdapter.config") where T : ColumnMap
        {
            return NonQueryFromClass(clazz, storedProcedureName, adapterConfigurationFileName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="clazz"></param>
        /// <param name="storedProcedureName"></param>
        /// <param name="adapterConfigurationFileName"></param>
        /// <returns></returns>
        public static int Update<T>(this T clazz, string storedProcedureName, string adapterConfigurationFileName = "SqlDataAdapter.config") where T : ColumnMap
        {
            return NonQueryFromClass(clazz, storedProcedureName, adapterConfigurationFileName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="clazz"></param>
        /// <param name="storedProcedureName"></param>
        /// <param name="adapterConfigurationFileName"></param>
        /// <returns></returns>
        public static int Delete<T>(this T clazz, string storedProcedureName, string adapterConfigurationFileName = "SqlDataAdapter.config") where T : ColumnMap
        {
            return NonQueryFromClass(clazz, storedProcedureName, adapterConfigurationFileName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="clazz"></param>
        /// <param name="storedProcedureName"></param>
        /// <param name="adapterConfigurationFileName"></param>
        /// <returns></returns>
        private static int NonQueryFromClass<T>(T clazz, string storedProcedureName, string adapterConfigurationFileName) where T : ColumnMap
        {
            SQLAdapterConfiguration.SetConfig(adapterConfigurationFileName);
            Command cmd = new Command(storedProcedureName);
            return DAO.ExecuteNonQuery(cmd.ToCommand(clazz));
        }
    }
}
