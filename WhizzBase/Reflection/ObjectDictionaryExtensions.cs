using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WhizzBase.Base;
using WhizzBase.Enums;
using WhizzBase.Extensions;

namespace WhizzBase.Reflection
{
    public static class ObjectDictionaryExtensions
    {
        public static Dictionary<string, object> ToDictionary(this object source, Case toCase = Case.Default)
        {
            var dictionary = new Dictionary<string, object>();
            foreach (var propertyMapper in PropertyMappers(typeof(object)))
            {
                var name = propertyMapper.PropertyInfo.Name;
                switch (toCase)
                {
                    case Case.Snake:
                        name = name.ToSnakeCase();
                        break;
                    case Case.Camel:
                        name = name.ToCamelCase();
                        break;
                    case Case.Pascal:
                        name = name.ToPascalCase();
                        break;
                    case Case.Default:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(toCase), toCase, null);
                }
                
                dictionary[name] = propertyMapper.Getter(source);
            }

            return dictionary;
        }

        public static T ToInstance<T>(this Dictionary<string, object> source, Case fromCase = Case.Default)
            where T : class, new()
        {
            var instance = new T();
            foreach (var propertyMapper in PropertyMappers(typeof(object)))
            {
                var name = propertyMapper.PropertyInfo.Name;
                switch (fromCase)
                {
                    case Case.Snake:
                        name = name.ToSnakeCase();
                        break;
                    case Case.Camel:
                        name = name.ToCamelCase();
                        break;
                    case Case.Pascal:
                        name = name.ToPascalCase();
                        break;
                    case Case.Default:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(fromCase), fromCase, null);
                }
                
                propertyMapper.Setter(instance, source[name]);
            }

            return instance;
        }
        
        private static ConcurrentDictionary<Type, PropertyMapper[]> cache = new ConcurrentDictionary<Type, PropertyMapper[]>();

        private static IEnumerable<PropertyMapper> PropertyMappers(Type type)
        {
            if (!cache.ContainsKey(type))
            {
                cache[type] = type.GetProperties().Select(propertyInfo => new PropertyMapper(propertyInfo, DelegateGenerator.GetDelegate(propertyInfo), DelegateGenerator.SetDelegate(propertyInfo))).ToArray();
            }
            
            return cache[type];
        }
        
        private class PropertyMapper
        {
            public PropertyInfo PropertyInfo;
            public Func<object, object> Getter;
            public Action<object, object> Setter;

            public PropertyMapper(PropertyInfo propertyInfo, Func<object, object> getter, Action<object, object> setter)
            {
                PropertyInfo = propertyInfo;
                Getter = getter;
                Setter = setter;
            }
        }
    }
}