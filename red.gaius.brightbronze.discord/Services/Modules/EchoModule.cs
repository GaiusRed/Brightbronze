using System.Threading.Tasks;
using Discord.Commands;

namespace red.gaius.brightbronze.discord.Services.Modules
{
    public class EchoModule : ModuleBase<SocketCommandContext>
    {
        [Command("echo")]
        [Summary("Echoes a message.")]
        public async Task SayAsync([Remainder][Summary("The text to echo")] string echo)
        {
            await ReplyAsync(echo);
        }

        [Command("ping")]
        [Summary("Returns server latency.")]
        public async Task PingAsync()
        {
            await ReplyAsync($"Pong! Latency: { Context.Client.Latency.ToString() }ms");
        }
    }
}
