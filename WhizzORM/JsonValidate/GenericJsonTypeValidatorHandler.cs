using System;
using System.Linq;

namespace WhizzORM.JsonValidate
{
    public class GenericJsonTypeValidatorHandler : BaseJsonHandler
    {
        protected override BaseJsonHandler Handle()
        {
            foreach (var property in State.Request.Properties())
            {
                var column = State.Columns.SingleOrDefault(q => q.ColumnName == property.Name);

                var allowNull = State.MandatoryColumns switch
                {
                    MandatoryColumns.All => false,
                    MandatoryColumns.Required => !column.IsRequired,
                    MandatoryColumns.None => true,
                    _ => throw new ArgumentOutOfRangeException()
                };

                var valid = Repository.TypeValidator.Validate(property.Value, column.DataType, allowNull);
                
                if (!valid)
                    State.AddError(property.Name, ErrorMessages.InvalidDataFormat(column.DataType));

            }

            return this;
        }
    }
}