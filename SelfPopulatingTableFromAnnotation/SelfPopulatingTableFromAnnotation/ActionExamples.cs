using SelfPopulatingTableFromAnnotation.Sql_Adapter;
using System;

namespace SelfPopulatingTableFromAnnotation {
    public class ActionExamples : ActionMap {

        public ActionExamples() {
            new ActionMap();
        }

        /// <summary>
        /// Takes in the step actionName and step Parameters and calls the appropriate method as delegate function.
        /// </summary>
        /// <param name="methodNameToExecute"></param>
        /// <param name="parameters"></param>
        public static void Execute(string methodNameToExecute, string parameters = "") {
            if(parameters != "") {
                Delegate.CreateDelegate(typeof(ActionExamples), typeof(ActionExamples).GetMethod(methodNameToExecute)).DynamicInvoke(parameters.Split(','));
            } else {
                Delegate.CreateDelegate(typeof(ActionExamples), typeof(ActionExamples).GetMethod(methodNameToExecute)).DynamicInvoke();
            }
        }

        [ActionMap("Preforms a click on the steps control.")]
        private void Click(string Required, string Optional = "") {
            Console.WriteLine("Required: " + Required);
            Console.WriteLine("Optional: " + Optional);
            Console.WriteLine("I Clicked!!!");
        }
    }
}
