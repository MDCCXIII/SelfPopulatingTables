using SelfPopulatingTableFromAnnotation.Sql_Adapter;
using System;

namespace SelfPopulatingTableFromAnnotation {
    public class ActionExamples : ActionMap {

        public ActionExamples() {
            //new ActionMap();
        }

        /// <summary>
        /// Takes in the step actionName and step Parameters and calls the appropriate method as delegate function.
        /// </summary>
        /// <param name="methodNameToExecute"></param>
        /// <param name="parameters"></param>
        public static void Execute(string methodNameToExecute, string[] parameters = null) {
            switch (methodNameToExecute)
            {
                case "Click":
                    Click(parameters[0], parameters[1]);
                    break;
            }
        }

        [ActionMap("Preforms a click on the steps control.")]
        public static void Click(string Required, string Optional = "") {
            Console.WriteLine("Required: " + Required);
            Console.WriteLine("Optional: " + Optional);
            Console.WriteLine("I Clicked!!!");
        }

        [ActionMap("Sends the provided text to the step control.")]
        public static void SendText()
        {

        }
    }
}
