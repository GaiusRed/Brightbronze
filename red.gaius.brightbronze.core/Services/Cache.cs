using System;
using System.Runtime.Caching;
using Microsoft.Extensions.Options;
using red.gaius.brightbronze.core.Models;

namespace red.gaius.brightbronze.core.Services
{
    public partial class Cache : IDatastore
    {
        readonly CacheSettings _settings;
        readonly IDatastore _datastore;
        readonly ResourceManager _resources;

        readonly ObjectCache _cache;

        public Cache(IOptions<CacheSettings> settings, IDatastore datastore, ResourceManager resourceManager)
        {
            _settings = settings.Value;
            _datastore = datastore;
            _resources = resourceManager;

            _cache = MemoryCache.Default;
        }

        public CacheItemPolicy GetPolicy()
        {
            return new CacheItemPolicy()
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(_settings.ExpiryInMinutes)
            };
        }

        public void Flush()
        {
            MemoryCache.Default.Dispose();
        }
    }
}
