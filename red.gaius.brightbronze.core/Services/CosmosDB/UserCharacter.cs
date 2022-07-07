using Azure.Cosmos;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using red.gaius.brightbronze.core.Models;
using System.Collections.Generic;
using System;

namespace red.gaius.brightbronze.core.Services
{
    public partial class CosmosDB
    {
        public async Task<bool> SetUserCharacter(UserCharacter character)
        {
            try
            {
                bool dupeNickname = false;
                foreach (UserCharacter c in await GetUserCharacters(character.userId))
                    if (c.nickname.Equals(character.nickname))
                        dupeNickname = true;
                if (dupeNickname)
                {
                    // Add Context
                    _logger.LogInformation(
                        "Unable to create UserCharacter due to duplicate nickname within User's roster.");
                    return false;
                }

                ItemResponse<UserCharacter> response =
                    await _cUsers.UpsertItemAsync<UserCharacter>(character,
                        new PartitionKey(character.userId));
                _logger.LogInformation($"UserCharacter for { character.userId } successfully " +
                                       $"upserted with DB id: { response.Value.id }");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return false;
            }
        }

        public async Task<List<UserCharacter>> GetUserCharacters(string userId)
        {
            List<UserCharacter> characters = new List<UserCharacter>();
            QueryDefinition query = new QueryDefinition(
                $"SELECT * FROM c WHERE c.userId = '{userId}' AND c.structure = 'character'");
            try
            {
                await foreach (UserCharacter UserCharacter in
                    _cUsers.GetItemQueryIterator<UserCharacter>(query))
                    characters.Add(UserCharacter);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return characters;
        }

        public async Task<bool> DeleteUserCharacter(UserCharacter character)
        {
            try
            {
                List<UserCharacter> characters = await GetUserCharacters(character.userId);
                if (characters.Count <= 1)
                    throw new ApplicationException("Delete failed: at least one character required in roster.");

                await _cUsers.DeleteItemAsync<UserCharacter>(
                    character.id, new PartitionKey(character.userId));
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return false;
        }
    }
}