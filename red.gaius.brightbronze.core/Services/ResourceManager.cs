using System.Collections.Generic;
using Microsoft.Extensions.Options;
using red.gaius.brightbronze.core.Models;

namespace red.gaius.brightbronze.core.Services
{
    public partial class ResourceManager
    {
        readonly ResourceManagerSettings _settings;

        public ResourceManager(IOptions<ResourceManagerSettings> settings)
        {
            _settings = settings.Value;
        }
    }
}