WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

IWebHostEnvironment environment = builder.Environment;

IConfiguration configuration = builder.Configuration;

WebApplication app = builder.Build();

await app.RunAsync();
