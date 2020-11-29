using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using WhizzBase.Helpers;

namespace WhizzBase.Extensions
{
    public static class TypeExtensions
    {
        private static Dictionary<Type, string> _typesMap = new Dictionary<Type, string>
        {
            {typeof(bool), "bool"},
            {typeof(byte), "byte"},
            {typeof(sbyte), "sbyte"},
            {typeof(char), "char"},
            {typeof(decimal), "decimal"},
            {typeof(double), "double"},
            {typeof(float), "float"},
            {typeof(int), "int"},
            {typeof(uint), "uint"},
            {typeof(long), "long"},
            {typeof(ulong), "ulong"},
            {typeof(short), "short"},
            {typeof(ushort), "ushort"},
            {typeof(object), "object"},
            {typeof(string), "string"},
            {typeof(bool[]), "bool[]"},
            {typeof(byte[]), "byte[]"},
            {typeof(sbyte[]), "sbyte[]"},
            {typeof(char[]), "char[]"},
            {typeof(decimal[]), "decimal[]"},
            {typeof(double[]), "double[]"},
            {typeof(float[]), "float[]"},
            {typeof(int[]), "int[]"},
            {typeof(uint[]), "uint[]"},
            {typeof(long[]), "long[]"},
            {typeof(ulong[]), "ulong[]"},
            {typeof(short[]), "short[]"},
            {typeof(ushort[]), "ushort[]"},
            {typeof(object[]), "object[]"},
            {typeof(string[]), "string[]"},
        };
        
        public static string ToKeywordName(this Type type)
        {
            return _typesMap.ContainsKey(type) ? _typesMap[type] : type.Name;
        }
    }
}
