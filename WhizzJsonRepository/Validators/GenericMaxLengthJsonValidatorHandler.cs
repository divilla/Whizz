using System.Linq;
using Newtonsoft.Json.Linq;
using WhizzJsonRepository.Base;

namespace WhizzJsonRepository.Validators
{
    public class GenericMaxLengthJsonValidatorHandler : Validator
    {
        protected override void Validate()
        {
            var columns = Columns.Where(q => q.CharacterMaximumLength > 0);

            foreach (var column in columns)
            {
                if (DbRequest[column.ColumnName].Value<string>().Length > column.CharacterMaximumLength)
                {
                    Response.AddError(DbToJsonName[column.ColumnName], Database.ErrorMessages.TextTooLong(column.CharacterMaximumLength));
                }
            }
        }
    }
}