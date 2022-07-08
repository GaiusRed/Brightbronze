using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using red.gaius.brightbronze.core.Models;

namespace red.gaius.brightbronze.core.Services
{
    public partial class ResourceManager
    {
        public async Task<string> GetScript(string scriptName)
        {
            string s = string.Empty;
            try
            {
                List<Script> scripts = _yml.Deserialize<List<Script>>(
                    await File.ReadAllTextAsync(_pathScripts));
                s = scripts.First(_ => _.name.Equals(scriptName)).value;
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
