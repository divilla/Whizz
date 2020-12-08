using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using WhizzBase.Base;
using WhizzBase.Reflection;
using WhizzORM.Interfaces;

namespace WhizzORM.Validators
{
    public class ValidationResult<T>
        where T : class, new()
    {
        public ValidationResult()
        {
            _properties = ReflectionFactory.PropertiesInfo(typeof(T)).ToArray();
        }

        public bool Success { get; private set; } = true;
        public IReadOnlyList<string> ErrorMessages => _errorMessages;
        public ImmutableDictionary<string, ImmutableList<string>> Errors 
            => _errors.ToDictionary(k => k.Key, v => v.Value.ToImmutableList())
                .ToImmutableDictionary();

        private readonly List<string> _errorMessages = new List<string>();
        private readonly Dictionary<string, List<string>> _errors = new Dictionary<string, List<string>>();
        private readonly PropertyInfo[] _properties;

        public void AddErrorMessage(string message)
        {
            _errorMessages.Add(message);
            Success = false;
        }

        public void AddError(string propertyName, string message)
        {
            if (_properties.All(q => q.Name != propertyName))
                throw new DbArgumentException($"'{typeof(T)}' does not contain property with name '{propertyName}'", nameof(message));
            
            if (string.IsNullOrWhiteSpace(message))
                throw new DbArgumentException("Message must not be empty", nameof(message));
            
            if (!_errors.ContainsKey(propertyName))
                _errors[propertyName] = new List<string>();
            
            _errors[propertyName].Add(message);
            Success = false;
        }
    }
}