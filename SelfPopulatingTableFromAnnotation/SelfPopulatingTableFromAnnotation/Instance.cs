using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace SelfPopulatingTableFromAnnotation
{
    public class Instance
    {
        public static string Current = Environment.MachineName + Assembly.GetExecutingAssembly().GetName().Version;
        public static List<string> ALL = new List<string>();
        public static string CSL = "";
        private static string FilePath_InstanceIds = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\")) + @"InstanceIds.txt";

        public Instance()
        {
            LoadALL();
            AddCurrent();
            BuildCSL();
        }

        private static void LoadALL()
        {
            if (File.Exists(FilePath_InstanceIds))
            {
                using (StreamReader sr = new StreamReader(FilePath_InstanceIds))
                {
                    string id = "";
                    while (!sr.EndOfStream)
                    {
                        id = sr.ReadLine();
                        if (!ALL.Contains(id))
                        {
                            ALL.Add(id);
                        }
                    }
                }
            }
            else
            {
                throw new Exception("The file " + FilePath_InstanceIds + " does not exist.");
            }
        }

        private static void AddCurrent()
        {
            if (!ALL.Contains(Current))
            {
                using (StreamWriter sw = new StreamWriter(FilePath_InstanceIds, true))
                {
                    sw.WriteLine(Current);
                }
                ALL.Add(Current);
            }
        }

        private static void BuildCSL()
        {

            CSL = string.Join(",", ALL.ToArray());
        }
    }
}
