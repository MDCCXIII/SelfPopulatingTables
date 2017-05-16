using System.Data.SqlClient;

namespace SqlDataAdapter.Communications
{
    public static class DAO
    {
        /// <summary>
        /// Executes a proceedure and populates the class that called this method.
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
        /// Executes Insert, Update or Delete Queries and returns the number of rows affected;
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
