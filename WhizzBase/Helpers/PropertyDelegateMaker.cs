using System;
using System.Linq.Expressions;
using System.Reflection;

namespace WhizzBase.Helpers
{
    public static class PropertyDelegateMaker
    {
        public static Func<object, object> GetDelegate(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
                throw new ArgumentException("PropertyInfo is null.", nameof(propertyInfo));

            var instance = Expression.Parameter(typeof(object), "instance");

            if (propertyInfo.DeclaringType == null)
                throw new ArgumentException($"DeclaringType is not accessible for property '{propertyInfo.Name}'.", nameof(propertyInfo));
            var instanceCast = (!propertyInfo.DeclaringType.IsValueType) 
                ? Expression.TypeAs(instance, propertyInfo.DeclaringType) 
                : Expression.Convert(instance, propertyInfo.DeclaringType);

            if (propertyInfo.GetGetMethod() == null)
                throw new ArgumentException($"GetGetMethod() is not accessible for property '{propertyInfo.Name}'.", nameof(propertyInfo));
            
            return Expression.Lambda<Func<object, object>>(
                Expression.TypeAs(Expression.Call(instanceCast, propertyInfo.GetGetMethod()),
                    typeof(object)), instance).Compile();
        }

        public static Action<object, object> SetDelegate(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
                throw new ArgumentException("PropertyInfo is null.", nameof(propertyInfo));

            var instance = Expression.Parameter(typeof(object), "instance");
            var value = Expression.Parameter(typeof(object), "value");

            if (propertyInfo.DeclaringType == null)
                throw new ArgumentException($"DeclaringType is not accessible for property '{propertyInfo.Name}'.", nameof(propertyInfo));
            var instanceCast = (!propertyInfo.DeclaringType.IsValueType) 
                ? Expression.TypeAs(instance, propertyInfo.DeclaringType) 
                : Expression.Convert(instance, propertyInfo.DeclaringType);

            var valueCast = (!propertyInfo.PropertyType.IsValueType) ? Expression.TypeAs(value, propertyInfo.PropertyType) : Expression.Convert(value, propertyInfo.PropertyType);

            if (propertyInfo.GetSetMethod() == null)
                throw new ArgumentException($"GetSetMethod() is not accessible for property '{propertyInfo.Name}'.", nameof(propertyInfo));

            return Expression.Lambda<Action<object, object>>(
                Expression.Call(instanceCast, propertyInfo.GetSetMethod(), valueCast), 
                instance, value).Compile();
        }
    }
}