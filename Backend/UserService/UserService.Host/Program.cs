    using Serilog;
    using UserService.Domain;
    using UserService.Host.Endpoints;
    using UserService.Host.Extensions;
    using UserService.Host.Middlewares;
    using UserService.Infrastructure.Extensions;

    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog();

    string connectionStringPostgreSql = builder.Environment.IsDevelopment()
        ? builder.Configuration.GetConnectionString(Constants.ConnectionStringDbConfiguration)!
        : Environment.GetEnvironmentVariable(Constants.ConnectionStringDbEnvironment)!;

    string connectionStringRedis = builder.Environment.IsDevelopment()
        ? builder.Configuration.GetConnectionString(Constants.ConnectionStringRedisConfiguration)!
        : Environment.GetEnvironmentVariable(Constants.ConnectionStringRedisEnvironment)!;

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