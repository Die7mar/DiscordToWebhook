using DiscordToWebhook.Config;
using DiscordToWebhook.Services;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DiscordToWebhook
{
    class Program
    {
        private ILogger _logger;

        public static int Main(string[] args)
        {
            IConfigurationRoot Configuration = ReadAppSetting();

            try
            {
                new Program().MainAsync(Configuration).GetAwaiter().GetResult();
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 1;
            }
        }

        public async Task MainAsync(IConfigurationRoot Configuration)
        {
            _logger = new LoggerConfiguration()
                          .ReadFrom.Configuration(Configuration)
                          .CreateLogger();

            _logger.Debug("Logger created");
            var configDiscord = Configuration.GetSection("DiscordConfig").Get<DiscordConfig>();
            var configWebhook = Configuration.GetSection("WebhookConfig").Get<WebhookConfig>();
            _logger.Debug("Config loaded");

            _logger.Debug("Create discordToWebhookService");
            new DiscordToWebhookService(configDiscord, configWebhook, _logger);
            _logger.Debug("discordToWebhookService created");

            await Task.Delay(-1);
        }

        public static IConfigurationRoot ReadAppSetting()
        {
            string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                          .SetBasePath(Directory.GetCurrentDirectory())
                          .AddJsonFile("appsettings.json");
            if (environment == "Development")
            {
                builder.AddJsonFile($"appsettings.{environment}.json", optional: true);
            }
            else if (!string.IsNullOrEmpty(environment))
            {
                builder.AddJsonFile($"appsettings.{environment}.json", optional: false);
            }

            return builder.Build();
        }

    }
}
