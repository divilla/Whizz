using System.Linq;
using System.Reflection;
using WhizzBase.Reflection;

namespace WhizzORM.Validators
{
    public abstract class BaseValidator<T>
        where T : class, new()
    {
        public BaseValidator()
        {
            _properties = ReflectionFactory.PropertiesInfo(typeof(T)).ToArray();
        }

        public bool Success { get; protected set; }

        protected readonly PropertyInfo[] _properties;
        public abstract bool Validate();
        public abstract string ErrorMessage { get; }
    }
}