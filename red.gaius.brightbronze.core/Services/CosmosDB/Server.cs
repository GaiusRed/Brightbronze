using Azure.Cosmos;
using System.Threading.Tasks;
using red.gaius.brightbronze.core.Models;

namespace red.gaius.brightbronze.core.Services
{
    public partial class CosmosDB
    {
        public async Task<bool> DeleteServer(string serverId)
        {
            QueryDefinition serverInfoQuery = new QueryDefinition(
                $"SELECT TOP 1 * FROM c WHERE c.serverId = '{serverId}' AND c.structure = 'info'");
            try
            {
                await foreach (ServerInfo serverInfo in
                    _cServers.GetItemQueryIterator<ServerInfo>(serverInfoQuery))
                    await _cServers.DeleteItemAsync<ServerInfo>(
                        serverInfo.id, new PartitionKey(serverInfo.serverId));
                return true;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex, ex.Message);
                return false;
            }
        }
    }
}