using System;
using System.Reflection;
using WhizzBase.Attributes;

namespace WhizzBase.Helpers
{
    public static class AttributeHelper
    {
        public static TAttribute GetAttribute<TAttribute>(Type type)
            where TAttribute : Attribute
        {
            var attribute = Attribute.GetCustomAttribute(type, typeof(TAttribute));
            return (TAttribute) attribute;
        }

        public static TAttribute GetAttribute<TAttribute>(PropertyInfo property)
            where TAttribute : Attribute
        {
            var attribute = property.GetCustomAttribute(typeof(TAttribute));
            return (TAttribute) attribute;
        }

        public static string GetRelationDbName(Type type)
        {
            var tableAttribute = GetAttribute<TableAttribute>(type);
            if (tableAttribute != null) return tableAttribute.Name;

            var viewAttribute = GetAttribute<ViewAttribute>(type);
            if (viewAttribute != null) return viewAttribute.Name;

            return null;
        }
    }
}