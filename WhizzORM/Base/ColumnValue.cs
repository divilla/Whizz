using System;
using System.Collections.Generic;
using System.Linq;
using WhizzORM.Entities;

namespace WhizzORM.Base
{
    public class ColumnValue
    {
        public ColumnEntity ColumnEntity { get; }
        public Type TargetType { get; }
        public object Input { get; private set; }
        public object Value { get; private set; }
        public bool HasValue { get; private set; }
        public bool HasErrors => _errors.Any();
        public IEnumerable<string> Errors => _errors.AsEnumerable();

        private readonly List<string> _errors = new List<string>();

        public ColumnValue(ColumnEntity columnEntity, Type type = null)
        {
            ColumnEntity = columnEntity;
            TargetType = type;
            HasValue = false;
        }

        public void SetValue(object value, string propertyName)
        {
            Input = value;

            if (value == null || value.GetType() == TargetType)
            {
                Value = value;
                HasValue = true;
                //if (ColumnEntity.IsRequired) _errors.Add("IsRequired");
                //return;
            }
            else
            {
                try
                {
                    Value = TargetType == null ? value : Convert.ChangeType(value, TargetType);
                    HasValue = true;
                }
                catch (InvalidCastException)
                {
                    throw new InvalidCastException($"Value '{value}' provided by property '{propertyName}'. '{value.GetType()}' was provided, while '{TargetType}' was expected.");
                }
            }
        }

        public void Validate()
        {
            //if (ColumnEntity.IsRequired && string.IsNullOrWhiteSpace(Value?.ToString()))
            //    _errors.Add("IsRequired");

            //if (ColumnEntity.CharacterMaximumLength != null && Value?.ToString().Length > ColumnEntity.CharacterMaximumLength)
            //    _errors.Add($"Too long, maximum allowed characters is '{ColumnEntity.CharacterMaximumLength}'");
        }
    }
}
