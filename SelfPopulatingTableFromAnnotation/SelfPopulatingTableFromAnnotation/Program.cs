using SelfPopulatingTableFromAnnotation.Sql_Adapter;
using SelfPopulatingTableFromAnnotation.Sql_Adapter.BonesBuilder;
using System;

namespace SelfPopulatingTableFromAnnotation
{
    class Program {
        public static string DefaultConnectionStringName = "SharedServer";
        
        static void Main(string[] args) {
            new Instance();
            Config.CheckAppSettings();
            SQLDataBase.Build();
            new ActionExamples().LoadActionsIntoDB();

            //////TEST CODE BELOW///////
            ActionExamples.Execute("Click");
            ActionExamples.Execute("SendText", new string[] { "I am writing all the text for this output." });
            ActionExamples.Execute("Update");
            Console.ReadLine();
        }
    }
}
