using System.Net.Sockets;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using MongoDB.Driver;

namespace SampleApi.Configurations.Modules;

public sealed class MongoModule
{
    public static void Configure(IServiceCollection services, IConfiguration config)
    {
        IConfigurationSection mongoSection = config.GetSection("Persistence:Mongo");
        IConfigurationSection databaseTitleSection = mongoSection.GetSection("DatabaseTitle");
        var mongoConnectionString = Environment.GetEnvironmentVariable("MONGO_CONNECTION_STRING");
        MongoClientSettings clientSettings = MongoClientSettings.FromUrl(new MongoUrl(mongoConnectionString));

        Action<Socket> socketConfigurator = s => s.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.TcpKeepAliveTime, 120);
        clientSettings.ClusterConfigurator = cb => cb.ConfigureTcp(tcp => tcp.With(socketConfigurator: socketConfigurator));
        services.AddSingleton(new MongoClient(clientSettings).GetDatabase(databaseTitleSection.Value));

#pragma warning disable 618
        BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
    }
}