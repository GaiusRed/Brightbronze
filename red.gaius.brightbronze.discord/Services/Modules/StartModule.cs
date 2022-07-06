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
            if (!await _engine._extData.ServerInfoExists(Context.Guild.Id.ToString()))
            {
                _logger.LogTrace("New server; onboarding...");
                core.Models.ServerInfo newServer = new core.Models.ServerInfo()
                {
                    id = Guid.NewGuid().ToString(),
                    serverId = Context.Guild.Id.ToString(),
                    name = Context.Guild.Name,
                    ownerUserId = Context.Guild.OwnerId.ToString()
                };
                if (!await _engine._extData.SetServerInfo(newServer))
                {
                    _logger.LogWarning("Unable to onboard new server."); // Add context
                    await ReplyAsync("Sorry, but you cannot play the Brightbronze Idle RPG on this server.");
                    return;
                }
            }
            if (!await _engine._extData.UserInfoExists(Context.User.Id.ToString()))
            {
                _logger.LogTrace("New user; onboarding...");
                core.Models.UserInfo newUser = new core.Models.UserInfo()
                {
                    id = Guid.NewGuid().ToString(),
                    userId = Context.User.Id.ToString(),
                    name = Context.User.Username,
                    discriminator = Context.User.Discriminator
                };
                if (!await _engine._extData.SetUserInfo(newUser))
                {
                    _logger.LogWarning("Unable to onboard new user."); // Add context
                    await ReplyAsync("Sorry, but Brightbronze Idle RPG is unable to register your Discord user.");
                    return;
                }
            }
        }
    }
}
