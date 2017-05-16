using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SqlDataAdapter
{
    public class Adapter
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

        public static AppSettingsSection AppSettings()
        {
            return (AppSettingsSection)GetSection("appSettings");
        }

        public static ColumnMapSection ColumnMappings()
        {
            //Configuration configuration = AccessAdapterConfig(Path_SqlDataAdapterConfig);
            //return (ColumnMapSection)configuration.GetSectionGroup("configuration").Sections.Get("columnMappings");
            return (ColumnMapSection)GetSection("DBMappings");// ?? new ColumnMapSection();
        }

        public static ConnectionStringSettingsCollection ConnectionStrings()
        {
            Configuration configuration = AccessAdapterConfig(Path_SqlDataAdapterConfig);
            return configuration.ConnectionStrings.ConnectionStrings;
        }

    }
}
