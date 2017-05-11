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
    public class Local
    {
        public static string Path_SqlDataAdapterConfig = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Locati‌​on) + @"\SqlDataAdapter.config";
        public static Configuration AccessAdapterConfig(string path)
        {
            ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
            fileMap.ExeConfigFilename = path;
            return ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
        }
    }
}
