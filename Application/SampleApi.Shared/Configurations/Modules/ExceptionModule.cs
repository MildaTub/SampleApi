using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SampleApi.Shared.ExceptionHandlers;

namespace SampleApi.Shared.Configurations.Modules;

public class ExceptionModule
{
    public static void Configure(IServiceCollection services, IConfiguration config)
    {
        services.AddSingleton<IExceptionHandler, DomainExceptionHandler>();
        services.AddSingleton<IExceptionHandler, MissingEntityExceptionHandler>();
    }
}