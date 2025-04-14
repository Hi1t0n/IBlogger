    using BaseLibrary.Classes.Extensions;
    using Serilog;
    using UserService.Domain;
    using UserService.Domain.Constants;
    using UserService.Domain.Models;
    using UserService.Host.Endpoints;
    using UserService.Host.Extensions;
    using UserService.Host.Middlewares;
    using UserService.Infrastructure.Extensions;

    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog();

    var connectionStringPostgreSql = builder.Environment.IsDevelopment()
        ? builder.Configuration.GetConnectionString(DatabasesConfigurationConstants.ConnectionStringDbConfiguration)!
        : Environment.GetEnvironmentVariable(DatabasesConfigurationConstants.ConnectionStringDbEnvironment)!;

    var connectionStringRedis = builder.Environment.IsDevelopment()
        ? builder.Configuration.GetConnectionString(DatabasesConfigurationConstants.ConnectionStringRedisConfiguration)!
        : Environment.GetEnvironmentVariable(DatabasesConfigurationConstants.ConnectionStringRedisEnvironment)!;

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddBusinessLogic(connectionStringPostgreSql, connectionStringRedis);

    var app = builder.Build();

    if (builder.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            options.RoutePrefix = string.Empty;
        });
        app.UseSerilogRequestLogging();
    }

    app.UseExceptionMiddleware();

    app.AddUserEndpoints();

    app.ApplyMigrations();
    
    app.Run();