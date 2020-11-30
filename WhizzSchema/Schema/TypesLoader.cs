using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using Npgsql;
using NpgsqlTypes;
using WhizzBase.Helpers;
using WhizzSchema.Entities;

namespace WhizzSchema.Schema
{
    public static class TypesLoader
    {
        public static Dictionary<uint, TypeEntity> Load(NpgsqlConnection connection)
        {
            var dataTable = connection.GetSchema("DataTypes");
            var mapper = EntityMapper.GetTableMapper<TypeEntity>(dataTable);
            var dataReader = dataTable.CreateDataReader();
            var types = new Dictionary<uint, TypeEntity>();
            var systemCollectionsAssembly = typeof(System.Collections.BitArray).Assembly;
            var systemNetAssembly = typeof(IPAddress).Assembly;
            var systemNetNetworkInformationAssembly = typeof(PhysicalAddress).Assembly;
            var npgsqlAssembly = typeof(NpgsqlBox).Assembly;
            var newtonsoftJsonLinqAssembly = typeof(JObject).Assembly;
            while (dataReader.Read())
            {
                var type  = new TypeEntity();
                foreach (var columnName in mapper.ColumnMaps.Keys)
                {
                    var value = dataReader[columnName];
                    if (value is DBNull) continue;
                    mapper.ColumnMaps[columnName].Set(type, value);
                }

                if ((type.TypeName == "OID" || type.TypeName == "xid" || type.TypeName == "cid" || type.TypeName == "tid") && string.IsNullOrEmpty(type.DataType))
                {
                    type.DataType = "System.UInt32";
                    type.IsUnsigned = true;
                }

                if (type.DataType == "String")
                    type.DataType = "System.String";
                else if (type.DataType == "System.Text.Json.JsonDocument")
                    type.DataType = "Newtonsoft.Json.Linq.JObject";
                else if (type.DataType == "System.Text.Json.JsonDocument[]")
                    type.DataType = "Newtonsoft.Json.Linq.JArray";

                if (!string.IsNullOrEmpty(type.DataType))
                {
                    type.Type = Type.GetType(type.DataType);
                    if (type.Type == null)
                    {
                        if (type.DataType.Contains("NpgsqlRange"))
                        {
                            var match = Regex.Match(type.DataType, @"\[[^\.]+\.([A-Za-z0-9]+)");
                            type.DataType = $"NpgsqlRange<{match.Groups[1].Value}>";
                            type.Type = npgsqlAssembly.GetType(type.DataType);
                            type.Using = "NpgsqlTypes";
                        }
                        else if (type.DataType.StartsWith("Npgsql"))
                        {
                            type.Type = npgsqlAssembly.GetType(type.DataType);
                            type.Using = "NpgsqlTypes";
                        }
                        else if (type.DataType.StartsWith("System.Collections"))
                        {
                            type.Type = systemCollectionsAssembly.GetType(type.DataType);
                            type.Using = "System.Collections";
                        }
                        else if (type.DataType.StartsWith("System.Net.NetworkInformation"))
                        {
                            type.Type = systemNetNetworkInformationAssembly.GetType(type.DataType);
                            type.Using = "System.Net.NetworkInformation";
                        }
                        else if (type.DataType.StartsWith("System.Net"))
                        {
                            type.Type = systemNetAssembly.GetType(type.DataType);
                            type.Using = "System.Net";
                        }
                        else if (type.DataType.StartsWith("Newtonsoft.Json.Linq"))
                        {
                            type.Type = newtonsoftJsonLinqAssembly.GetType(type.DataType);
                            type.Using = "Newtonsoft.Json.Linq";
                        }
                    }
                }

                types[type.OID] = type;
            }

            return types;
        }
    }
}