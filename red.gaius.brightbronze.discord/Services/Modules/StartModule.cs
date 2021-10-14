using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.Rest;
using Discord.WebSocket;

namespace red.gaius.brightbronze.discord.Services.Modules
{
    public class StartModule : ModuleBase<SocketCommandContext>
    {
        const string btnServerJoin = "serverJoin";

        readonly core.Services.Engine _engine;
        public StartModule(core.Services.Engine engine)
        {
            _engine = engine;

            OnServerJoinIsSubbed = false;
        }

        [Command("registerServer")]
        [Summary("Registers the current Discord Server into the Brightbronze Game.")]
        public async Task ServerJoin()
        {
            OnServerJoinSubscribe();
            if (Context.IsPrivate)
            {
                await ReplyAsync("You are currently not in a Server.");
                return;
            }
            if (Context.Guild.OwnerId != Context.User.Id)
            {
                await ReplyAsync("Sorry, only Server Owners are allowed to register into the game.");
                return;
            }
            if (await _engine._extData.ServerInfoExists(Context.Guild.Id.ToString()))
            {
                await ReplyAsync("Server already registered.");
                return;
            }

            ComponentBuilder builder = new ComponentBuilder().WithButton("As the Server Owner, I acknowledge the changes that the bot will apply.", btnServerJoin, emote: Emoji.Parse(":ballot_box_with_check:"));

            await ReplyAsync("WARNING!\nBy registering this server to join the Brightbronze Game, this `bot` will create and manage several Channel Groups and Channels.", component: builder.Build());
        }

        private bool OnServerJoinIsSubbed;
        private void OnServerJoinSubscribe()
        {
            if (OnServerJoinIsSubbed) return;
            Context.Client.ButtonExecuted += async (component) =>
            {
                if (component.Data.CustomId == btnServerJoin)
                    if (Context.Guild.OwnerId == component.User.Id)
                        await ServerJoin(component);
            };
            OnServerJoinIsSubbed = true;
        }

        private async Task ServerJoin(SocketMessageComponent component)
        {
            ulong msgId = component.Message.Id;
            await component.UpdateAsync((msg) =>
            {
                msg.Components = (new ComponentBuilder()).Build();
            });
            if (await _engine._extData.ServerInfoExists(Context.Guild.Id.ToString()))
            {
                await ReplyAsync("Server already registered.", messageReference: new MessageReference(msgId));
                return;
            }

            IUserMessage msg = await ReplyAsync("Please wait, registering the server...",
                messageReference: new MessageReference(msgId));
            // Create Channel Groups & Channels...
            RestCategoryChannel category = await Context.Guild.CreateCategoryChannelAsync(
                _engine._settings.ChannelCategoryName);
            await Context.Guild.CreateTextChannelAsync(_engine._settings.QuestChannel,
                (p) => p.CategoryId = category.Id);
            await Context.Guild.CreateTextChannelAsync(_engine._settings.MarketChannel,
                (p) => p.CategoryId = category.Id);
            await Context.Guild.CreateTextChannelAsync(_engine._settings.ArenaChannel,
                (p) => p.CategoryId = category.Id);
            for (int i = 1; i <= _engine._settings.DefaultDelveChannelCount; i++)
                await Context.Guild.CreateTextChannelAsync("BBRZ-Delve-" + i.ToString(),
                    (p) => p.CategoryId = category.Id);
            // Save to extData...
            string result = "Congratulations! This server has officially joined the Brightbronze Game!";
            core.Models.ServerInfo newServer = new core.Models.ServerInfo()
            {
                id = Guid.NewGuid().ToString(),
                serverId = Context.Guild.Id.ToString(),
                name = Context.Guild.Name,
                ownerUserId = Context.Guild.OwnerId.ToString(),
                structure = "info"
            };
            if (!await _engine._extData.SetServerInfo(newServer))
                result = "Unfortunately, something went wrong. Unable to register this server.";
            await msg.ModifyAsync((m) => m.Content = result);
        }

        [Command("join")]
        [Summary("Joins the current User into the Brightbronze game.")]
        public async Task UserJoin()
        {
            if (!Context.IsPrivate)
                await Context.Message.DeleteAsync();
            if (await _engine._extData.UserInfoExists(Context.User.Id.ToString()))
            {
                await Context.User.SendMessageAsync("User already registered.");
                return;
            }
            await ReplyAsync("Under Construction...");
        }
    }
}
