using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using Newtonsoft.Json.Linq;

namespace WhizzBase.Extensions
{
    public static class JObjectExtensions
    {
        public static JObject ResolvePropertyNames(this JObject jObject, Func<string, string> resolver)
        {
            var resolvedJObject = new JObject();
            foreach (var property in jObject.Properties())
            {
                resolvedJObject[resolver(property.Name)] = property.Value;
            }

            return resolvedJObject;
        }
    }
}