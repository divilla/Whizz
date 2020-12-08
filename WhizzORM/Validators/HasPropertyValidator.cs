using System.Linq;

namespace WhizzORM.Validators
{
    public class HasPropertyValidator<T> : BaseValidator<T>
        where T : class, new()
    {
        public HasPropertyValidator(string propertyName)
        {
            _propertyName = propertyName;
        }

        public override string ErrorMessage => $"'{typeof(T)} must implement property {_propertyName}'";

        protected string _propertyName;

        public override bool Validate()
        {
            Success = _properties.Any(q => q.Name == _propertyName);
            return Success;
        }
    }
}
