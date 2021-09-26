using System.Threading.Tasks;

namespace red.gaius.brightbronze.core.Services
{
    public partial class Cache
    {
        public async Task<bool> DeleteServer(string serverId)
        {
            bool result = await _datastore.DeleteServer(serverId);
            if (result)
            {
                _cache.Remove($"serverInfo_{serverId}");
            }
            return result;
        }
    }
}
