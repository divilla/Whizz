using System;
using System.Reflection;
using WhizzBase.Reflection;

namespace WhizzBase.Helpers
{
    public class ColumnPropertyMapper
    {
        public ColumnPropertyMapper(string columnName, PropertyInfo propertyInfo)
        {
            ColumnName = columnName;
            PropertyInfo = propertyInfo;
            Name = propertyInfo.Name;
            Type = propertyInfo.PropertyType;
            _getMethod = DelegateGenerator.GetDelegate(propertyInfo);
            _setMethod = DelegateGenerator.SetDelegate(propertyInfo);
        }

        public string ColumnName { get; } 
        public PropertyInfo PropertyInfo { get; } 
        public string Name { get; } 
        public Type Type { get; }
        
        private Func<object, object> _getMethod;
        private Action<object, object> _setMethod;

        public object Get(object declaringInstance)
        {
            return _getMethod(declaringInstance);
        }

        public void Set(object declaringInstance, object value)
        {
            if (value == null || value is DBNull) return;
            
            _setMethod(declaringInstance, value);
        }
    }
}