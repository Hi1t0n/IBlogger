using BaseLibrary.Classes.Extensions;
using PostService.Domain.Constants;
using PostService.Host.Endpoints;
using PostService.Host.Extensions;
using PostService.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

var connectionStringDataBase = builder.Environment.IsDevelopment()
    ? builder.Configuration.GetConnectionString(DatabaseConfig.DataBaseConnectionStringConfigurationName)!
    : Environment.GetEnvironmentVariable(DatabaseConfig.DataBaseConnectionStringConfigurationName)!;

var connectionStringRedis = builder.Environment.IsDevelopment()
    ? builder.Configuration.GetConnectionString(RedisConfig.RedisConnectionStringConfigurationName)!
    : Environment.GetEnvironmentVariable(RedisConfig.RedisConnectionStringConfigurationName)!;

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddBusinessLogic(connectionStringDataBase, connectionStringRedis);

var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

app.ApplyMigrations();
app.AddPostEndpoints();

app.Run();