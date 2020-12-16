using System.Linq;
using Newtonsoft.Json.Linq;
using WhizzBase.Enums;
using WhizzJsonRepository.Base;

namespace WhizzJsonRepository.Validators
{
    public class GenericRequiredJsonValidatorHandler : QueryJsonHandler
    {
        protected override QueryJsonHandler Handle()
        {
            var columnNames = MandatoryColumns switch
            {
                MandatoryColumns.All => Columns.Select(s => s.ColumnName).ToArray(),
                MandatoryColumns.Required => Columns.Where(q => q.IsRequired).Select(s => s.ColumnName).ToArray(),
                _ => new string[0]
            };

            var propertyNames = State.Request.Properties().Select(s => s.Name).ToArray();
            foreach (var columnName in columnNames)
            {
                if (!propertyNames.Contains(columnName))
                {
                    State.AddError(columnName, Repository.PgValidationErrorMessages.MissingProperty);
                    State.Continue = false;
                }
                else if (string.IsNullOrWhiteSpace(State.Request[columnName].Value<string>()))
                {
                    State.AddError(columnName, Repository.PgValidationErrorMessages.Required);
                }
            }

            return this;
        }
    }
}