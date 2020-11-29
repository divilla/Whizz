using System;
using System.Collections.Generic;

namespace WhizzBase.Helpers
{
    public class TypeHelper
    {
        public static List<Type> FindNullableTypes(IEnumerable<Type> types)
        {
            var typesToAdd = new List<Type>();

            foreach (var type in types)
            {
                var nullable = GetNullableType(type);
                if (nullable != null && nullable != type)
                    typesToAdd.Add(nullable);
            }

            return typesToAdd;
        }
        
        public static Type GetNullableType(Type type)
        {
            // Abort if no type supplied
            if (type == null)
                return null;

            // If the given type is already nullable, just return it
            if (IsTypeNullable(type))
                return type;

            // If the type is a ValueType and is not System.Void, convert it to a Nullable<Type>
            if (type.IsValueType && type != typeof(void))
                return typeof(Nullable<>).MakeGenericType(type);

            // Done - no conversion
            return null;
        }
        
        public static bool IsTypeNullable(Type type)
        {
            // Abort if no type supplied
            if (type == null)
                return false;

            // If this is not a value type, it is a reference type, so it is automatically nullable
            //  (NOTE: All forms of Nullable<T> are value types)
            if (!type.IsValueType)
                return true;

            // Report whether TypeToTest is a form of the Nullable<> type
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }
    }
}