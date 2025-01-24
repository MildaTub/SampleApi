using SampleApi.Configurations.Modules;
using SampleApi.Shared.Configurations.Modules;
using SampleApi.Shared.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<TimeProvider>(TimeProvider.System);

MongoModule.Configure(builder.Services, builder.Configuration);
RepositoriesModule.Configure(builder.Services, builder.Configuration);
ServicesModule.Configure(builder.Services, builder.Configuration);
ExceptionModule.Configure(builder.Services, builder.Configuration);

var app = builder.Build();
app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
