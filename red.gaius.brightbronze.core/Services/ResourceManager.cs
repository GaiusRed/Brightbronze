using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using red.gaius.brightbronze.core.Models;
using YamlDotNet.Serialization;

namespace red.gaius.brightbronze.core.Services
{
    public partial class ResourceManager
    {
        readonly ResourceManagerSettings _settings;
        readonly ILogger<ResourceManager> _logger;
        readonly IDeserializer _yml;

        readonly string _pathDir = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
        readonly string _pathScripts;

        public ResourceManager(IOptions<ResourceManagerSettings> settings, ILogger<ResourceManager> logger)
        {
            _settings = settings.Value;
            _logger = logger;
            _yml = new DeserializerBuilder().Build();

            _pathScripts = _pathDir + _settings.Path_ScriptsYml;
        }

        public async Task<string> GetScript(string scriptName)
        {
            string s = string.Empty;
            try
            {
                List<Script> scripts = _yml.Deserialize<List<Script>>(
                    await File.ReadAllTextAsync(_pathScripts));
                s = scripts.First(_ => _.name.Equals(scriptName)).script;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                s = string.Empty;
            }
            return s;
        }
    }
}
