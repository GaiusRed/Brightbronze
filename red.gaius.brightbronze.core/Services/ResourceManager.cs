using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Serilog;
using red.gaius.brightbronze.core.Models;
using YamlDotNet.Serialization;

namespace red.gaius.brightbronze.core.Services
{
    public partial class ResourceManager
    {
        readonly ResourceManagerSettings _settings;
        readonly ILogger _logger;
        readonly IDeserializer _yml;

        readonly string _pathScripts;

        public ResourceManager(IOptions<ResourceManagerSettings> settings, ILogger logger)
        {
            _settings = settings.Value;
            _logger = logger;
            _yml = new DeserializerBuilder().Build();

            string pathDir = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            _pathScripts = pathDir + _settings.Path_ScriptsYml;
        }
    }
}
