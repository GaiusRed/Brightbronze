using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using red.gaius.brightbronze.core.Models;

namespace red.gaius.brightbronze.core.Services
{
    public partial class Engine
    {
        readonly ILogger<Engine> _logger;
        EngineSettings _settings;
        public readonly Cache _extData;

        public Engine(ILogger<Engine> logger,
                      IOptions<EngineSettings> settings,
                      Cache cache)
        {
            _logger = logger;
            _settings = settings.Value;
            _extData = cache;
        }
    }
}
