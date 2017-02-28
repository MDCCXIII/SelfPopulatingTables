using SelfPopulatingTableFromAnnotation.Sql_Adapter;
using SelfPopulatingTableFromAnnotation.Sql_Adapter.BonesBuilder;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Reflection;

namespace SelfPopulatingTableFromAnnotation {
    class Program {
        public static string DefaultConnectionStringName = "TestDB";
        

        static void Main(string[] args) {
            new Instance();
            CheckAppConfig();
            SQLDataBase.Build();
            new ActionExamples().LoadActionsIntoDB();
            ActionExamples.Execute("Click");
            ActionExamples.Execute("SendText", new string[] { "I am writing all the text for this output." });
            Console.ReadLine();
        }

        public static void CheckAppConfig()
        {
            List<string> Keys = new List<string>();
            Keys.Add("getActionByActionName");
            Keys.Add("getActionInformation");
            Keys.Add("InstertAction");
            Keys.Add("UpdateAction");
            Keys.Add("DeleteAction");
            Keys.Add("Action_Information");
            Keys.Add("actionName");
            Keys.Add("requiredParameters");
            Keys.Add("optionalParameters");
            Keys.Add("actionDescription");

            foreach (string key in Keys)
            {
                if (ConfigurationManager.AppSettings[key] == null)
                {
                    throw new Exception("The App.Config does not contains the key \"" + key + "\" in the <appSettings>.");
                }
                if (ConfigurationManager.AppSettings[key] == "")
                {
                    throw new Exception("The App.Config <appSettings> for \"" + key + "\" has a value of \"\".");
                }
            }
        }

        public static string CurrentInstanceID = Environment.MachineName + Assembly.GetExecutingAssembly().GetName().Version;
        public static List<string> AllInstanceIds = new List<string>();
        public static string CSL_InstanceIds = "";
        public static string FilePath_InstanceIds = Environment.CurrentDirectory + "\\InstanceIds.txt";

        private static void LoadApplicationInstanceIds()
        {
            if (File.Exists(FilePath_InstanceIds))
            {
                StreamReader sr = new StreamReader(FilePath_InstanceIds);
                string id = "";
                while (!sr.EndOfStream)
                {
                    id = sr.ReadLine();
                    if (!AllInstanceIds.Contains(id))
                    {
                        AllInstanceIds.Add(id);
                    }
                }
            }
        }

        public static void AddInstanceId()
        {
            LoadApplicationInstanceIds();
            using (StreamWriter sw = new StreamWriter(FilePath_InstanceIds, true))
            {
                if (!AllInstanceIds.Contains(CurrentInstanceID))
                {
                    sw.WriteLine(CurrentInstanceID);
                }
            }
        }

        public static string InsanceIdsAsCSLine()
        {
            string result = "";
            foreach(string instanceId in AllInstanceIds)
            {
                result += instanceId + ",";
            }
            return result.TrimEnd(',');
        }
    }
}
