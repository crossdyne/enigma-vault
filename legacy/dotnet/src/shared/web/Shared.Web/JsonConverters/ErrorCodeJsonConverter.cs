using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Crossdyne.Toolkit.Results;

namespace Shared.Web.JsonConverters
{
    public class ErrorCodeJsonConverter : JsonConverter<ErrorCode>
    {
        private static readonly Func<string, int, ErrorCode>? _createErrorCode = 
            CreateConstructorDelegate<string, int, ErrorCode>(BindingFlags.NonPublic | BindingFlags.Instance);

        private static Func<TArg1, TArg2, TResult>? CreateConstructorDelegate<TArg1, TArg2, TResult>(BindingFlags flags)
            where TResult : struct 
        {
            var ctor = typeof(TResult).GetConstructor(
                flags, null, [typeof(TArg1), typeof(TArg2)], null);
            
            if (ctor == null) return null;
            
            var param1 = Expression.Parameter(typeof(TArg1), "arg1");
            var param2 = Expression.Parameter(typeof(TArg2), "arg2");
            var newExpr = Expression.New(ctor, param1, param2);
            var lambda = Expression.Lambda<Func<TArg1, TArg2, TResult>>(newExpr, param1, param2);
            
            return lambda.Compile();
        }

        public override ErrorCode Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using var doc = JsonDocument.ParseValue(ref reader);
            var root = doc.RootElement;
            
            string name = null!;
            int code = 0;

            foreach (var prop in root.EnumerateObject())
            {
                if (prop.NameEquals("name") || prop.NameEquals("Name"))
                    name = prop.Value.GetString()!;
                else if (prop.NameEquals("code") || prop.NameEquals("Code"))
                    code = prop.Value.GetInt32();
            }
            
            if (_createErrorCode != null && name != null)
                return _createErrorCode(name, code);

                return default;
            }

        public override void Write(Utf8JsonWriter writer, ErrorCode value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString("name", value.Name);
            writer.WriteNumber("code", value.Code);
            writer.WriteEndObject();
        }
    }
}