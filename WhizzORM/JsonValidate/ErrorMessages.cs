namespace WhizzORM.JsonValidate
{
    public static class ErrorMessages
    {
        public static string MissingProperty => "Required property is undefined";
        public static string Required => "Required";
        public static string InvalidDataFormat(string pgsqlType) => $"Invalid data format, must be convertible to '{pgsqlType}'.";
        public static string TooLong(int? maxLength) => $"Too long, maximum allowed characters '{maxLength}'.";
    }
}