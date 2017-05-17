using System.Data.SqlClient;

namespace SqlDataAdapter.Communications
{
    public static class DAO
    {
        /// <summary>
        /// Sends the System.Data.SqlClient.SqlCommand.CommandText to the System.Data.SqlClient.SqlCommand.Connection
        /// and builds a System.Data.SqlClient.SqlDataReader.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="clazz"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        public static SqlDataReader ExecuteQuery(Command command)
        {
            SqlDataReader result = null;
            SqlCommand cmd = command.command;
            using (cmd)
            {
                cmd.Connection.Open();
                result = cmd.ExecuteReader();
            }
            cmd.Dispose();
            return result;
        }

        /// <summary>
        /// Executes the query, and returns the first column of the first row in the result 
        /// set returned by the query. Additional columns or rows are ignored.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static object ExecuteScalar(Command command)
        {
            object result = null;
            SqlCommand cmd = command.command;
            using (cmd)
            {
                cmd.Connection.Open();
                result = cmd.ExecuteScalar();
            }
            cmd.Dispose();
            return result;
        }

        /// <summary>
        /// Executes a Transact-SQL statement against the connection and returns the number 
        /// of rows affected.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(Command command)
        {
            SqlCommand cmd = command.command;
            int result = 0;
            using (cmd)
            {
                cmd.Connection.Open();
                result = cmd.ExecuteNonQuery();
            }
            cmd.Dispose();
            return result;
        }
    }
}
