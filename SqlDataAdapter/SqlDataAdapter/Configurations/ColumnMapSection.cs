using System.Configuration;

namespace SqlDataAdapter.Configurations
{
   
    public class ColumnMapSection : ConfigurationSection
    {
        /// <summary>
        /// 
        /// </summary>
        [ConfigurationProperty("columnMappings", IsDefaultCollection = true)]
        public NodeElement ColumnMap
        {
            get
            {
                return (NodeElement)this["columnMappings"];
            }
        }
    }


    [ConfigurationCollection(typeof(NodeElement), AddItemName = "add", CollectionType = ConfigurationElementCollectionType.AddRemoveClearMapAlternate)]
    public class NodeElement : ConfigurationElementCollection
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public new BaseElement this[string key]
        {
            get { return BaseGet(key) as BaseElement; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new BaseElement();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((BaseElement)element).ColumnName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        protected object GetElementValue(ConfigurationElement element)
        {
            return ((BaseElement)element).ParameterName;
        }
    }
    
    public class BaseElement : ConfigurationElement
    {
        /// <summary>
        /// 
        /// </summary>
        [ConfigurationProperty("columnName", IsKey=true, IsRequired=true)]
        public string ColumnName
        {
            get
            {
                return (string)this["columnName"];
            }
            set
            {
                this["columnName"] = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [ConfigurationProperty("parameterName", IsRequired=true)]
        public string ParameterName
        {
            get
            {
                return (string)this["parameterName"];
            }
            set
            {
                this["parameterName"] = value;
            }
        }
    }
}
