using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Serilog;

namespace red.gaius.brightbronze.discord.Services.Modules
{
    public class StartModule : ModuleBase<SocketCommandContext>
    {
        readonly core.Services.Engine _engine;
        readonly ILogger _logger;

        public StartModule(core.Services.Engine engine, ILogger logger)
        {
            _engine = engine;
            _logger = logger;
        }

        [Command("menu")]
        [Summary("Starts and/or continues the Brightbronze Idle RPG.")]
        public async Task Start()
        {
            if (Context.IsPrivate)
            {
                await ReplyAsync("You are currently not in a server.");
                return;
            }
            if (!await _engine.data.ServerInfoExists(Context.Guild.Id.ToString()))
            {
                _logger.Verbose("New server; onboarding...");
                core.Models.ServerInfo newServer = new core.Models.ServerInfo()
                {
                    id = Guid.NewGuid().ToString(),
                    serverId = Context.Guild.Id.ToString(),
                    name = Context.Guild.Name,
                    ownerUserId = Context.Guild.OwnerId.ToString()
                };
                if (!await _engine.data.SetServerInfo(newServer))
                {
                    _logger
                        .ForContext("New Server Id", newServer.id)
                        .ForContext("New Server Discord Id", newServer.serverId)
                        .ForContext("New Server Name", newServer.name)
                        .ForContext("New Server Discord Owner Id", newServer.ownerUserId)
                        .Warning("Unable to onboard new server.");
                    await ReplyAsync(await _engine.data.GetScript("ServerOnboardFail"));
                    return;
                }
            }
            if (!await _engine.data.UserInfoExists(Context.User.Id.ToString()))
            {
                _logger.Verbose("New user; onboarding...");
                core.Models.UserInfo newUser = new core.Models.UserInfo()
                {
                    id = Guid.NewGuid().ToString(),
                    userId = Context.User.Id.ToString(),
                    name = Context.User.Username,
                    discriminator = Context.User.Discriminator
                };
                if (!await _engine.data.SetUserInfo(newUser))
                {
                    _logger
                        .ForContext("New User Id", newUser.id)
                        .ForContext("New User Discord Id", newUser.userId)
                        .ForContext("New User Discord Name", newUser.name)
                        .ForContext("New User Discord Discriminator", newUser.discriminator)
                        .Warning("Unable to onboard new user.");
                    await ReplyAsync(await _engine.data.GetScript("UserOnboardFail"));
                    return;
                }
            }
            if (!await _engine.data.UserCharactersExist(Context.User.Id.ToString()))
            {
                _logger.Verbose("User has no characters in roster; starting story via DM...");
                await Context.Message.DeleteAsync();
                Onboard();
                return;
            }
            ShowMenu();
        }

        private async void Onboard()
        {
            var welcome = new EmbedBuilder(){
                Color = Color.Red,
                Title = await _engine.data.GetScript("NewPlayerTitle"),
                Fields = new System.Collections.Generic.List<EmbedFieldBuilder>(){
                    new EmbedFieldBuilder() {
                        Name = await _engine.data.GetScript("NewPlayerField1Name"),
                        Value = await _engine.data.GetScript("NewPlayerField1Value")
                    },
                    new EmbedFieldBuilder() {
                        Name = await _engine.data.GetScript("NewPlayerField2Name"),
                        Value = await _engine.data.GetScript("NewPlayerField2Value")
                    },
                    new EmbedFieldBuilder() {
                        Name = await _engine.data.GetScript("NewPlayerField3Name"),
                        Value = await _engine.data.GetScript("NewPlayerField3Value")
                    }
                },
                Footer = new EmbedFooterBuilder(){
                    IconUrl = Context.Client.CurrentUser.GetAvatarUrl(),
                    Text = await _engine.data.GetScript("NewPlayerFooter")
                }
            }.Build();
            var dm = await Context.User.CreateDMChannelAsync();
            await dm.SendMessageAsync(embed: welcome);
        }

        private void ShowMenu()
        {
            // TODO: Show Game Menu
        }
    }
}
