using System;
using System.Reflection;

namespace WhizzORM.Reflection
{
    public class ObjectPropertyDelegates
    {
        public PropertyInfo PropertyInfo { get; }
        public Func<object, object> Getter { get; }
        public Action<object, object> Setter { get; }

        public ObjectPropertyDelegates(
            PropertyInfo propertyInfo,
            Func<object, object> getter,
            Action<object, object> setter)
        {
            PropertyInfo = propertyInfo;
            Getter = getter;
            Setter = setter;
        }
    }
}
