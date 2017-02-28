using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Reflection;

namespace SelfPopulatingTableFromAnnotation.Sql_Adapter {
    [AttributeUsage(AttributeTargets.Method)]
    public class ActionMap : Attribute {
        Dictionary<string, string> ColumnMap = new Dictionary<string,string>();
        public static string ColumnName_ActionName = null;
        public static string ColumnName_RequiredParameters = null;
        public static string ColumnName_OptionalParameters = null;
        public static string ColumnName_ActionDescription = null;
        public static MethodInfo mi;

        public ActionMap() {
            ColumnName_ActionName = ConfigurationManager.AppSettings.Get("actionName");
            ColumnName_RequiredParameters = ConfigurationManager.AppSettings.Get("requiredParameters");
            ColumnName_OptionalParameters = ConfigurationManager.AppSettings.Get("optionalParameters");
            ColumnName_ActionDescription = ConfigurationManager.AppSettings.Get("actionDescription");
        }

        public ActionMap(string actionDescription, [System.Runtime.CompilerServices.CallerMemberName] string memberName = "") {
            ColumnMap = new Dictionary<string, string>();
            ColumnMap.Add(ColumnName_ActionName, memberName);
            ColumnMap.Add(ColumnName_RequiredParameters, "");
            ColumnMap.Add(ColumnName_OptionalParameters, "");
            ColumnMap.Add(ColumnName_ActionDescription, actionDescription);
            foreach (ParameterInfo pi in mi.GetParameters()) {
                if (pi.IsOptional) {
                    ColumnMap[ColumnName_OptionalParameters] += pi.Name + ",";
                } else {
                    ColumnMap[ColumnName_RequiredParameters] += pi.Name + ",";
                }
            }
            ColumnMap[ColumnName_RequiredParameters] = ColumnMap[ColumnName_RequiredParameters].TrimEnd(',');
            ColumnMap[ColumnName_OptionalParameters] = ColumnMap[ColumnName_OptionalParameters].TrimEnd(',');
        }

        public virtual Dictionary<string,string> TableRecord {
            get { return ColumnMap; }
        }
        
    }

    public static class ActionMapExtensions {
      public static void LoadActionsIntoDB<T>(this T clazz) where T : ActionMap {
            //T c = (T)Activator.CreateInstance(typeof(T), new object[] { }); ;
            DeleteStaleActions(clazz);
            foreach (MethodInfo mi in clazz.GetType().GetMethods()) {
                MethodInfo methodInfo = typeof(T).GetMethod(mi.Name);
                ActionMap.mi = methodInfo;
                if (GetTableRecord(methodInfo) != null)
                {
                    Command check = new Command(ConfigurationManager.AppSettings.Get("getActionByActionName"));
                    check.AddParameter("CSL", Instance.CSL);
                    check.AddParameter(ActionMap.ColumnName_ActionName, GetTableRecord(methodInfo)[ActionMap.ColumnName_ActionName]);
                    if (!DAO.QueryReturnsRows(check))
                    {
                        Command command = new Command(ConfigurationManager.AppSettings.Get("InstertAction"));
                        command.AddParameter("id", Instance.Current);
                        command.AddParameters(GetTableRecord(methodInfo));
                        DAO.ExecuteNonQuery(command);
                        command.Dispose();
                    }
                    else
                    {
                        Command command = new Command(ConfigurationManager.AppSettings.Get("UpdateAction"));
                        command.AddParameter("CSL", Instance.CSL);
                        command.AddParameters(GetTableRecord(methodInfo));
                        DAO.ExecuteNonQuery(command);
                        command.Dispose();
                    }
                    check.Dispose();
                }
            }
        }

        private static void DeleteStaleActions<T>(T clazz)
        {
            Command Actions = new Command(ConfigurationManager.AppSettings.Get("getActionInformation"));
            Actions.AddParameter("CSL", Instance.CSL);
            List<Dictionary<string,string>> ActionInfo = DAO.ExecuteQuery(Actions);
            foreach(Dictionary<string, string> action in ActionInfo)
            {
                bool actionInClass = false;
                foreach (MethodInfo mi in clazz.GetType().GetMethods())
                {
                    MethodInfo methodInfo = typeof(T).GetMethod(mi.Name);
                    ActionMap.mi = methodInfo;
                    if (GetTableRecord(methodInfo) != null)
                    {
                        if (action[ActionMap.ColumnName_ActionName].Equals(GetTableRecord(methodInfo)[ActionMap.ColumnName_ActionName]))
                        {
                            actionInClass = true;
                            break;
                        }
                    }
                }
                if (!actionInClass)
                {
                    Command deleteAction = new Command(ConfigurationManager.AppSettings.Get("DeleteAction"));
                    deleteAction.AddParameter("CSL", Instance.CSL);
                    deleteAction.AddParameter(ActionMap.ColumnName_ActionName, action[ActionMap.ColumnName_ActionName]);
                    DAO.ExecuteNonQuery(deleteAction);
                    deleteAction.Dispose();
                }
            }
        }

        private static Dictionary<string, string> GetTableRecord(MethodInfo methodInfo) {
            try
            {
                return ((ActionMap)Attribute.GetCustomAttribute(methodInfo, typeof(ActionMap))).TableRecord;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return null;
            }
        }
        
    }
}
