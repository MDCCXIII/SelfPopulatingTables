using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;

namespace SqlDataAdapter
{
    public class Command
    {
        private const string DefaultConnectionStringKey = "DefaultConnectionString";

        /// <summary>
        /// SqlCommand Setter
        /// </summary>
        public SqlCommand command { get; internal set; }

        /// <summary>
        /// Sets the command object type and text
        /// </summary>
        /// <param name="cmdText"></param>
        /// <exception cref="NullReferenceException"></exception>
        /// <exception cref="AggregateException"></exception>
        public Command(string cmdText)
        {
            if (cmdText == null)
            {
                throw new NullReferenceException();
            }
            else if (cmdText.Equals(""))
            {
                throw new AggregateException();
            }
            command = new SqlCommand(cmdText);
            SetCommandType();
            BuildConnection();
        }

        /// <summary>
        /// Sets command type of the Sql command object
        /// </summary>
        /// <param name="commandType"></param>
        public void SetCommandType(System.Data.CommandType commandType = System.Data.CommandType.StoredProcedure)
        {
            command.CommandType = commandType;
        }

        /// <summary>
        /// Sets the parameter Name and Value to the command object
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="parameterValue"></param>
        public void AddParameter(string parameterName, object parameterValue)
        {
            command.Parameters.AddWithValue(parameterName, parameterValue);
        }

        /// <summary>
        /// Sets Parameters from Dictionary into the command object
        /// </summary>
        /// <param name="Parameters"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void AddParameters(Dictionary<string, object> Parameters)
        {
            foreach (KeyValuePair<string, object> parameter in Parameters)
            {
                if (Parameters == null || Parameters.Equals(""))
                {
                    throw new ArgumentNullException();
                }
                command.Parameters.Add(new SqlParameter(parameter.Key, parameter.Value));
            }
        }

        /// <summary>
        /// Sets SqlParameter into command object
        /// </summary> 
        /// <param name="parameter"></param>
        /// <exception cref="NullReferenceException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public void AddParameter(SqlParameter parameter)
        {
            if (parameter == null)
            {
                throw new NullReferenceException();
            }
            else if (parameter.ParameterName.Equals(""))
            {
                throw new ArgumentException();
            }
            command.Parameters.Add(parameter);
        }

        /// <summary>
        /// sets Sql Parameters from an array into the command object 
        /// </summary>
        /// <param name="Parameters"></param>
        public void AddParameters(SqlParameter[] Parameters)
        {
            foreach (SqlParameter parameter in Parameters)
            {
                command.Parameters.Add(parameter);
            }
        }

        /// <summary>
        /// sets the connection string of the command object
        /// </summary>
        /// <param name="connectionString"></param>
        public void BuildConnection(string connectionString)
        {
            command.Connection = new SqlConnection(connectionString);
        }

        /// <summary>
        /// sets the default of the command object from the app config
        /// program.DefaultConnectionString defines the app config key for the default connectin string
        /// </summary>
        public void BuildConnection()
        {
            Configuration configuration = Local.AccessAdapterConfig(Local.Path_SqlDataAdapterConfig);
            AppSettingsSection appSettings = ((AppSettingsSection)configuration.GetSection("appSettings"));
            string defaultConnectionName = appSettings.Settings[DefaultConnectionStringKey].Value;
            string connectionString = configuration.ConnectionStrings.ConnectionStrings[defaultConnectionName].ConnectionString;
            command.Connection = new SqlConnection(connectionString);
        }

        /// <summary>
        /// Disposes the connection and command object
        /// </summary>
        public void Dispose()
        {
            try
            {
                this.command.Connection.Dispose();
                this.command.Dispose();
            }
            catch (Exception)
            {

            }

        }
    }
}