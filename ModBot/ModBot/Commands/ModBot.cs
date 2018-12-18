using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModBot.Commands
{
    [Group("modmail")]
    public class ModMailCommand : ModuleBase
    {
        private EmbedBuilder _Builder;

        //default command
        [Command]
        public async Task Info()
        {
            _Builder = new EmbedBuilder();
            _Builder.WithTitle("Have an issue?");
            _Builder.WithDescription(
                $"If you have an issue, you can send modmail to a mod!" +
                $"\n" +
                $"\n" +
                $"User Commands:\n" +
                $"!modmail: Information about {Context.Client.CurrentUser.Username}\n" +
                $"!modmail create <message>: Sends a message to the mods with your current issue\n" +
                $"!modmail reply <ID> <message>: Reply to a ticket\n" +
                $"\n" +
                $"Admin Commands:\n" +
                $"!modmail anonreply <ID> <message>: Reply to a user message anonymously\n" +
                $"!modmail close <ID> <reason (optional)>"
                );

            await ReplyAsync("", false, _Builder.Build());
        }

        //create modmail
        [Command("create")]
        public async Task Create([Remainder]string message)
        {
            //if channel is not a DM channel
            if (!(Context.Channel is IDMChannel))
                await Context.Message.DeleteAsync();

            int ID = new Random((int)DateTime.Now.Ticks).Next(0, 9999);
            //while ID exists in tickets, create a new one
            while (Config._Modmail.ContainsKey(ID)){
                ID = new Random((int)DateTime.Now.Ticks).Next(0, 9999);
            }

            _Builder = new EmbedBuilder();
            _Builder.WithTitle($"ModMail #{ID}");
            _Builder.WithDescription($"{Context.User.Username}#{Context.User.Discriminator}: {message}");
            _Builder.WithFooter($"Reply to this ticket with `!modmail reply {ID} <message>`");

            //send a message to the modmail channel and user
            var modMessageChannel = await Config.ModmailChannel.SendMessageAsync("", false, _Builder.Build());
            var userMessageChannel = await Context.User.SendMessageAsync("", false, _Builder.Build());

            //add the modmail
            Config._Modmail.Add(ID, new ModMail() { Messages = new List<string>() { message }, User = Context.User });
        }

        [Command("reply")]
        public async Task Reply(int ID, [Remainder]string message)
        {
            if (!(Context.Channel is IDMChannel))
                await Context.Message.DeleteAsync();

            if (!Config._Modmail.ContainsKey(ID))
            {
                await ReplyAsync("ID not found.");
                return;
            }

            Config._Modmail[ID].Messages.Add($"{Context.User.Username}#{Context.User.Discriminator}: {message}");
            string conversation = string.Join("\n", Config._Modmail[ID].Messages);
            
            _Builder = new EmbedBuilder();
            _Builder.WithTitle($"ModMail #{ID}");
            _Builder.WithDescription($"{Context.User.Username}#{Context.User.Discriminator}: {message}");
            _Builder.WithFooter($"Reply to this modmail with `!modmail reply {ID} <message>`");

            await Config.ModmailChannel.SendMessageAsync("", false, _Builder.Build());
            await Config._Modmail[ID].User.SendMessageAsync("", false, _Builder.Build());
        }

        [Command("anonreply")]
        public async Task AnonReply(int ID, [Remainder]string message)
        {
            if (!(Context.Channel is IDMChannel))
                await Context.Message.DeleteAsync();

            if (!Config._Modmail.ContainsKey(ID))
            {
                await ReplyAsync("ID not found.");
                return;
            }

            Config._Modmail[ID].Messages.Add($"{Context.User.Username}#{Context.User.Discriminator}: {message}");
            //string conversation = string.Join("\n", Config._Tickets[ID].Messages);

            _Builder = new EmbedBuilder();
            _Builder.WithTitle($"Ticket #{ID}");
            _Builder.WithDescription($"Anonymous: {message}");
            _Builder.WithFooter($"Reply to this modmail with `!modmail reply {ID} <message>`");

            await Config.ModmailChannel.SendMessageAsync("", false, _Builder.Build());
            await Config._Modmail[ID].User.SendMessageAsync("", false, _Builder.Build());
        }

        [Command("close")]
        public async Task Close(int ID, [Remainder]string reason = null)
        {
            if (!(Context.Channel is IDMChannel))
                await Context.Message.DeleteAsync();

            if (Context.Channel.Id != Config.ModmailChannel.Id || Context.User.Id == Config._Modmail[ID].User.Id)
            {
                await ReplyAsync("You can't close modmail here!");
                return;
            }
            if (!Config._Modmail.ContainsKey(ID))
            {
                await ReplyAsync("ID not found.");
                return;
            }

            _Builder = new EmbedBuilder();
            _Builder.WithTitle($"Ticket #{ID}");
            if(reason != null)
                _Builder.WithDescription($"Ticket Closed @{DateTime.Now.ToUniversalTime()} UTC\nReason: {reason}");
            else
                _Builder.WithDescription($"Ticket Closed @{DateTime.Now.ToUniversalTime()} UTC");

            await Config.ModmailChannel.SendMessageAsync("", false, _Builder.Build());
            await Config._Modmail[ID].User.SendMessageAsync("", false, _Builder.Build());

            Config._Modmail.Remove(ID);
        }
    }
}
