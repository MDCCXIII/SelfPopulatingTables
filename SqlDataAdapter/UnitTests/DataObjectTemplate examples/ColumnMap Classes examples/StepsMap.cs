using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTests.DataObjectTemplate.Data_Object;

namespace UnitTests.DataObjectTemplate.ColumnMap_Classes
{
    class StepsMap
    {

        public int TestCaseId;

        public int Id;

        public int Number;

        public bool NegativePath;

        public int PriorityId;

        public string Parameters;

        public string PriorityMessage;

        public StepMap StepInformation = new StepMap();

        public string KeywordName;

        public string KeywordScriptName;

        public string KeywordDescription;

        public string ActionName;

        public string ActionDescription;

        public string ElementName;

        public int ScreenId;

        public int ElementAccessibleTypeId;
        
        


    }
}
