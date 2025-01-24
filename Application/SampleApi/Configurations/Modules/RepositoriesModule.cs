using SampleApi.Models.Users;
using SampleApi.Mongo.Users;

namespace SampleApi.Configurations.Modules;

public sealed class RepositoriesModule
{
    public static void Configure(IServiceCollection services, IConfiguration config)
    {
        services.AddTransient<IUserRepository, UserRepository>();
    }
}