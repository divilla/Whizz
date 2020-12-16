using System;
using System.Linq;
using WhizzBase.Enums;
using WhizzJsonRepository.Base;

namespace WhizzJsonRepository.Validators
{
    public class GenericTypeJsonValidatorHandler : QueryJsonHandler
    {
        protected override QueryJsonHandler Handle()
        {
            foreach (var property in State.Request.Properties())
            {
                var column = Columns.SingleOrDefault(q => q.ColumnName == property.Name);

                var allowNull = MandatoryColumns switch
                {
                    MandatoryColumns.All => false,
                    MandatoryColumns.Required => !column.IsRequired,
                    _ => true,
                };

                var valid = Repository.TypeValidator.Validate(property.Value, column.DataType, allowNull);
                
                if (!valid)
                    State.AddError(property.Name, Repository.PgValidationErrorMessages.InvalidDataFormat(column.DataType));

            }

            return this;
        }
    }
}