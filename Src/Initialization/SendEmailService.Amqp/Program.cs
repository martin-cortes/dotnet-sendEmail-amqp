using Application.Common.Helpers.Serializer;
using Application.Common.Utilities;
using Infrastructure;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using SendEmailService.Amqp.Configuration;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

IWebHostEnvironment environment = builder.Environment;

IConfiguration configuration = builder.Configuration;

#region Configuration

builder.Configuration.AddAppSetting(environment);

string connectionString = configuration["AppSettings:ConnectionStringMongo"];

builder.Configuration
    .AddMongoProvider(connectionString,
                      configuration["AppSettings:DatabaseName"],
                      configuration["MongoConfigurationProvider:CollectionName"],
                      Convert.ToBoolean(configuration["MongoConfigurationProvider:ReloadOnChange"]),
                      nameof(BusinessSettings)
    );

#endregion Configuration

#region Service - Host

builder.Services
    .Configure<BusinessSettings>(builder.Configuration
    .GetRequiredSection(nameof(BusinessSettings)));

BusinessSettings settings = builder.Configuration
    .GetSection(nameof(BusinessSettings))
    .Get<BusinessSettings>();

Console.WriteLine($"Mongo Provider Configuration: \n{SerializerObject.ConvertObjectToJsonIndented(settings)}");

builder.Host
    .ConfigureSerilog(settings.LogLevelSink,
                      configuration["AppSettings:DatabaseName"],
                      settings.SinkCollectionName,
                      connectionString,
                      settings.DocumentExpiration
    );

builder.Services
    .RegisterService()
    .RegisterHealthCheck()
    .RegisterMongo(connectionString,
                   configuration["AppSettings:DatabaseName"],
                   configuration["AppSettings:CollectionName"]
    );

string[] mongoHealthCheck = ["MongoDB", "Mongo"];

builder.Services
    .AddHealthChecks()
    .AddMongoDb(connectionString,
                configuration["AppSettings:DatabaseName"],
                name: "Mongo",
                failureStatus: HealthStatus.Unhealthy,
                tags: mongoHealthCheck
    );

#endregion Service - Host

#region Middleware

WebApplication app = builder.Build();

#region Logging

ILogger logger = app.Logger;

IHostApplicationLifetime lifetime = app.Lifetime;

lifetime.ApplicationStarted.Register(() =>
{
    ProgramInformation information = new()
    {
        ApplicationName = environment.ApplicationName,
        HealthChecksEndPoint = settings.HealthChecksEndPoint,
        EnvironmentName = environment.EnvironmentName,
        Date = DateTime.UtcNow.ToLocalTime()
    };

    logger.LogInformation("=============== PROGRAM ============= \n" +
                          "{Information} \n" +
                          "============= END PROGRAM ===========",
                          SerializerObject.ConvertObjectToJsonIndented(information)
    );
});

#endregion Logging

await app.RunAsync();

#endregion Middleware