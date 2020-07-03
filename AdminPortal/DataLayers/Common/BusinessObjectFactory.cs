using Spring.Context.Support;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdminPortal.DataLayers.Common
{
    public class BusinessObjectFactory<T>
    {
        public static T GetBusinessObject()
        {
            var listItem = ContextRegistry.GetContext().GetObjectsOfType(typeof(T)).Values;
            return listItem.Cast<T>().FirstOrDefault();
        }

        public static T GetBusinessObject(string objectName)
        {
            return (T)ContextRegistry.GetContext().GetObject(objectName);
        }
    }
}
