// ReSharper disable ClassWithVirtualMembersNeverInherited.Global
namespace WhizzJsonRepository
{
    public class ValidationErrorMessages
    {
        public virtual string InvalidProperties(string names) => $"Bad request. Invalid properties: {names}";
        public virtual string MissingProperty => "Mandatory property is undefined";
        public virtual string ReadonlyProperty => "Property is readonly";
        public virtual string Required => "Required";
        public virtual string InvalidDataFormat(string pgsqlType) => $"Invalid data format, must be convertible to '{pgsqlType}'.";
        public virtual string TextTooLong(int? maxLength) => $"Too long text, maximum allowed characters '{maxLength}'.";
    }
}