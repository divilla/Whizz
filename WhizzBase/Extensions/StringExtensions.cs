using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace WhizzBase.Extensions
{
    public static class StringExtensions
    {
        public static string ToCamelCase(this string str)
        {
            var pattern = new Regex(@"[A-Z]{2,}(?=[A-Z][a-z]+[0-9]*|\b)|[A-Z]?[a-z]+[0-9]*|[A-Z]|[0-9]+");
            var values = (from Match match in pattern.Matches(str) select match.Value).ToList();
            
            return new string(
                new CultureInfo("en-US", false)
                    .TextInfo
                    .ToTitleCase(
                        string.Join(" ", values).ToLower()
                    )
                    .Replace(@" ", "")
                    .Select((x, i) => i == 0 ? char.ToLower(x) : x)
                    .ToArray()
            );
        }

        public static string ToUnderscoredCamelCase(this string value)
        {
            return new string("_" + value.ToCamelCase());
        }
        
        public static string ToPascalCase(this string str)
        {
            var pattern = new Regex(@"[A-Z]{2,}(?=[A-Z][a-z]+[0-9]*|\b)|[A-Z]?[a-z]+[0-9]*|[A-Z]|[0-9]+");
            return new string(
                new CultureInfo("en-US", false)
                    .TextInfo
                    .ToTitleCase(
                        string.Join(" ", pattern.Matches(str)).ToLower()
                    )
                    .Replace(@" ", "")
                    .ToArray()
            );
        }

        public static string ToSnakeCase(this string str)
        {
            var pattern = new Regex(@"[A-Z]{2,}(?=[A-Z][a-z]+[0-9]*|\b)|[A-Z]?[a-z]+[0-9]*|[A-Z]|[0-9]+");
            var values = (from Match match in pattern.Matches(str) select match.Value).ToList();
            return string.Join("_", values).ToLower();
        }

        public static string ToKebabCase(this string str)
        {
            var pattern = new Regex(@"[A-Z]{2,}(?=[A-Z][a-z]+[0-9]*|\b)|[A-Z]?[a-z]+[0-9]*|[A-Z]|[0-9]+");
            var values = (from Match match in pattern.Matches(str) select match.Value).ToList();
            return string.Join("-", values).ToLower();
        }

        public static string AddArrayDimension(this string value, int dimension)
        {
            for (var i = 1; i <= dimension; i++)
            {
                value += "[]";
            }

            return value;
        }
    }
}
