using SampleApi.Services.Users;

namespace SampleApi.Configurations.Modules;

public sealed class ServicesModule
{
    public static void Configure(IServiceCollection services, IConfiguration config)
    {
        services.AddTransient<IUserService, UserService>();
    }
}