using System.Linq;
using Newtonsoft.Json.Linq;
using WhizzJsonRepository.Base;

namespace WhizzJsonRepository.Validators
{
    public class GenericMaxLengthJsonValidatorHandler : QueryJsonHandler
    {
        protected override QueryJsonHandler Handle()
        {
            foreach (var column in Columns.Where(q => q.CharacterMaximumLength != null && q.CharacterMaximumLength > 0))
            {
                if (State.Request[column.ColumnName].Type == JTokenType.Null)
                    continue;
                
                if (State.Request[column.ColumnName] is JValue 
                    && ((JValue) State.Request[column.ColumnName]).Value<string>().Length > column.CharacterMaximumLength)
                    State.AddError(column.ColumnName, Repository.PgValidationErrorMessages.TooLong(column.CharacterMaximumLength));
            }

            return this;
        }
    }
}