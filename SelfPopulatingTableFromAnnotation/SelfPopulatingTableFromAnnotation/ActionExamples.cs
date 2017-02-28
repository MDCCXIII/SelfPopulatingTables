using SelfPopulatingTableFromAnnotation.Sql_Adapter;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace SelfPopulatingTableFromAnnotation
{
    public class ActionExamples : ActionMap {

        /// <summary>
        /// Takes in the step actionName and step Parameters and calls the appropriate method as delegate function.
        /// </summary>
        /// <param name="methodNameToExecute"></param>
        /// <param name="parameters"></param>
        public static void Execute(string methodNameToExecute, string[] parameters = null) {
            MethodInfo method = typeof(ActionExamples).GetMethod(methodNameToExecute);
            try
            {
                Type deligateType = Expression.GetDelegateType(
                    (from parameter in method.GetParameters() select parameter.ParameterType)
                    .Concat(new[] { method.ReturnType }).ToArray());
                method.CreateDelegate(deligateType).DynamicInvoke(parameters);
            }
            catch (ArgumentException)
            {
            }
        }

        [ActionMap("Preforms a click on the steps control.")]
        public static void Click() {
            Console.WriteLine("I Clicked!!!");
        }

        [ActionMap("Sends the provided text to the step control.")]
        public static void SendText(string text)
        {
            Console.WriteLine(text);
        }

        [ActionMap("Preforms an update on a table.")]
        public static void Update()
        {
            Console.WriteLine("I'm Updating.");
        }
    }
}
