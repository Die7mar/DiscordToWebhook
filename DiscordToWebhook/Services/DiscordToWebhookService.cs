using Discord.WebSocket;
using DiscordToWebhook.Config;
using DiscordToWebhook.TransferObjects;
using RestSharp;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordToWebhook.Services
{
    class DiscordToWebhookService
    {
        private readonly Config.DiscordConfig _discordConfig;
        private readonly ILogger _logger;
        private DiscordService _discordService;
        private RestService _restService;


        public DiscordToWebhookService(Config.DiscordConfig discordConfig, WebhookConfig webhookConfig,
                                       ILogger logger)
        {
            _logger = logger;
            _discordService = new DiscordService(discordConfig, logger);
            _restService = new RestService(webhookConfig, logger);

            _discordConfig = discordConfig;

            _discordService.AddMessageReceivedHandler(CommandHandler);

            _logger.Information($"Init compilte rest service url: {webhookConfig.WebHookBaseUrl} " +
                                $"and prefix: {discordConfig.MessagePrefix}");

        }


        private async Task<Task> CommandHandler(SocketMessage msg)
        {
            if (!MsgIsValid(msg)) { return Task.CompletedTask; }

            _logger.Information($"Execute message: {msg.Content} " +
                                $"from {msg.Author.Username} in channel: {msg.Channel.Name} ");

            var msgContentWithOutPrefix = msg.Content.Replace(_discordConfig.MessagePrefix, "").Trim();
            CommandObj commandObj = new CommandObj(msgContentWithOutPrefix.Split(' '));

            RestRequest restRequest = new RestRequest("{webhook-id}", Method.POST);
            restRequest.AddUrlSegment("webhook-id", commandObj.WebhookId);

            if (commandObj.JsonBoy != string.Empty)
            {
                restRequest.AddJsonBody(commandObj.JsonBoy);
            }

            var response = await _restService.Post(restRequest);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                string errorMsg = $"Error: {msg.Content}: {response.ErrorMessage}";
                _logger.Error(errorMsg);
                await _discordService.WriteChatMessage(msg.Channel.Id, errorMsg);
            }

            if (_discordConfig.DeleteMsg)
            {
                await _discordService.DeleteMessage(msg);
            }

            return Task.CompletedTask;
        }

        private bool MsgIsValid(SocketMessage msg)
        {
            bool output = false;

            if (msg.Content.StartsWith(_discordConfig.MessagePrefix))
            {
                output = true;
                if (msg.Author.IsBot && _discordConfig.OnlyHuman) { output = false; };
            }
            return output;
        }

    }
}

