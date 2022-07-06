using System.Threading.Tasks;
using red.gaius.brightbronze.core.Models;

namespace red.gaius.brightbronze.core.Services
{
    public partial class Cache
    {
        public async Task<bool> UserInfoExists(string userId)
        {
            if (await GetUserInfo(userId) == null)
                return false;
            return true;
        }

        public async Task<UserInfo> GetUserInfo(string userId)
        {
            UserInfo userInfo = _cache[$"userInfo_{userId}"] as UserInfo;
            if (_cache[$"userInfo_{userId}"] == null)
            {
                userInfo = await _datastore.GetUserInfo(userId);
                if (userInfo != null)
                    _cache.Set($"userInfo_{userInfo.userId}", userInfo, GetPolicy());
            }
            return userInfo;
        }

        public async Task<bool> SetUserInfo(UserInfo userInfo)
        {
            bool result = await _datastore.SetUserInfo(userInfo);
            if (result) _cache.Set($"userInfo_{userInfo.userId}", userInfo, GetPolicy());
            return result;
        }
    }
}
