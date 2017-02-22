using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;

namespace SelfPopulatingTableFromAnnotation.Sql_Adapter {
    [AttributeUsage(AttributeTargets.Method)]
    public class ActionMap : Attribute {
        Dictionary<string, string> ColumnMap = new Dictionary<string,string>();
        public static string ColumnName_ActionName = null;
        public static string ColumnName_RequiredParameters = null;
        public static string ColumnName_OptionalParameters = null;
        public static string ColumnName_ActionDescription = null;

        public ActionMap() {
            CheckAppConfig();
            ColumnName_ActionName = ConfigurationManager.AppSettings.Get("ActionTableProceedure_ActionName_ParameterName");
            ColumnName_RequiredParameters = ConfigurationManager.AppSettings.Get("ActionTableProceedure_RequiredParameters_ParameterName");
            ColumnName_OptionalParameters = ConfigurationManager.AppSettings.Get("ActionTableProceedure_OptionalParameters_ParameterName");
            ColumnName_ActionDescription = ConfigurationManager.AppSettings.Get("ActionTableProceedure_ActionDescription_ParameterName");
        }

        public ActionMap(string actionDescription, [System.Runtime.CompilerServices.CallerMemberName] string memberName = "") {
            ColumnMap.Add(ColumnName_ActionName, memberName);
            ColumnMap.Add(ColumnName_RequiredParameters, "");
            ColumnMap.Add(ColumnName_OptionalParameters, "");
            ColumnMap.Add(ColumnName_ActionDescription, actionDescription);
            foreach(ParameterInfo pi in typeof(ActionMap).GetMethod(memberName).GetParameters()) {
                if (pi.IsOptional) {
                    ColumnMap[ColumnName_OptionalParameters] += pi.Name + ",";
                } else {
                    ColumnMap[ColumnName_RequiredParameters] += pi.Name + ",";
                }
            }
            ColumnMap[ColumnName_RequiredParameters].TrimEnd(',');
            ColumnMap[ColumnName_OptionalParameters].TrimEnd(',');
        }

        public virtual Dictionary<string,string> TableRecord {
            get { return ColumnMap; }
        }
        public static void CheckAppConfig() {
            List<string> Keys = new List<string>();
            Keys.Add("InstertActionsIntoActionsProceedureName");
            Keys.Add("CheckForActionProceedureName");
            Keys.Add("ActionTableProceedure_ActionName_ParameterName");
            Keys.Add("ActionTableProceedure_RequiredParameters_ParameterName");
            Keys.Add("ActionTableProceedure_OptionalParameters_ParameterName");
            Keys.Add("ActionTableProceedure_ActionDescription_ParameterName");
            
            foreach(string key in Keys) {
                if(ConfigurationManager.AppSettings[key] == null) {
                    throw new Exception("The App.Config does not contains the key \"" + key + "\" in the <appSettings>.");
                }
                if(ConfigurationManager.AppSettings[key] == "") {
                    throw new Exception("The App.Config <appSettings> for \"" + key + "\" has a value of \"\".");
                }
            }
        }
    }

    public static class ActionMapExtensions {
      public static void LoadActionsIntoDB<T>(this T clazz) where T : ActionMap {
            //T c = (T)Activator.CreateInstance(typeof(T), new object[] { }); ;
            foreach (MethodInfo mi in clazz.GetType().GetMethods()) {
                var methodInfo = typeof(T).GetMethod(mi.Name);
                Command check = new Command(ConfigurationManager.AppSettings.Get("CheckForActionProceedureName"));
                check.AddParameter(ActionMap.ColumnName_ActionName, GetTableRecord(methodInfo)[ActionMap.ColumnName_ActionName]);
                if (DAO.RowInTable(check)) {
                    Command command = new Command(ConfigurationManager.AppSettings.Get("InstertActionsIntoActionsProceedureName"));
                    command.AddParameters(GetTableRecord(methodInfo));
                    DAO.ExecuteStoredNonQuery(command);
                }

            }
        }

        private static Dictionary<string, string> GetTableRecord(MethodInfo methodInfo) {
            return ((ActionMap)Attribute.GetCustomAttribute(methodInfo, typeof(ActionMap))).TableRecord;
        }
        
    }
}
