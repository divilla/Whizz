using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WhizzBase.Base;
using WhizzBase.Extensions;

namespace WhizzBase.Reflection
{
    public static class ReflectionFactory
    {
        public static IEnumerable<PropertyInfo> PropertiesInfo(Type type)
        {
            if (PropertiesInfoCache.ContainsKey(type)) return PropertiesInfoCache[type];

            PropertiesInfoCache[type] = type.GetProperties().ToArray();

            return PropertiesInfoCache[type];
        }

        public static ConcurrentDictionary<PropertyInfo, Func<object, object>> GetterDelegatesByPropertyInfo(Type type)
        {
            if (!GetterDelegatesByPropertyInfoCache.ContainsKey(type))
                GetterDelegatesByPropertyInfoCache[type] = new ConcurrentDictionary<PropertyInfo, Func<object, object>>();

            foreach (var propertyInfo in PropertiesInfo(type))
            {
                GetterDelegatesByPropertyInfoCache[type][propertyInfo] = DelegateGenerator.GetDelegate(propertyInfo);
            }

            return GetterDelegatesByPropertyInfoCache[type];
        }

        public static ConcurrentDictionary<PropertyInfo, Action<object, object>> SetterDelegatesByPropertyInfo(Type type)
        {
            if (!SetterDelegatesByPropertyInfoCache.ContainsKey(type))
                SetterDelegatesByPropertyInfoCache[type] = new ConcurrentDictionary<PropertyInfo, Action<object, object>>();

            foreach (var propertyInfo in PropertiesInfo(type))
            {
                SetterDelegatesByPropertyInfoCache[type][propertyInfo] = DelegateGenerator.SetDelegate(propertyInfo);
            }

            return SetterDelegatesByPropertyInfoCache[type];
        }

        public static ConcurrentDictionary<string, Func<object, object>> GetterDelegatesByPropertyName(Type type, Case toCase = Case.Default)
        {
            if (!GetterDelegatesByPropertyNameCache.ContainsKey(type))
                GetterDelegatesByPropertyNameCache[type] = new ConcurrentDictionary<Case, ConcurrentDictionary<string, Func<object, object>>>();
            
            if (!GetterDelegatesByPropertyNameCache[type].ContainsKey(toCase))
            {
                GetterDelegatesByPropertyNameCache[type][toCase] = new ConcurrentDictionary<string, Func<object, object>>();
            
                foreach (var propertyInfo in PropertiesInfo(type))
                {
                    var name = propertyInfo.Name;
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

                    GetterDelegatesByPropertyNameCache[type][toCase][name] = DelegateGenerator.GetDelegate(propertyInfo);
                }
            }

            return GetterDelegatesByPropertyNameCache[type][toCase];
        }
        
        public static ConcurrentDictionary<string, Action<object, object>> SetterDelegatesByPropertyName(Type type, Case toCase = Case.Default)
        {
            if (!SetterDelegatesByPropertyNameCache.ContainsKey(type))
                SetterDelegatesByPropertyNameCache[type] = new ConcurrentDictionary<Case, ConcurrentDictionary<string, Action<object, object>>>();
            
            if (!SetterDelegatesByPropertyNameCache[type].ContainsKey(toCase))
            {
                SetterDelegatesByPropertyNameCache[type][toCase] = new ConcurrentDictionary<string, Action<object, object>>();
            
                foreach (var propertyInfo in PropertiesInfo(type))
                {
                    var name = propertyInfo.Name;
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

                    SetterDelegatesByPropertyNameCache[type][toCase][name] = DelegateGenerator.SetDelegate(propertyInfo);
                }
            }

            return SetterDelegatesByPropertyNameCache[type][toCase];
        }
        
        private static readonly ConcurrentDictionary<Type, PropertyInfo[]> PropertiesInfoCache = new ConcurrentDictionary<Type, PropertyInfo[]>();
        
        private static readonly ConcurrentDictionary<Type, ConcurrentDictionary<PropertyInfo, Func<object, object>>> GetterDelegatesByPropertyInfoCache = new ConcurrentDictionary<Type, ConcurrentDictionary<PropertyInfo, Func<object, object>>>();
        
        private static readonly ConcurrentDictionary<Type, ConcurrentDictionary<PropertyInfo, Action<object, object>>> SetterDelegatesByPropertyInfoCache = new ConcurrentDictionary<Type, ConcurrentDictionary<PropertyInfo, Action<object, object>>>();
        
        private static readonly ConcurrentDictionary<Type, ConcurrentDictionary<Case, ConcurrentDictionary<string, Func<object,object>>>> GetterDelegatesByPropertyNameCache 
            = new ConcurrentDictionary<Type, ConcurrentDictionary<Case, ConcurrentDictionary<string, Func<object, object>>>>();
        
        private static readonly ConcurrentDictionary<Type, ConcurrentDictionary<Case, ConcurrentDictionary<string, Action<object,object>>>> SetterDelegatesByPropertyNameCache 
            = new ConcurrentDictionary<Type, ConcurrentDictionary<Case, ConcurrentDictionary<string, Action<object, object>>>>();
    }
}