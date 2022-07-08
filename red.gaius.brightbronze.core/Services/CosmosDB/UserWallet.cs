using Azure.Cosmos;
using System.Threading.Tasks;
using red.gaius.brightbronze.core.Models;

namespace red.gaius.brightbronze.core.Services
{
    public partial class CosmosDB
    {
        public async Task<bool> SetUserWallet(UserWallet userWallet)
        {
            try
            {
                ItemResponse<UserWallet> response =
                    await _cUsers.UpsertItemAsync<UserWallet>(userWallet,
                        new PartitionKey(userWallet.userId));
                _logger.Information($"UserWallet for { userWallet.userId } successfully " +
                                    $"upserted with DB id: { response.Value.id }");
                return true;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex, ex.Message);
                return false;
            }
        }

        public async Task<UserWallet> GetUserWallet(string userId)
        {
            QueryDefinition query = new QueryDefinition(
                $"SELECT TOP 1 * FROM c WHERE c.userId = '{userId}' AND c.structure = 'wallet'");
            try
            {
                await foreach (UserWallet userWallet in
                    _cUsers.GetItemQueryIterator<UserWallet>(query))
                    return userWallet;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex, ex.Message);
            }
            return null;
        }
    }
}