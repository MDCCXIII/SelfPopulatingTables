using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace SelfPopulatingTableFromAnnotation.Sql_Adapter {
    public class Command {
        public SqlCommand command { get; }

        public Command(string cmdText) {
            command = new SqlCommand(cmdText);
            SetCommandType();
        }

        public void AddParameter(string parameterName, string parameterValue) {
            command.Parameters.Add(new SqlParameter(parameterName, parameterValue));
        }

        public void AddParameters(Dictionary<string, string> Parameters) {
            foreach (KeyValuePair<string, string> parameter in Parameters) {
                command.Parameters.Add(new SqlParameter(parameter.Key, parameter.Value));
            }
        }

        public void AddParameter(SqlParameter parameter) {
            command.Parameters.Add(parameter);
        }

        public void AddParameters(SqlParameter[] Parameters) {
            foreach (SqlParameter parameter in Parameters) {
                command.Parameters.Add(parameter);
            }
        }

        public void setConnection(SqlConnection conn) {
            command.Connection = conn;
        }
        public void BuildConnection(string connectionString) {
            command.Connection = new SqlConnection(connectionString);
        }

        public void BuildConnection() {
            string connectionString = ConfigurationManager.ConnectionStrings[Program.DefaultConnectionStringName].ConnectionString;
            command.Connection = new SqlConnection(connectionString);
        }

        public void SetCommandType(System.Data.CommandType commandType = System.Data.CommandType.StoredProcedure) {
            command.CommandType = commandType;
        }

        public void Dispose() {
            try {
                this.command.Connection.Dispose();
                this.command.Dispose();
            }
            catch (Exception) {

            }
        }
    }
    public static class CommandExtensions {
        public static SqlCommand CheckConnectivity(this SqlCommand cmd) {
            if (cmd.Connection == null) {
                string connectionString = ConfigurationManager.ConnectionStrings[Program.DefaultConnectionStringName].ConnectionString;
                cmd.Connection = new SqlConnection(connectionString);
            }
            return cmd;
        }

        public static SqlCommand CheckConnectivity(this SqlCommand cmd, string connectionName) {
            if (cmd.Connection == null) {
                if (ConfigurationManager.AppSettings[connectionName] == null) {
                    throw new Exception(connectionName + " is not a valid key in the App.Config");
                }
                string connectionString = ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;
                cmd.Connection = new SqlConnection(connectionString);
            }
            return cmd;
        }
    }
}