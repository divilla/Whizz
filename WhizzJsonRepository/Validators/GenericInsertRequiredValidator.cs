using System.Linq;
using Newtonsoft.Json.Linq;
using WhizzBase.Enums;
using WhizzJsonRepository.Base;

namespace WhizzJsonRepository.Validators
{
    public class GenericInsertRequiredValidator : Validator
    {
        protected override void Validate()
        {
            var columnNames = Columns.Where(q => q.IsRequired).Select(s => s.ColumnName);

            foreach (var columnName in columnNames)
            {
                if (!DbRequest.ContainsKey(columnName))
                {
                    Response.AddError(DbToJsonName[columnName], Database.ErrorMessages.MissingProperty);
                    Response.Continue = false;
                }
                else if (string.IsNullOrEmpty(DbRequest[columnName].Value<string>()))
                {
                    Response.AddError(DbToJsonName[columnName], Database.ErrorMessages.Required);
                }
            }
        }
    }
}