namespace SendEmailService.Amqp.Configuration
{
    public static class BuilderConfiguration
    {
        public static IConfigurationBuilder AddAppSetting(this IConfigurationBuilder configurationBuilder,
                                                               IWebHostEnvironment environment)
        {
            configurationBuilder
                .AddJsonFile("config/appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment.ApplicationName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            return configurationBuilder;
        }
    }
}
