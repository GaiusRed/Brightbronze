using Azure.Cosmos;
using Microsoft.Extensions.Options;
using red.gaius.brightbronze.core.Models;
using Serilog;

namespace red.gaius.brightbronze.core.Services
{
    public partial class CosmosDB : IDatastore
    {
        readonly ILogger _logger;
        readonly CosmosDBSettings _settings;
        readonly CosmosClient _client;

        const string containerName_Servers = "servers";
        readonly CosmosContainer _cServers;

        const string containerName_User = "users";
        readonly CosmosContainer _cUsers;

        public CosmosDB(ILogger logger, IOptions<CosmosDBSettings> settings)
        {
            _logger = logger;
            _settings = settings.Value;
            _client = new CosmosClient(_settings.EndpointUrl, _settings.AuthorizationKey);
            _cServers = _client.GetContainer(_settings.DatabaseId, containerName_Servers);
            _cUsers = _client.GetContainer(_settings.DatabaseId, containerName_User);
        }
    }
}
