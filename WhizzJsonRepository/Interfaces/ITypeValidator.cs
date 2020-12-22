using Newtonsoft.Json.Linq;

namespace WhizzORM.Interfaces
{
    public interface ITypeValidator
    {
        bool Validate(JToken jToken, string pgsqlType, bool allowNull);
    }
}