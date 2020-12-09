using Newtonsoft.Json.Linq;

namespace WhizzORM.Interfaces
{
    public interface IJsonTypeValidator
    {
        bool Validate(JToken jToken, string pgsqlType, bool allowNull);
    }
}