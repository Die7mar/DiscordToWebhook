using DiscordToWebhook.Config;
using RestSharp;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordToWebhook.Services
{
    class RestService
    {
        private readonly WebhookConfig _webhookConfig;
        private readonly ILogger _logger;

        public RestService(WebhookConfig webhookConfig, ILogger logger)
        {
            _webhookConfig = webhookConfig;
            _logger = logger;
        }

        private RestClient RestClientFactory()
        {
            _logger.Debug($"Create Restclient with {_webhookConfig.WebHookBaseUrl}");
            RestClient restClient = new RestClient(_webhookConfig.WebHookBaseUrl);
            return restClient;
        }

        public async Task<IRestResponse> Post(RestRequest restRequest)
        {
            _logger.Debug($"Post to {_webhookConfig.WebHookBaseUrl} to Method {restRequest.Method} " +
                         $"{restRequest.Resource}");
            var restClient = RestClientFactory();
            return await restClient.ExecuteAsync(restRequest);
        }

    }
}
