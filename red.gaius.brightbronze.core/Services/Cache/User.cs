using System.Threading.Tasks;

namespace red.gaius.brightbronze.core.Services
{
    public partial class Cache
    {
        public async Task<bool> DeleteUser(string userId)
        {
            bool result = await _datastore.DeleteUser(userId);
            if (result)
            {
                _cache.Remove($"userInfo_{userId}");
                _cache.Remove($"userWallet_{userId}");
            }
            return result;
        }
    }
}
