using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Reflection;

using ica.aps.orm.attributes;

namespace ica.aps.orm
{	
    public sealed class PropertyMappingInfo
    {
        #region Constructors

        internal PropertyMappingInfo()
            : this(null, null){}

        internal PropertyMappingInfo(string[] dataFieldNames, PropertyInfo info)
        {
            _dataFieldNames = dataFieldNames;            
            _propInfo = info;
        }

        #endregion

        #region Public Properties

        public string[] DataFieldNames
        {
            get 
            {
                if (_dataFieldNames == null || _dataFieldNames.Length < 1)
                    _dataFieldNames = new string[] {_propInfo.Name};
                return _dataFieldNames;
            }
        }
		
        public PropertyInfo PropertyInfo
        {
            get { return _propInfo; }
        }

        #endregion
		
        #region Private Variables
        private string[] _dataFieldNames;        
        private PropertyInfo _propInfo;
        #endregion
    }
}