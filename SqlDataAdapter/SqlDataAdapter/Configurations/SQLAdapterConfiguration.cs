using System.Configuration;
using System.IO;
using System.Reflection;

namespace SqlDataAdapter.Configurations
{
    public static class SQLAdapterConfiguration
    {
        private static string Path_SqlDataAdapterConfig = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Locati‌​on) + @"\SqlDataAdapter.config";

        private static Configuration AccessAdapterConfig(string path)
        {
            ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
            fileMap.ExeConfigFilename = path;
            return ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
        }

        private static ConfigurationSection GetSection(string sectionName)
        {
            Configuration configuration = AccessAdapterConfig(Path_SqlDataAdapterConfig);
            return configuration.GetSection(sectionName);
        }

        /// <summary>
        /// Returns the app setting section of a custom configuration file 
        /// </summary>
        /// <returns></returns>
        public static AppSettingsSection AppSettings()
        {
            return (AppSettingsSection)GetSection("appSettings");
        }

        /// <summary>
        /// Returns the column mapping section of a custom configuration file 
        /// </summary>
        /// <returns></returns>
        public static ColumnMapSection ColumnMappings()
        {
            return (ColumnMapSection)GetSection("DBMappings");
        }

        /// <summary>
        /// Returns the connection strings of a custom configuration file
        /// </summary>
        /// <returns></returns>
        public static ConnectionStringSettingsCollection ConnectionStrings()
        {
            Configuration configuration = AccessAdapterConfig(Path_SqlDataAdapterConfig);
            return configuration.ConnectionStrings.ConnectionStrings;
        }

        /// <summary>
        /// Set the custom configuration name
        /// </summary>
        /// <param name="fileName"></param>
        public static void SetConfig(string fileName, string filePath = "")
        {
            Path_SqlDataAdapterConfig = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Locati‌​on) + @"\" + fileName;
            if (filePath != "")
            {
                filePath.TrimEnd(' ', '\\');
                Path_SqlDataAdapterConfig = filePath + @"\" + fileName;
            }
            
        }

    }
}
