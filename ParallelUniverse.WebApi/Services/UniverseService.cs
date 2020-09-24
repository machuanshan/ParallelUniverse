using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParallelUniverse.WebApi
{
    public class UniverseService
    {
        private readonly IMemoryCache _linkCache;

        public UniverseService(IMemoryCache memoryCache)
        {
            _linkCache = memoryCache;
        }

        public string GeneratePortalLink(string link, TimeSpan duration)
        {
            while (true)
            {
                var key = Utils.GetRandomString(4);

                if (!_linkCache.TryGetValue(key, out _))
                {
                    _linkCache.Set(key, link, duration);
                    return QueryHelpers.AddQueryString(link, "key", key);
                }
            }            
        }
    }
}
