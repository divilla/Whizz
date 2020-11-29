using System;
using System.Reflection;
using WhizzORM.Reflection;

namespace WhizzORM.Context
{
    internal class PropertyMap
    {
        internal string ColumnName { get; }
        internal PropertyInfo PropertyInfo { get; }
        internal Func<object, object> GetDelegate { get; }
        internal Action<object, object> SetDelegate { get; }

        public PropertyMap(string columnName, PropertyInfo propertyInfo)
        {
            ColumnName = columnName;
            PropertyInfo = propertyInfo;
            GetDelegate = PropertyDelegateMaker.GetDelegate(propertyInfo);
            SetDelegate = PropertyDelegateMaker.SetDelegate(propertyInfo);
        }
    }
}
