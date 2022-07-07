using System.Threading.Tasks;

namespace red.gaius.brightbronze.core.Services
{
    public partial class Cache
    {
        public async Task<string> GetScript(string scriptName)
        {
            string s = _cache[$"script_{scriptName}"] as string;
            if (string.IsNullOrEmpty(_cache[$"script_{scriptName}"] as string))
            {
                s = await _resources.GetScript(scriptName);
                if (string.IsNullOrEmpty(s))
                    _cache.Set($"script_{scriptName}", s, GetPolicy());
            }
            return s;
        }
    }
}
