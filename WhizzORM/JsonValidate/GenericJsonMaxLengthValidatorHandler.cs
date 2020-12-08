using System.Linq;
using Newtonsoft.Json.Linq;

namespace WhizzORM.JsonValidate
{
    public class GenericJsonMaxLengthValidatorHandler : BaseJsonHandler
    {
        protected override BaseJsonHandler Handle()
        {
            foreach (var column in State.Columns.Where(q => q.CharacterMaximumLength != null && q.CharacterMaximumLength > 0))
            {
                if (State.Request[column.ColumnName].Type == JTokenType.Null)
                    continue;
                
                if (State.Request[column.ColumnName] is JValue 
                    && ((JValue) State.Request[column.ColumnName]).Value<string>().Length > column.CharacterMaximumLength)
                    State.AddError(column.ColumnName, ErrorMessages.TooLong(column.CharacterMaximumLength));
            }

            return this;
        }
    }
}