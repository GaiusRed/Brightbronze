using Azure.Cosmos;
using System.Threading.Tasks;
using red.gaius.brightbronze.core.Models;

namespace red.gaius.brightbronze.core.Services
{
    public partial class CosmosDB
    {
        public async Task<bool> SetServerInfo(ServerInfo serverInfo)
        {
            try
            {
                ItemResponse<ServerInfo> response =
                    await _cServers.UpsertItemAsync<ServerInfo>(serverInfo,
                        new PartitionKey(serverInfo.serverId));
                _logger.Information($"ServerInfo for { serverInfo.serverId } successfully " +
                                       $"upserted with DB id: { response.Value.id }");
                return true;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex, ex.Message);
                return false;
            }
        }

        public async Task<ServerInfo> GetServerInfo(string serverId)
        {
            QueryDefinition query = new QueryDefinition(
                $"SELECT TOP 1 * FROM c WHERE c.serverId = '{serverId}' AND c.structure = 'info'");
            try
            {
                await foreach (ServerInfo serverInfo in
                    _cServers.GetItemQueryIterator<ServerInfo>(query))
                    return serverInfo;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex, ex.Message);
            }
            return null;
        }
    }
}