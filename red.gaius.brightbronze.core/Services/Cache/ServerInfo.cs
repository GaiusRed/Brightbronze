using System.Threading.Tasks;
using red.gaius.brightbronze.core.Models;

namespace red.gaius.brightbronze.core.Services
{
    public partial class Cache
    {
        public async Task<bool> ServerInfoExists(string serverId)
        {
            if (await GetServerInfo(serverId) == null)
                return false;
            return true;
        }

        public async Task<ServerInfo> GetServerInfo(string serverId)
        {
            ServerInfo serverInfo = _cache[$"serverInfo_{serverId}"] as ServerInfo;
            if (_cache[$"serverInfo_{serverId}"] == null)
            {
                serverInfo = await _datastore.GetServerInfo(serverId);
                _cache.Set($"serverInfo_{serverInfo.serverId}", serverInfo, GetPolicy());
            }
            return serverInfo;
        }

        public async Task<bool> SetServerInfo(ServerInfo serverInfo)
        {
            bool result = await _datastore.SetServerInfo(serverInfo);
            if (result) _cache.Set($"serverInfo_{serverInfo.serverId}", serverInfo, GetPolicy());
            return result;
        }
    }
}
