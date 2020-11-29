﻿using System;
 using System.Collections.Concurrent;
 using System.Collections.Generic;

 namespace WhizzORM.Reflection
{
    public class ObjectPropertyHelper : PropertyHelper
    {
        private static readonly ConcurrentDictionary<Type, ObjectPropertyHelper> ObjectPropertiesCache =
            new ConcurrentDictionary<Type, ObjectPropertyHelper>();

        private IEnumerable<ObjectPropertyDelegates> _propertyDelegates;

        public ObjectPropertyHelper(Type type) : base(type) {}

        public IEnumerable<ObjectPropertyDelegates> PropertyDelegates
        {
            get
            {
                if (_propertyDelegates == null)
                {
                    TryCreateObjectPropertyDelegates();
                }

                return _propertyDelegates;
            }
        }

        public static ObjectPropertyHelper GetProperties<TObject>()
            where TObject : class
            => GetProperties(typeof(TObject));

        public static ObjectPropertyHelper GetProperties(Type type)
            => ObjectPropertiesCache.GetOrAdd(type, _ => new ObjectPropertyHelper(type));

        private void TryCreateObjectPropertyDelegates()
        {
            var delegates = new List<ObjectPropertyDelegates>();

            foreach (var property in Properties)
            {
                if (property.GetMethod?.IsPublic != true || property.SetMethod?.IsPublic != true)
                {
                    continue;
                }

                var getter = MakeFastPropertyGetter<object>(property);
                var setter = MakeFastPropertySetter(property);

                delegates.Add(new ObjectPropertyDelegates(property, getter, setter));
            }

            _propertyDelegates = delegates;
        }
    }
}
