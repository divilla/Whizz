using System.Collections.Generic;

namespace WhizzBase.Extensions
{
    public static class DictionaryExtensions
    {
        public static string MatchKey(this Dictionary<string, object> dictionary, string value)
        {
            foreach (var key in new[] {value.ToPascalCase(), value.ToCamelCase(), value.ToSnakeCase()})
            {
                if (dictionary.ContainsKey(key)) 
                    return key;
            }

            return null;
        }
    }
}
