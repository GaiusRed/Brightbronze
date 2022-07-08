using Microsoft.Extensions.Options;
using red.gaius.brightbronze.core.Models;
using Serilog;

namespace red.gaius.brightbronze.core.Services
{
    public partial class Engine
    {
        readonly ILogger _logger;
        readonly EngineSettings _settings;

        public Engine(ILogger logger,
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
