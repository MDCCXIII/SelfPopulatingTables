﻿using System.Configuration;
using System.IO;
using System.Reflection;

namespace SqlDataAdapter.Configurations
{
    public class SQLAdapterConfiguration
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

        public static AppSettingsSection AppSettings(string FilePath)
        {
            SetConfigPath(FilePath);
            return (AppSettingsSection)GetSection("appSettings");
        }

        public static ColumnMapSection ColumnMappings()
        {
            return (ColumnMapSection)GetSection("DBMappings");
        }

        public static ColumnMapSection ColumnMappings(string FilePath)
        {
            SetConfigPath(FilePath);
            return (ColumnMapSection)GetSection("DBMappings");
        }

        public static ConnectionStringSettingsCollection ConnectionStrings()
        {
            Configuration configuration = AccessAdapterConfig(Path_SqlDataAdapterConfig);
            return configuration.ConnectionStrings.ConnectionStrings;
        }

        public static ConnectionStringSettingsCollection ConnectionStrings(string FilePath)
        {
            SetConfigPath(FilePath);
            Configuration configuration = AccessAdapterConfig(Path_SqlDataAdapterConfig);
            return configuration.ConnectionStrings.ConnectionStrings;
        }

        public static void SetConfigPath(string FilePath)
        {
            Path_SqlDataAdapterConfig = FilePath;
        }

    }
}