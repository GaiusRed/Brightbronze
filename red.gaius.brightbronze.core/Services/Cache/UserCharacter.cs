using System.Collections.Generic;
using System.Threading.Tasks;
using red.gaius.brightbronze.core.Models;

namespace red.gaius.brightbronze.core.Services
{
    public partial class Cache
    {
        public async Task<bool> UserCharactersExist(string userId)
        {
            return (await GetUserCharacters(userId)).Count > 0;
        }

        public async Task<List<UserCharacter>> GetUserCharacters(string userId)
        {
            List<UserCharacter> UserCharacters = _cache[$"UserCharacters_{userId}"] as List<UserCharacter>;
            if (_cache[$"UserCharacters_{userId}"] == null)
            {
                UserCharacters = await _datastore.GetUserCharacters(userId);
                _cache.Set($"UserCharacters_{userId}", UserCharacters, GetPolicy());
            }
            return UserCharacters;
        }

        public async Task<bool> SetUserCharacter(UserCharacter character)
        {
            bool result = await _datastore.SetUserCharacter(character);
            if (result)
            {
                List<UserCharacter> UserCharacters =
                    await _datastore.GetUserCharacters(character.userId);
                _cache.Set($"UserCharacters_{character.userId}", UserCharacters, GetPolicy());
            }
            return result;
        }

        public async Task<bool> DeleteUserCharacter(UserCharacter character)
        {
            bool result = await _datastore.DeleteUserCharacter(character);
            if (result)
            {
                List<UserCharacter> UserCharacters =
                    await _datastore.GetUserCharacters(character.userId);
                _cache.Set($"UserCharacters_{character.userId}", UserCharacters, GetPolicy());
            }
            return result;
        }
    }
}
