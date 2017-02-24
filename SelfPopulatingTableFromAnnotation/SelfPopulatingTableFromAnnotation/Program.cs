using SelfPopulatingTableFromAnnotation.Sql_Adapter;
using System;

namespace SelfPopulatingTableFromAnnotation {
    class Program {
        //TODO: Add Default connection string in App.Config
        //TODO: Create Query that Inserts new Action Records
        //TODO: Create Query that Gets all rows with a provided action name
        //TODO: Create a class that creates a Stored Proceedure in the DB For the two queries above if those queries do not already exist.
        //TODO: Create a class that creates the Actions table in the DB if it does not already exist
        public static string DefaultConnectionStringName = "TestDB";
        //public const string ActionCheckProceedureName = "getActionRowsByName";

        static void Main(string[] args) {
            new ActionExamples().LoadActionsIntoDB();
            ActionExamples.Execute("Click", new string[] { "req", "opt" });
            Console.ReadLine();
        }
    }
}
