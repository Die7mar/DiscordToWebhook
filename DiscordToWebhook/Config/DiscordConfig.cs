using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordToWebhook.Config
{
    class DiscordConfig
    {
        public string MessagePrefix { get; set; }
        public string Token { get; set; }
        public bool OnlyHuman { get; set; }
        public bool DeleteMsg { get; set; }
    }
}
