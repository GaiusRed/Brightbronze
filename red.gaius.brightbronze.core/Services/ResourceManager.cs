using System.Collections.Generic;
using Microsoft.Extensions.Options;
using red.gaius.brightbronze.core.Models;

namespace red.gaius.brightbronze.core.Services
{
    public partial class ResourceManager
    {
        readonly ResourceManagerSettings _settings;

        public readonly Dictionary<string, Quest> _quests;
        public readonly Dictionary<string, QuestItem> _questItems;

        public ResourceManager(IOptions<ResourceManagerSettings> settings)
        {
            _settings = settings.Value;

            _quests = new Dictionary<string, Quest>();
            _questItems = new Dictionary<string, QuestItem>();
        }
    }
}