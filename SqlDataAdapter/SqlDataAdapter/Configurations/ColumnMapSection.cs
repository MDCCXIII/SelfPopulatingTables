﻿using System.Configuration;

namespace SqlDataAdapter.Configurations
{
    /// <summary>
    /// Describes a custom configuration section
    /// </summary>
    public class ColumnMapSection : ConfigurationSection
    {
       
        [ConfigurationProperty("columnMappings", IsDefaultCollection = true)]
        public NodeElement ColumnMap
        {
            get
            {
                return (NodeElement)this["columnMappings"];
            }
        }
    }

    /// <summary>
    /// Describes a custom configuration element collection
    /// </summary>
    [ConfigurationCollection(typeof(NodeElement), AddItemName = "add", CollectionType = ConfigurationElementCollectionType.AddRemoveClearMapAlternate)]
    public class NodeElement : ConfigurationElementCollection
    {
        public new BaseElement this[string key]
        {
            get { return BaseGet(key) as BaseElement; }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new BaseElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((BaseElement)element).ColumnName;
        }

        protected object GetElementValue(ConfigurationElement element)
        {
            return ((BaseElement)element).ParameterName;
        }
    }

    /// <summary>
    /// Describes the elements of a custom configuration element collection 
    /// </summary>
    public class BaseElement : ConfigurationElement
    {
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
