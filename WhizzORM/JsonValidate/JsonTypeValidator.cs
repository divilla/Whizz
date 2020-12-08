using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using WhizzBase.Extensions;
using WhizzORM.Interfaces;

namespace WhizzORM.JsonValidate
{
    public class JsonTypeValidator : IJsonTypeValidator
    {
        public JsonTypeValidator()
        {
            _validators["smallint"] = jToken => 
                jToken.Type == JTokenType.Integer 
                && jToken.Value<long>() >= short.MinValue 
                && jToken.Value<long>() <= short.MaxValue;
            
            _validators["integer"] = jToken => 
                jToken.Type == JTokenType.Integer 
                && jToken.Value<long>() >= int.MinValue 
                && jToken.Value<long>() <= int.MaxValue;
            
            _validators["bigint"] = jToken => 
                jToken.Type == JTokenType.Integer;
            
            _validators["decimal"] = jToken => 
                jToken.Type == JTokenType.Float;
            
            _validators["numeric"] = jToken => 
                jToken.Type == JTokenType.Float;
            
            _validators["money"] = jToken => 
                jToken.Type == JTokenType.Float;
            
            _validators["real"] = jToken => 
                jToken.Type == JTokenType.Float 
                && jToken.Value<double>() >= float.MinValue 
                && jToken.Value<double>() <= float.MaxValue;

            _validators["double precision"] = jToken 
                => jToken.Type == JTokenType.Float;

            _validators["smallserial"] = jToken => 
                jToken.Type == JTokenType.Integer 
                && jToken.Value<double>() > 0 
                && jToken.Value<double>() <= short.MaxValue;

            _validators["serial"] = jToken => 
                jToken.Type == JTokenType.Integer 
                && jToken.Value<double>() > 0 
                && jToken.Value<double>() <= int.MaxValue;

            _validators["bigserial"] = jToken => 
                jToken.Type == JTokenType.Integer 
                && jToken.Value<double>() > 0 
                && jToken.Value<double>() <= int.MaxValue;

            _validators["character"] = jToken => 
                jToken.Type == JTokenType.String;

            _validators["character varying"] = jToken => 
                jToken.Type == JTokenType.String;

            _validators["text"] = jToken => 
                jToken.Type == JTokenType.String;

            _validators["uuid"] = jToken => 
                jToken.Type == JTokenType.String
                && Guid.TryParse(jToken.Value<string>(), out _);

            _validators["timestamp with time zone"] = jToken => 
                jToken.Type == JTokenType.String 
                && DateTime.TryParse(jToken.Value<string>(), out _);

            _validators["timestamp without time zone"] = jToken =>
                jToken.Type == JTokenType.String 
                && DateTime.TryParse(jToken.Value<string>(), out _);

            _validators["date"] = jToken =>
                jToken.Type == JTokenType.String 
                && DateTime.TryParse(jToken.Value<string>(), out _);

            _validators["time with time zone"] = jToken =>
                jToken.Type == JTokenType.String 
                && DateTimeOffset.TryParse(jToken.Value<string>(), out _);

            _validators["time without time zone"] = jToken =>
                jToken.Type == JTokenType.String 
                && TimeSpan.TryParse(jToken.Value<string>(), out _);

            _validators["interval"] = jToken =>
                jToken.Type == JTokenType.String 
                && TimeSpan.TryParse(jToken.Value<string>(), out _);

            _validators["boolean"] = jToken => 
                jToken.Type == JTokenType.Boolean;

            _validators["json"] = jToken => 
                jToken.Type == JTokenType.String
                && jToken.Value<string>().TryParseJson(out _);

            _validators["jsonb"] = jToken => 
                jToken.Type == JTokenType.Object || jToken.Type == JTokenType.Array;
        }

        private readonly Dictionary<string, Func<JToken, bool>> _validators = new Dictionary<string, Func<JToken, bool>>();

        public virtual bool Validate(JToken jToken, string pgsqlType, bool allowNull)
        {
            if (jToken.Type == JTokenType.Array && pgsqlType.EndsWith("[]"))
            {
                var pType = pgsqlType.Replace("[]", "");
                if (!_validators.ContainsKey(pType)) return true;

                foreach (var token in (JArray) jToken)
                {
                    if (!_validators[pType](token)) return false;
                }

                return true;
            }
            
            return (allowNull && jToken.Type == JTokenType.Null) 
                   || !_validators.ContainsKey(pgsqlType) 
                   || _validators[pgsqlType](jToken);
        }
    }
}