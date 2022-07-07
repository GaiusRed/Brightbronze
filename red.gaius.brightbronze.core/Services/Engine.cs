using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using red.gaius.brightbronze.core.Models;

namespace red.gaius.brightbronze.core.Services
{
    public partial class Engine
    {
        readonly ILogger<Engine> _logger;
        readonly EngineSettings _settings;

        public Engine(ILogger<Engine> logger,
                      IOptions<EngineSettings> settings,
                      Cache cache)
        {
            _logger = logger;
            _settings = settings.Value;
            data = cache;
        }

        public Cache data { get; }
    }
}
