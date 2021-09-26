using System.Threading.Tasks;
using red.gaius.brightbronze.core.Models;

namespace red.gaius.brightbronze.core.Services
{
    public interface IDatastore
    {
        Task<bool> SetServerInfo(ServerInfo serverInfo);
        Task<ServerInfo> GetServerInfo(string serverId);
        Task<bool> DeleteServer(string serverId);

        Task<bool> SetUserInfo(UserInfo userInfo);
        Task<UserInfo> GetUserInfo(string userId);
        Task<bool> SetUserWallet(UserWallet userWallet);
        Task<UserWallet> GetUserWallet(string userId);
        Task<bool> DeleteUser(string userId);
    }
}