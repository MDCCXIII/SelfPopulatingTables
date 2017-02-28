using SelfPopulatingTableFromAnnotation.Sql_Adapter;
using SelfPopulatingTableFromAnnotation.Sql_Adapter.BonesBuilder;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Reflection;

namespace SelfPopulatingTableFromAnnotation {
    class Program {
        public static string DefaultConnectionStringName = "SharedServer";
        
        static void Main(string[] args) {
            new Instance();
            Config.CheckAppSettings();
            SQLDataBase.Build();
            new ActionExamples().LoadActionsIntoDB();
            ActionExamples.Execute("Click");
            ActionExamples.Execute("SendText", new string[] { "I am writing all the text for this output." });
            ActionExamples.Execute("Update");
            Console.ReadLine();
        }
    }
}
