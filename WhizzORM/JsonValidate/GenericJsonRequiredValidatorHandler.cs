using System.Linq;
using Newtonsoft.Json.Linq;
using WhizzSchema.Entities;

namespace WhizzORM.JsonValidate
{
    public class GenericJsonRequiredValidatorHandler : BaseJsonHandler
    {
        protected override BaseJsonHandler Handle()
        {
            ColumnEntity[] columns;
            switch (State.MandatoryColumns)
            {
                case MandatoryColumns.All:
                    columns = State.Columns.ToArray();
                    break;
                case MandatoryColumns.Required:
                    columns = State.Columns.Where(q => q.IsRequired).ToArray();
                    break;
                default:
                    columns = new ColumnEntity[0];
                    break;
            }
            
            var propertyNames = State.OriginalRequest.Properties().Select(s => s.Name).ToArray();
            foreach (var column in columns)
            {
                if (!propertyNames.Contains(column.ColumnName))
                {
                    State.AddError(column.ColumnName, ErrorMessages.MissingProperty);
                    State.Continue = false;
                }
                else if (string.IsNullOrWhiteSpace(State.OriginalRequest[column.ColumnName].Value<string>()))
                {
                    State.AddError(column.ColumnName, ErrorMessages.Required);
                }
            }

            return this;
        }
    }
}