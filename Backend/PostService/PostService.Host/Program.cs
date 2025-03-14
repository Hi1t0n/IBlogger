using PostService.Domain.Constants;
using PostService.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

var connectionStringDb = builder.Environment.IsDevelopment()
    ? builder.Configuration.GetConnectionString(ConnectionStrings.Postgre)!
    : Environment.GetEnvironmentVariable(ConnectionStrings.Postgre)!;

var connectionStringRedis = builder.Environment.IsDevelopment()
    ? builder.Configuration.GetConnectionString(ConnectionStrings.Redis)!
    : Environment.GetEnvironmentVariable(ConnectionStrings.Redis)!;

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddBusinessLogic(connectionStringDb);

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

app.Run();