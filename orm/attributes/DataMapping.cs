using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ica.aps.orm.attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public sealed class DataMappingAttribute : System.Attribute
    {
        #region Constructors
        public DataMappingAttribute(string dataFieldName)
            : base()
        {
            DataFieldName = dataFieldName;
        }
        #endregion

        #region Public Properties
        public string DataFieldName
        {
            get;
            private set;
        }
        #endregion
    }
}
	