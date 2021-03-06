﻿using SqlDataAdapter.Attributes;
using SqlDataAdapter.Configurations;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;

namespace SqlDataAdapter.Communications
{
    public static class Transformer
    {
        private const string Procedure_GetProcedureParameterInformation = "GetProcedureParameterInformation";
        private const string Parameter_ProcedureName = "procedureName";

        /// <summary>
        /// Transforms a table map class to a Command object with its parameters defined from the column map attribute, ParameterMap
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storedProceedureName"></param>
        /// <param name="clazz"></param>
        /// <returns></returns>
        public static Command ToCommand<T>(string storedProceedureName, T clazz) where T : ColumnMap
        {
            Command cmd = new Command(storedProceedureName);
            cmd = ToCommand(cmd, clazz);
            return cmd;
        }

        /// <summary>
        /// Transforms a table map class into a Command object with its parameters defined from the column map attribute, ParameterMap
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storedProceedureName"></param>
        /// <param name="clazz"></param>
        /// <returns></returns>
        public static Command ToCommand<T>(this Command cmd, T clazz) where T : ColumnMap
        {
            Command command = new Command(Procedure_GetProcedureParameterInformation);
            command.AddParameter(Parameter_ProcedureName, cmd.command.CommandText);
            List<string> storedProcedureParameters = DAO.ExecuteQuery(command).ToStringList();

            SetParameterFromField(cmd, clazz, storedProcedureParameters);
            SetParameterFromProperty(cmd, clazz, storedProcedureParameters);
            return cmd;
        }

        private static void SetParameterFromProperty<T>(Command cmd, T clazz, List<string> storedProcedureParameters) where T : ColumnMap
        {
            foreach (PropertyInfo p in typeof(T).GetProperties())
            {
                ColumnMap attr = (ColumnMap)Attribute.GetCustomAttribute(p, typeof(ColumnMap));
                if (attr != null)
                {
                    if (attr.parameter != null && storedProcedureParameters.Contains(attr.parameter))
                    {
                        cmd.AddParameter(attr.parameter, p.GetValue(clazz));
                    }
                    else if (attr.Name != null && SQLAdapterConfiguration.ColumnMappings().ColumnMap[attr.Name] != null)
                    {
                        string columnParameterName = SQLAdapterConfiguration.ColumnMappings().ColumnMap[attr.Name].ParameterName;
                        if (storedProcedureParameters.Contains(columnParameterName))
                        {
                            cmd.AddParameter(columnParameterName, p.GetValue(clazz));
                        }
                    }
                }
            }
        }

