using System.Collections.Generic;
using System.Linq;
using WhizzJsonRepository.Base;

namespace WhizzJsonRepository.Validators
{
    public class GenericInsertPropertiesValidator : Validator
    {
        protected override void Validate()
        {
            var invalidProperties = new List<string>();
            var columnNames = ColumnNames;
            foreach (var property in DbRequest.Properties())
            {
                var propertyName = Database.ToDbCase(property.Name);
                if (columnNames.Contains(propertyName))
                {
                    if (!Columns.Any(q => q.IsReadonly && q.ColumnName == propertyName))
                    {
                        DbRequest[propertyName] = property.Value;
                        DbToJsonName[propertyName] = propertyName;
                    }
                    else
                    {
                        Response.AddError(DbToJsonName[property.Name], Database.ErrorMessages.ReadonlyProperty);
                    }
                }
                else
                {
                    invalidProperties.Add(property.Name);
                }
            }

            if (invalidProperties.Count == 0) return;
            
            Response.SetBadRequestError(Database.ErrorMessages.InvalidProperties(string.Join(", ", invalidProperties)));
            Response.Continue = false;
        }
    }
}