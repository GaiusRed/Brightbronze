using System.Threading.Tasks;
using Discord.Commands;

namespace red.gaius.brightbronze.discord.Services.Modules
{
    public class StartModule : ModuleBase<SocketCommandContext>
    {
        readonly core.Services.Engine _engine;
        public StartModule(core.Services.Engine engine)
        {
            _engine = engine;
        }

        [Command("registerServer")]
        [Summary("Registers the current Discord Server into the Brightbronze Game.")]
        public async Task ServerJoin()
        {
            string serverId = Context.Guild.Id.ToString();
            if (await _engine._extData.ServerInfoExists(serverId))
            {
                await ReplyAsync("Server already registered.");
                return;
            }
            await ReplyAsync("Under Construction...");
        }

        [Command("join")]
        [Summary("Joins the current User into the Brightbronze game.")]
        public async Task Join()
        {
            await ReplyAsync("Under Construction...");
        }
    }
}
