using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordToWebhook.TransferObjects
{
    class CommandObj
    {
        public string WebhookId { get; set; }
        public Dictionary<string, string> Body { get; set; }
        public string JsonBoy { get => GenerateJsonBody(); }

        public CommandObj()
        {

        }

        public CommandObj(string[] messageParts)
        {
            WebhookId = messageParts[0];

            Body = new Dictionary<string, string>();

            for (int i = 1; i < messageParts.Length; i++)
            {
                Body.Add("value" + i, messageParts[i]);
            }
        }

        private string GenerateJsonBody()
        {
            return JsonConvert.SerializeObject(Body);
        }
    }
}
