using System;
using System.Runtime.Caching;
using Microsoft.Extensions.Options;
using red.gaius.brightbronze.core.Models;

namespace red.gaius.brightbronze.core.Services
{
    public partial class Cache : IDatastore
    {
        readonly IDatastore _datastore;
        readonly CacheSettings _settings;

        readonly ObjectCache _cache;

        public Cache(IOptions<CacheSettings> settings, IDatastore datastore)
        {
            _settings = settings.Value;
            _datastore = datastore;

            _cache = MemoryCache.Default;
        }

        public CacheItemPolicy GetPolicy()
        {
            return new CacheItemPolicy()
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(_settings.ExpiryInMinutes)
            };
        }
    }
}
