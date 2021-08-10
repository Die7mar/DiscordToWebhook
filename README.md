# DiscordToWebhook
Send webhooks with Discord. I have the problem I have a IPv4 light stack, it means the provider shared the IPv4 adress. So I can´t open ports. \
So I use a Discord bot tosend a webhook to my local home assistent.
The home assistent take the request and aktivate a automation. Example you can find in the Example folder.


## Configuration
1. First you need a Discord bot [link](https://docs.stillu.cc/guides/getting_started/first-bot.html) and add it to your server.
2. You edit the appsettings.json:
    - DiscordConfig:
         - MessagePrefix: The prefix that the boot trigger (for example "!ha")
         - Token: The discord bot token
         - OnlyHuman: When true it igonore when the messages came from a Bot
         - DeleteMsg: When true delete the message
    - WebhookConfig:
         - WebHookBaseUrl: Base url from Webhook (for example http://192.168.2.49:8123/api/webhook/)
3. Start the programm.

## More coming  soon maybe