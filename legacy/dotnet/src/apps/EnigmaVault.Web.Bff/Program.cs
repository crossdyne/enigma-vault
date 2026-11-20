using System.Reflection;
using EnigmaVault.Web.Bff.Extensions;
using Serilog;
using Shared.Logging;

var builder = WebApplication.CreateBuilder(args);

Assembly? assembly = Assembly.GetExecutingAssembly();
IConfiguration configuration = builder.Configuration;
IWebHostEnvironment environment = builder.Environment; 

builder.Logging.ClearProviders();
builder.Host.AddSerilogLogger(); 

builder.Services
    //Default
    .AddOpenApi()
    .AddAuthorization()
    .AddHttpContextAccessor()
    //Custom
    .AddCustomOptions()
    .AddServices(configuration)
    .AddHttpClients(configuration)
    .AddDelegationsHandlers()
    .AddDistributedLock()
    .UseCors()
    .AddSharedCryptoKeyForDecryptCookie(configuration)
    .AddCookie(environment);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseSerilogRequestLogging();
app.UseCors("AllowTrustedFrontend");
app.UseAuthentication(); 
app.UseAuthorization();  
app.MapEndpoints(assembly);

app.Run();