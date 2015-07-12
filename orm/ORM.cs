using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Reflection;

using ica.aps.orm.attributes;

namespace ica.aps.orm
{
	public delegate void FillChildObjects<T>(T o, IDataReader dr);

    public static class ORM
    {

        /// <summary>
        /// Creates and populates a collection with instances of the objType object passed in.
        /// </summary>
        /// <typeparam name="T">Type of object to add to the collection.</typeparam>
        /// <param name="objType">Type of object to add to the collection.</param>
        /// <param name="dr">IDataReader that contains the data for the collection.</param>
        /// <param name="collection">ICollection object to populate.</param>
        /// 
        public static C FillCollection<T, C>(IDataReader dr, FillChildObjects<T> helper = null)
            where T : class, new()
            where C : ICollection<T>, new()
        {
            C coll = new C();
			Type objType = typeof(T);
			
            List<PropertyMappingInfo> mapInfo = GetProperties(objType);

			while (dr.Read())
			{
                T obj = CreateObject<T>(dr, mapInfo, helper);
                coll.Add(obj);
            }
            return coll;
        }
		
		
        /// <summary>
        /// Creates and populates an instance of the objType Type.
        /// </summary>
        /// <typeparam name="T">Type of object to return.</typeparam>
        /// <param name="objType">Type of the object to instantiate.</param>
        /// <param name="dr">IDataReader with the data to populate the object instance with.</param>
        /// <returns>An instance of the objType type.</returns>
        public static T FillObject<T>(IDataReader dr, FillChildObjects<T> helper = null) where T : class, new()
        {
			Type objType = typeof(T);
			
            List<PropertyMappingInfo> mapInfo = GetProperties(objType);            
			//if (dr.Read())
			{
	            T obj = CreateObject<T>(dr, mapInfo, helper);
				
	            return obj;
			}

			//return default(T);
			
        }

        /// <summary>
        /// Iterates through the object type's properties and attempts to assign the value from the datareader to
        /// the matching property.
        /// </summary>
        /// <typeparam name="T">The type of object to populate.</typeparam>
        /// <param name="dr">IDataReader that contains the data to populate the object with.</param>
        /// <param name="propInfoList">List of PropertyMappingInfo objects.</param>
        /// <returns>A populated instance of type T</returns>
        public static T CreateObject<T>(IDataReader dr, List<PropertyMappingInfo> propInfoList = null, FillChildObjects<T> helper = null) where T : class, new()
        {
            T obj = new T();

			if (propInfoList == null)
			{
				Type objType = typeof(T);
				propInfoList = GetProperties(objType);
			}
			
            // iterate through the PropertyMappingInfo objects for this type.
			foreach (PropertyMappingInfo info in propInfoList)
			{
                if (info.PropertyInfo.CanWrite)
                {
                    Type type = info.PropertyInfo.PropertyType;
					object value = null;
					int ordinal = findOrdinal(dr, info);
					if (ordinal != -1 && !dr.IsDBNull(ordinal))
						value = dr.GetValue(ordinal);
                    if (value == null || value == DBNull.Value)
                        continue;

                    if (type == typeof(DateTime))
                    {
                        value = DateTimeToString(value);
                    }
                    else if (type == typeof(string))
                    {
                        value = value.ToString().Trim();
                    }

                    try
                    {
                        // try implicit conversion first
                        info.PropertyInfo.SetValue(obj, value, null);
                    }
                    catch(Exception ex)
                    {
                        System.Diagnostics.Trace.WriteLine(string.Format("Retry assigning [{0}] to {1}: {2}", value, dr.GetName(ordinal), ex.Message));                            
                        // data types do not match
                        try
                        {
                            // need to handle enumeration types differently than other base types.
                            if (type.BaseType.Equals(typeof(System.Enum)))
                            {
                                info.PropertyInfo.SetValue(
                                    obj, System.Enum.ToObject(type, value), null);
                            }
                            else
                            {
                                // try explicit conversion
                                info.PropertyInfo.SetValue(
                                    obj, Convert.ChangeType(value, type), null);
                            }
                        }
                        catch(Exception iex)
                        {
                            // error assigning the value to a property
                            System.Diagnostics.Trace.WriteLine(string.Format("Failed to assign [{0}] to {1}: {2}", value, dr.GetName(ordinal), iex.Message));
                        }
                    }
                } 
			}
			
			if (helper != null)
			{
				helper(obj, dr);
			}
			
            return obj;
        }
		        
        /// <summary>
        /// Loads the PropertyMappingInfo collection for the type specified by objType from the cache, or creates the
        /// collection and adds it to the cache if it does not exist.
        /// </summary>
        /// <param name="objType">Type to load the properties for.</param>
        /// <returns>A collection of PropertyMappingInfo objects that are associated with the Type.</returns>
        private static List<PropertyMappingInfo> LoadPropertyMappingInfo(Type objType)
        {
            List<PropertyMappingInfo> mapInfoList = new List<PropertyMappingInfo>();

            foreach (PropertyInfo info in objType.GetProperties())
            {						
                DataMappingAttribute[] mapAttrs = 
                    (DataMappingAttribute[])Attribute.GetCustomAttributes(info, typeof(DataMappingAttribute));

                if (mapAttrs != null)
                {
                    List<string> names = new List<string>();                    
                    foreach (DataMappingAttribute attr in mapAttrs)
                    {
                        if (!names.Contains(attr.DataFieldName))
                            names.Add(attr.DataFieldName);
                    }

                    PropertyMappingInfo mapInfo = new PropertyMappingInfo(names.ToArray(), info);
                    mapInfoList.Add(mapInfo);
                }
            }

            return mapInfoList;
        }

        /// <summary>
        /// Loads the PropertyMappingInfo collection for type specified.
        /// </summary>
        /// <param name="objType">Type that contains the properties to load.</param>
        /// <returns>A collection of PropertyMappingInfo objects that are associated with the Type.</returns>
        private static List<PropertyMappingInfo> GetProperties(Type objType)
        {
            List<PropertyMappingInfo> info = MappingInfoCache.GetCache(objType.Name);

            if (info == null)
            {
                info = LoadPropertyMappingInfo(objType);
                MappingInfoCache.SetCache(objType.Name, info);
            }
            return info;                       
        }

        /// <summary>
        /// Finds the ordinal for a field
        /// </summary>
        /// <returns></returns>
		private static int findOrdinal(IDataReader dr, PropertyMappingInfo info)
		{
            foreach (string dataFieldName in info.DataFieldNames)
            {
				try
				{
					int ordinal = dr.GetOrdinal(dataFieldName);
					if (ordinal != -1)
	                    return ordinal;
				} 
				catch (IndexOutOfRangeException)
				{// Field name does not exist in the datareader.
				}
            }
			return -1;
		}
		
		private static object DateTimeToString(object o)
		{
			if (o == null || string.IsNullOrEmpty(o.ToString()) || string.IsNullOrWhiteSpace(o.ToString()))
			{
				return o;
			}

            try
            {
                DateTime dt = DateTime.Parse(o.ToString());
                DateTime localtime = DateTime.SpecifyKind(dt, DateTimeKind.Local);

                string s = localtime.ToString("o");  // ex:  "2012-11-14T10:14:00.0000000-04:00"
                // remove milliseconds portion...
                int nDecimalSpot = s.IndexOf('.');
                int nDashSpot = s.LastIndexOf('-');
                if (nDecimalSpot != -1 && nDashSpot != -1)
                    s = s.Substring(0, nDecimalSpot) + s.Substring(nDashSpot);

                return s;
            }
            catch
            {
                return o;
            }
		}		
    }
}