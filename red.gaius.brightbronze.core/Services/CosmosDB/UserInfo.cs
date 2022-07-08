using Azure.Cosmos;
using System.Threading.Tasks;
using red.gaius.brightbronze.core.Models;

namespace red.gaius.brightbronze.core.Services
{
    public partial class CosmosDB
    {
        public async Task<bool> SetUserInfo(UserInfo userInfo)
        {
            try
            {
                ItemResponse<UserInfo> response =
                    await _cUsers.UpsertItemAsync<UserInfo>(userInfo,
                        new PartitionKey(userInfo.userId));
                _logger.Information($"UserInfo for { userInfo.userId } successfully " +
                                    $"upserted with DB id: { response.Value.id }");
                return true;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex, ex.Message);
                return false;
            }
        }

        public async Task<UserInfo> GetUserInfo(string userId)
        {
            QueryDefinition query = new QueryDefinition(
                $"SELECT TOP 1 * FROM c WHERE c.userId = '{userId}' AND c.structure = 'info'");
            try
            {
                await foreach (UserInfo userInfo in
                    _cUsers.GetItemQueryIterator<UserInfo>(query))
                    return userInfo;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex, ex.Message);
            }
            return null;
        }
    }
}