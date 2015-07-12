using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace ica.aps.orm
{	
    internal static class MappingInfoCache
    {
        private static Dictionary<string, List<PropertyMappingInfo>> cache = 
            new Dictionary<string,List<PropertyMappingInfo>>();

        internal static List<PropertyMappingInfo> GetCache(string typeName)
        {
            List<PropertyMappingInfo> info = null;
            try
            {
                info = (List<PropertyMappingInfo>) cache[typeName];
                
            }
            catch(KeyNotFoundException){}

            return info;
        }

        internal static void SetCache(string typeName, List<PropertyMappingInfo> mappingInfoList)
        {
            cache[typeName] = mappingInfoList;
        }

        public static void ClearCache()
        {
            cache.Clear();
        }
    }
}