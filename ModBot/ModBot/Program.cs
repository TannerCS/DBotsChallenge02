using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace ModBot
{
    class Program
    {
        public static DiscordSocketClient client;
        private CommandService commands;
        private IServiceProvider services;

        private static void Main(string[] args) => new Program().Start().GetAwaiter().GetResult();

        public async Task Start()
        {
            //new client
            client = new DiscordSocketClient();
            //new command
            commands = new CommandService();
            //new service
            services = new ServiceCollection()
                    .BuildServiceProvider();
            new Config();

            //log messages
            client.Log += Log;
            client.Ready += OnReady;

            await InstallCommands();

            //log in
            await client.LoginAsync(TokenType.Bot, Config.BotToken);
            await client.StartAsync();

            await Task.Delay(-1);
        }

        private async Task OnReady()
        {
            Config.GetModmailChannel();
        }

        public async Task Log(LogMessage message)
        {
            //log messages to console
            Console.WriteLine(message);
        }

        public async Task InstallCommands()
        {
            // Hook the MessageReceived Event into our Command Handler
            client.MessageReceived += HandleCommand;
            // Discover all of the commands in this assembly and load them.
            await commands.AddModulesAsync(Assembly.GetEntryAssembly());
        }

        public async Task HandleCommand(SocketMessage messageParam)
        {
            // Don't process the command if it was a System Message
            var message = messageParam as SocketUserMessage;
            if (message == null) return;
            // Create a number to track where the prefix ends and the command begins
            int argPos = 0;
            // Determine if the message is a command, based on if it starts with '!' or a mention prefix
            if (!(message.HasCharPrefix('!', ref argPos) || message.HasMentionPrefix(client.CurrentUser, ref argPos))) return;
            // Create a Command Context
            var context = new CommandContext(client, message);
            // Execute the command. (result does not indicate a return value, 
            // rather an object stating if the command executed successfully)
            var result = await commands.ExecuteAsync(context, argPos, services);
            if (!result.IsSuccess)
                await context.Channel.SendMessageAsync(result.ErrorReason);
        }
    }
}
