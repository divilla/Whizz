using WhizzJsonRepository.Base;

namespace WhizzJsonRepository.Validators
{
    public class GenericTypeJsonValidator : Validator
    {
        protected override void Validate()
        {
            foreach (var column in Columns)
            {
                if (!Database.TypeValidator.Validate(DbRequest[column.ColumnName], column.DataType, !column.IsRequired))
                {
                    Response.AddError(DbToJsonName[column.ColumnName], Database.ErrorMessages.InvalidDataFormat(column.DataType));
                }
            }
        }
    }
}