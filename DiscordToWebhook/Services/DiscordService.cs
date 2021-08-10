using Discord;
using Discord.Rest;
using Discord.WebSocket;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordToWebhook.Services
{
    class DiscordService
    {
        private readonly Config.DiscordConfig _discordConfig;
        private readonly ILogger _logger;
        private DiscordSocketClient _client;


        public DiscordService(Config.DiscordConfig discordConfig, ILogger logger)
        {
            _discordConfig = discordConfig;
            _logger = logger;

            _client = new DiscordSocketClient();
            _client.Log += Log;

            _client.LoginAsync(TokenType.Bot, discordConfig.Token);
            _client.StartAsync();

        }

        public void AddMessageReceivedHandler(Func<SocketMessage, Task<Task>> CommandHandler)
        {
            _client.MessageReceived += CommandHandler;
        }

        public void RemoveMessageReceivedHandler(Func<SocketMessage, Task<Task>> CommandHandler)
        {
            _client.MessageReceived -= CommandHandler;
        }

        public async Task<RestUserMessage> WriteChatMessage(ulong channelID, string message)
        {
            var channel = _client.GetChannel(channelID) as ISocketMessageChannel;
            return await channel.SendMessageAsync(message);
        }

        public async Task DeleteMessage(SocketMessage msg)
        {
            var channel = _client.GetChannel(msg.Channel.Id) as ISocketMessageChannel;
            await channel.DeleteMessageAsync(msg.Id);
        }

        private Task Log(LogMessage msg)
        {
            string logText = $"{msg.Message}";
            if (!string.IsNullOrEmpty(msg.Exception?.ToString()))
            {
                logText = $"{msg.Exception} {msg.Message}";
            }

            switch (msg.Severity.ToString())
            {
                case "Critical": _logger.Fatal(logText); break;
                case "Warning": _logger.Warning(logText); break;
                case "Info": _logger.Information(logText); break;
                case "Verbose": _logger.Verbose(logText); break;
                case "Debug": _logger.Debug(logText); break;
                case "Error": _logger.Error(logText); break;
            }

            return Task.CompletedTask;
        }
    }
}
