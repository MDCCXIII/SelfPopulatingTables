using System;
using System.Collections.Generic;
using System.Configuration;

namespace SelfPopulatingTableFromAnnotation
{
    class Config
    {
        public static void CheckAppSettings()
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
    }
}
