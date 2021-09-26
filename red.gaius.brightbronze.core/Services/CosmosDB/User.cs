using Azure.Cosmos;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using red.gaius.brightbronze.core.Models;

namespace red.gaius.brightbronze.core.Services
{
    public partial class CosmosDB
    {
        public async Task<bool> DeleteUser(string userId)
        {
            QueryDefinition userInfoQuery = new QueryDefinition(
                $"SELECT TOP 1 * FROM c WHERE c.userId = '{userId}' AND c.structure = 'info'");
            QueryDefinition userWalletQuery = new QueryDefinition(
                $"SELECT TOP 1 * FROM c WHERE c.userId = '{userId}' AND c.structure = 'wallet'");
            try
            {
                await foreach (UserInfo userInfo in
                    _cUsers.GetItemQueryIterator<UserInfo>(userInfoQuery))
                    await _cUsers.DeleteItemAsync<UserInfo>(
                        userInfo.id, new PartitionKey(userInfo.userId));
                await foreach (UserWallet userWallet in
                    _cUsers.GetItemQueryIterator<UserWallet>(userWalletQuery))
                    await _cUsers.DeleteItemAsync<UserWallet>(
                        userWallet.id, new PartitionKey(userWallet.userId));
                return true;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return false;
            }
        }
    }
}