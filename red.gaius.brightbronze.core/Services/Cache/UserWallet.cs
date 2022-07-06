using System.Threading.Tasks;
using red.gaius.brightbronze.core.Models;

namespace red.gaius.brightbronze.core.Services
{
    public partial class Cache
    {
        public async Task<bool> UserWalletExists(string userId)
        {
            if (await GetUserWallet(userId) == null)
                return false;
            return true;
        }

        public async Task<UserWallet> GetUserWallet(string userId)
        {
            UserWallet userWallet = _cache[$"userWallet_{userId}"] as UserWallet;
            if (_cache[$"userInfo_{userId}"] == null)
            {
                userWallet = await _datastore.GetUserWallet(userId);
                if (userWallet != null)
                    _cache.Set($"userWallet_{userWallet.userId}", userWallet, GetPolicy());
            }
            return userWallet;
        }

        public async Task<bool> SetUserWallet(UserWallet userWallet)
        {
            bool result = await _datastore.SetUserWallet(userWallet);
            if (result) _cache.Set($"userWallet_{userWallet.userId}", userWallet, GetPolicy());
            return result;
        }
    }
}
