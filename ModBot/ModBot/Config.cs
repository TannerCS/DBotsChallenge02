using Discord;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ModBot
{
    class Config
    {
        private static JObject _Json = JObject.Parse(File.ReadAllText("config.json"));
        public static Dictionary<int, ModMail> _Modmail = new Dictionary<int, ModMail>();

        public static string BotToken;
        public static IMessageChannel ModmailChannel;

        public Config()
        {
            //check if bot token or modmail channel id is null
            if(string.IsNullOrWhiteSpace(_Json["botToken"].ToString()) || string.IsNullOrWhiteSpace(_Json["modmailChannelID"].ToString()))
            {
                throw new Exception("Bot token or modmail channel id is null. Open config.json");
            }

            BotToken = _Json["botToken"].ToString();
        }

        public static void GetModmailChannel()
        {
            //set modmail channel
            ModmailChannel = (IMessageChannel)Program.client.Guilds.FirstOrDefault(x => x.Channels.FirstOrDefault(y => y.Id == (ulong)_Json["modmailChannelID"]).Id == (ulong)_Json["modmailChannelID"]).GetChannel((ulong)_Json["modmailChannelID"]);
        }
    }
}
