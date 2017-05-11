using System.Configuration;

namespace SqlDataAdapter
{
    class ColumnMapSection : ConfigurationSection
    {
        [ConfigurationProperty("columnMap")]
        public NodeElement ColumnMap
        {
            get { return (NodeElement)this["columnMap"]; }
        }
    }

    [ConfigurationCollection(typeof(BaseElement), AddItemName = "add")]
    public class NodeElement : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new BaseElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((BaseElement)element).ColumnName;
        }

        public BaseElement this[int index]
        {
            get { return this.BaseGet(index) as BaseElement; }
        }
    }

    public class BaseElement : ConfigurationElement
    {
        [ConfigurationProperty("ColumnName", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string ColumnName
        {
            get
            {
                return (string)this["ColumnName"];
            }
            set
            {
                this["ColumnName"] = value;
            }
        }

        [ConfigurationProperty("ParameterName", DefaultValue = "", IsRequired = true)]
        public string ParameterName
        {
            get
            {
                return (string)this["ParameterName"];
            }
            set
            {
                this["ParameterName"] = value;
            }
        }
    }
}
