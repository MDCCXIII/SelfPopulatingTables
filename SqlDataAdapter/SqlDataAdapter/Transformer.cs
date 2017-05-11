﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Reflection;

namespace SqlDataAdapter
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
            foreach (FieldInfo f in typeof(T).GetFields())
            {
                ColumnMap attr = (ColumnMap)f.GetCustomAttribute(typeof(T));

                if (attr.parameter != null && attr.parameter != null)
                {
                    Command command = new Command(Procedure_GetProcedureParameterInformation);
                    command.AddParameter(Parameter_ProcedureName, cmd.command.CommandText);
                    List<object> storedProcedureParameters = DAO.ExecuteQuery(command).ToList();
                    if (storedProcedureParameters.Contains(attr.parameter))
                    {
                        cmd.AddParameter(attr.parameter, f.GetValue(clazz));
                    }
                }
            }
            return cmd;
        }

        /// <summary>
        /// Transforms a table map class into a Command object with its parameters defined from the configuration Parameter Name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storedProceedureName"></param>
        /// <param name="clazz"></param>
        /// <returns></returns>
        public static Command ToCommandFromConfiguration<T>(string storedProceedureName, T clazz) where T : ColumnMap
        {
            Command cmd = new Command(storedProceedureName);
            foreach (FieldInfo f in typeof(T).GetFields())
            {
                ColumnMap attr = (ColumnMap)f.GetCustomAttribute(typeof(T));

                if (attr.parameter != null && attr.parameter != null)
                {
                    Command command = new Command(Procedure_GetProcedureParameterInformation);
                    command.AddParameter(Parameter_ProcedureName, cmd.command.CommandText);
                    List<object> storedProcedureParameters = DAO.ExecuteQuery(command).ToList();
                    Configuration configuration = Local.AccessAdapterConfig(Local.Path_SqlDataAdapterConfig);
                    IDictionary columnMap = (IDictionary)configuration.GetSection("columnMap");
                    if (storedProcedureParameters.Contains(columnMap[attr.Name]))
                    {
                        cmd.AddParameter(attr.parameter, f.GetValue(clazz));
                    }
                }
            }
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
            foreach (FieldInfo f in typeof(T).GetFields())
            {
                ColumnMap attr = (ColumnMap)f.GetCustomAttribute(typeof(T));

                if (attr.Name != null && attr.parameter != null)
                {
                    Command command = new Command(Procedure_GetProcedureParameterInformation);
                    command.AddParameter(Parameter_ProcedureName, cmd.command.CommandText);
                    List<object> storedProcedureParameters = DAO.ExecuteQuery(command).ToList();
                    if (storedProcedureParameters.Contains(attr.parameter))
                    {
                        cmd.AddParameter(attr.parameter, f.GetValue(clazz));
                    }
                }
            }
            return cmd;
        }

        /// <summary>
        /// Transforms a table map class into a Command object with its parameters defined from the configuration Parameter Name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cmd"></param>
        /// <param name="clazz"></param>
        /// <returns></returns>
        public static Command ToCommandFromConfiguration<T>(this Command cmd, T clazz) where T : ColumnMap
        {
            foreach (FieldInfo f in typeof(T).GetFields())
            {
                ColumnMap attr = (ColumnMap)f.GetCustomAttribute(typeof(T));

                if (attr.Name != null)
                {
                    Command command = new Command(Procedure_GetProcedureParameterInformation);
                    command.AddParameter(Parameter_ProcedureName, cmd.command.CommandText);
                    List<object> storedProcedureParameters = DAO.ExecuteQuery(command).ToList();
                    Configuration configuration = Local.AccessAdapterConfig(Local.Path_SqlDataAdapterConfig);
                    IDictionary columnMap = (IDictionary)configuration.GetSection("columnMap");
                    AppSettingsSection appSettings = ((AppSettingsSection)configuration.GetSection("appSettings"));
                    if (storedProcedureParameters.Contains(columnMap[attr.Name]))
                    {
                        cmd.AddParameter(appSettings.Settings[attr.Name].Value, f.GetValue(clazz));
                    }
                }
            }
            return cmd;
        }

        /// <summary>
        /// Disposes of a SqlDataReader object
        /// </summary>
        /// <param name="rdr"></param>
        private static void Dispose(this SqlDataReader rdr)
        {
            try
            {
                rdr.Close();
                rdr = null;
            }
            catch
            {

            }
        }

        /// <summary>
        /// Returns each record of a query as an entry in a list of equivalent table map classes
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rdr"></param>
        /// <param name="clazz"></param>
        /// <returns></returns>
        public static List<T> ToTableMapList<T>(this SqlDataReader rdr, T clazz) where T : ColumnMap
        {
            List<T> result = new List<T>();
            List<string> columns = new List<string>();
            for (int i = 0; i < rdr.FieldCount; i++)
            {
                if (clazz.HasColumn<T>(clazz, rdr.GetName(i)))
                {
                    columns.Add(rdr.GetName(i));
                }
            }
            while (rdr.Read())
            {
                clazz = (T)Activator.CreateInstance(typeof(T));
                foreach (string column in columns)
                {
                    if (rdr.GetFieldType(rdr.GetOrdinal(column)).Name.Equals("Int32") && rdr[column].ToString().Equals(""))
                    {
                        clazz.SetValue<T>(clazz, column, 0);
                    }
                    else
                    {
                        clazz.SetValue<T>(clazz, column, rdr[column]);
                    }
                }
                result.Add(clazz);
            }
            rdr.Dispose();
            return result;
        }

        /// <summary>
        /// Returns the first record of a query as its equivalent table map class
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rdr"></param>
        /// <param name="clazz"></param>
        /// <returns></returns>
        public static T ToTableMap<T>(this SqlDataReader rdr, T clazz) where T : ColumnMap
        {
            T result = (T)Activator.CreateInstance(typeof(T), new object[] { });
            List<string> columns = new List<string>();
            for (int i = 0; i < rdr.FieldCount; i++)
            {
                if (clazz.HasColumn<T>(clazz, rdr.GetName(i)))
                {
                    columns.Add(rdr.GetName(i));
                }
            }
            while (rdr.Read())
            {
                clazz = (T)Activator.CreateInstance(typeof(T));
                foreach (string column in columns)
                {
                    if (rdr.GetFieldType(rdr.GetOrdinal(column)).Name.Equals("Int32") && rdr[column].ToString().Equals(""))
                    {
                        clazz.SetValue<T>(clazz, column, 0);
                    }
                    else
                    {
                        clazz.SetValue<T>(clazz, column, rdr[column]);
                    }
                }
                result = clazz;
                break;
            }
            rdr.Dispose();
            return result;
        }

        /// <summary>
        /// Returns true if the rdr has rows
        /// </summary>
        /// <param name="rdr"></param>
        /// <returns></returns>
        public static bool HasRows(this SqlDataReader rdr)
        {
            bool result = false;
            result = rdr.HasRows();
            Dispose(rdr);
            return result;
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
                    result.Add(rdr.GetValue(i).ToString());
                }
                break;
            }
            rdr.Dispose();
            return result;
        }
    }
}
