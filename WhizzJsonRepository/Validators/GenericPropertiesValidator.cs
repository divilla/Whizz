using System.Collections.Generic;
using WhizzJsonRepository.Base;

namespace WhizzJsonRepository.Validators
{
    public class GenericPropertiesValidator : Validator
    {
        protected override void Validate()
        {
            var invalidProperties = new List<string>();
            var columnNames = ColumnNames;
            foreach (var property in JsonRequest.Properties())
            {
                var propertyName = Database.ToDbCase(property.Name);
                if (columnNames.Contains(propertyName))
                {
                    DbRequest[propertyName] = property.Value;
                    DbToJsonName[propertyName] = propertyName;
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