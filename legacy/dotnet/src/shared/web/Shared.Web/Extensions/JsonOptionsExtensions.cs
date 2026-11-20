using System.Text.Json;
using Shared.Web.JsonConverters;

namespace Shared.Web.Extensions
{
    public static class JsonOptionsExtensions
    {
        public static void AddCrossdyneDefaults(this JsonSerializerOptions options)
        {
            options.PropertyNameCaseInsensitive = true;
            options.Converters.Add(new ErrorCodeJsonConverter());
        }
    }
}