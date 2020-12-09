namespace WhizzJsonRepository.JsonValidate
{
    public class ErrorMessages
    {
        public virtual string MissingProperty => "Required property is undefined";
        public virtual string Required => "Required";
        public virtual string InvalidDataFormat(string pgsqlType) => $"Invalid data format, must be convertible to '{pgsqlType}'.";
        public virtual string TooLong(int? maxLength) => $"Too long, maximum allowed characters '{maxLength}'.";
    }
}