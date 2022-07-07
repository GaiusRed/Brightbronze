using System;
using System.Threading.Tasks;
using Discord.Commands;
using Microsoft.Extensions.Logging;

namespace red.gaius.brightbronze.discord.Services.Modules
{
    public class StartModule : ModuleBase<SocketCommandContext>
    {
        readonly core.Services.Engine _engine;
        readonly ILogger<StartModule> _logger;

        public StartModule(core.Services.Engine engine, ILogger<StartModule> logger)
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
                _logger.LogTrace("New server; onboarding...");
                core.Models.ServerInfo newServer = new core.Models.ServerInfo()
                {
                    id = Guid.NewGuid().ToString(),
                    serverId = Context.Guild.Id.ToString(),
                    name = Context.Guild.Name,
                    ownerUserId = Context.Guild.OwnerId.ToString()
                };
                if (!await _engine.data.SetServerInfo(newServer))
                {
                    _logger.LogWarning("Unable to onboard new server."); // Add context
                    await ReplyAsync(await _engine.data.GetScript("ServerOnboardFail"));
                    return;
                }
            }
            if (!await _engine.data.UserInfoExists(Context.User.Id.ToString()))
            {
                _logger.LogTrace("New user; onboarding...");
                core.Models.UserInfo newUser = new core.Models.UserInfo()
                {
                    id = Guid.NewGuid().ToString(),
                    userId = Context.User.Id.ToString(),
                    name = Context.User.Username,
                    discriminator = Context.User.Discriminator
                };
                if (!await _engine.data.SetUserInfo(newUser))
                {
                    _logger.LogWarning("Unable to onboard new user."); // Add context
                    await ReplyAsync(await _engine.data.GetScript("UserOnboardFail"));
                    return;
                }
            }
            if (!await _engine.data.UserCharactersExist(Context.User.Id.ToString()))
            {
                _logger.LogTrace("User has no characters in roster; starting story via DM...");
                await Context.Message.DeleteAsync();
                Onboard();
                return;
            }
            ShowMenu();
        }

        private void Onboard()
        {
            // TODO: Onboard via PM
        }

        private void ShowMenu()
        {
            // TODO: Show Game Menu
        }
    }
}
