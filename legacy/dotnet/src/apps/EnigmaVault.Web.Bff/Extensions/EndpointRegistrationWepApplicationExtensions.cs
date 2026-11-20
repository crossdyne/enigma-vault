using System.Reflection;

namespace EnigmaVault.Web.Bff.Extensions
{
    public static class EndpointRegistrationWepApplicationExtensions
    {
        public static void MapEndpoints(this IEndpointRouteBuilder app, Assembly assembly)
        {
            var endpointMethods = assembly
            .GetTypes()
            .SelectMany(t => t.GetMethods(BindingFlags.Public | BindingFlags.Static))
            .Where(m =>
                m.Name.StartsWith("Map") &&
                m.ReturnType == typeof(void) &&
                m.GetParameters() is [{ ParameterType: Type paramType}] &&
                paramType == typeof(IEndpointRouteBuilder));

                foreach (var method in endpointMethods)
                    method.Invoke(null, [app]);
        }
    }
}