        private static void SetParameterFromField<T>(Command cmd, T clazz, List<string> storedProcedureParameters) where T : ColumnMap
        {
            foreach (FieldInfo f in typeof(T).GetFields())
            {
                ColumnMap attr = (ColumnMap)Attribute.GetCustomAttribute(f, typeof(ColumnMap));
                if (attr != null)
                {
                    if (attr.parameter != null && storedProcedureParameters.Contains(attr.parameter))
                    {
                        cmd.AddParameter(attr.parameter, f.GetValue(clazz));
                    }
                    else if (attr.Name != null && SQLAdapterConfiguration.ColumnMappings().ColumnMap[attr.Name] != null)
                    {
                        string columnParameterName = SQLAdapterConfiguration.ColumnMappings().ColumnMap[attr.Name].ParameterName;
                        if (storedProcedureParameters.Contains(columnParameterName))
                        {
                            cmd.AddParameter(columnParameterName, f.GetValue(clazz));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Returns each record of a query as an entry in a list of equivalent column map classes
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rdr"></param>
        /// <param name="clazz"></param>
        /// <returns></returns>
        public static List<T> ToColumnMapList<T>(this SqlDataReader rdr, T clazz) where T : ColumnMap
        {
            List<T> result = new List<T>();
            List<string> columns = GetMatchingColumns(rdr, clazz);
            while (rdr.Read())
            {
                result.Add(SetMatchingColumns<T>(rdr, columns));
            }
            rdr.Dispose();
            return result;
        }

        /// <summary>
        /// Returns the first record of a query as its equivalent column map class
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rdr"></param>
        /// <param name="clazz"></param>
        /// <returns></returns>
        public static T ToColumnMap<T>(this SqlDataReader rdr, T clazz) where T : ColumnMap
        {
            T result = (T)Activator.CreateInstance(typeof(T), new object[] { });
            List<string> columns = GetMatchingColumns(rdr, clazz);
            while (rdr.Read())
            {
                result = SetMatchingColumns<T>(rdr, columns);
                break;
            }
            rdr.Dispose();
            return result;
        }

        /// <summary>
        /// Sets field values for columns found in both the rdr and T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rdr"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        private static T SetMatchingColumns<T>(SqlDataReader rdr, List<string> columns) where T : ColumnMap
        {
            T clazz = (T)Activator.CreateInstance(typeof(T));
            foreach (string column in columns)
            {
                clazz.SetValue<T>(clazz, column, rdr[column]);
            }

            return clazz;
        }

        /// <summary>
        /// Builds a list of all columns found in both the rdr and the clazz
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rdr"></param>
        /// <param name="clazz"></param>
        /// <returns></returns>
        private static List<string> GetMatchingColumns<T>(SqlDataReader rdr, T clazz) where T : ColumnMap
        {
            List<string> columns = new List<string>();
            for (int i = 0; i < rdr.FieldCount; i++)
            {
                if (clazz.HasColumn<T>(clazz, rdr.GetName(i)))
                {
                    columns.Add(rdr.GetName(i));
                }
            }

            return columns;
        }

        /// <summary>
        /// Returns each record of a query as an entry in a list of equivalent Dictionary's
        /// </summary>
        /// <param name="rdr"></param>
        /// <returns></returns>
        public static List<Dictionary<string, object>> ToDictionaryList(this SqlDataReader rdr)
        {
            List<Dictionary<string, object>> result = new List<Dictionary<string, object>>();

            int rownum = -1;
            while (rdr.Read())
            {
                rownum++;
                result.Add(new Dictionary<string, object>());
                for (int i = 0; i < rdr.FieldCount; i++)
                {
                    result[rownum].Add(rdr.GetName(i), rdr.GetValue(i));
                }
            }
            rdr.Dispose();
            return result;
        }

        /// <summary>
        /// Returns the first record of a query as a Dictionary
        /// </summary>
        /// <param name="rdr"></param>
        /// <returns></returns>
        public static Dictionary<string, object> ToDictionary(this SqlDataReader rdr)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            while (rdr.Read())
            {
                for (int i = 0; i < rdr.FieldCount; i++)
                {
                    result.Add(rdr.GetName(i), rdr.GetValue(i));
                }
                break;
            }
            rdr.Dispose();
            return result;
        }

        /// <summary>
        /// Transforms a SqlDataReader into a list whos indices are equal to each column of each row of all values in the reader.
        /// </summary>
        /// <param name="rdr"></param>
        /// <returns></returns>
        public static List<object> ToList(this SqlDataReader rdr)
        {
            List<object> result = new List<object>();
            while (rdr.Read())
            {
                for (int i = 0; i < rdr.FieldCount; i++)
                {
                    result.Add(rdr.GetValue(i));
                }
                break;
            }
            rdr.Dispose();
            return result;
        }

        /// <summary>
        /// Transforms a SqlDataReader into a list whos indices are equal to each column of each row of all values in the reader.
        /// </summary>
        /// <param name="rdr"></param>
        /// <returns></returns>
        public static List<string> ToStringList(this SqlDataReader rdr)
        {
            List<string> result = new List<string>();
            while (rdr.Read())
            {
                for (int i = 0; i < rdr.FieldCount; i++)
                {
                    result.Add(rdr[i].ToString());
                }
            }
            rdr.Dispose();
            return result;
        }
    }
}
