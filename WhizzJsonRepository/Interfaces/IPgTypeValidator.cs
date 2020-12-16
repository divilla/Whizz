using Newtonsoft.Json.Linq;

namespace WhizzORM.Interfaces
{
    public interface IPgTypeValidator
    {
        bool Validate(JToken jToken, string pgsqlType, bool allowNull);
    }
}