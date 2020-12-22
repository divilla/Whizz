using System.Linq;
using Newtonsoft.Json.Linq;
using WhizzBase.Enums;
using WhizzJsonRepository.Base;

namespace WhizzJsonRepository.Validators
{
    public class GenericUpdateRequiredValidator : Validator
    {
        protected override void Validate()
        {
            var columnNames = Columns.Where(q => q.IsRequired).Select(s => s.ColumnName);

            foreach (var property in DbRequest.Properties())
            {
                if (columnNames.Contains(property.Name) && string.IsNullOrWhiteSpace(property.Value<string>()))
                {
                    Response.AddError(DbToJsonName[property.Name], Database.ErrorMessages.Required);
                }
            }
        }
    }
}