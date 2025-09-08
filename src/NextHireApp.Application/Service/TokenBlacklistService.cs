using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;

namespace NextHireApp.Service
{
    public class TokenBlacklistService : ITokenBlacklistService, ITransientDependency
    {
        private readonly IDistributedCache<string> _cache;
        private const string Prefix = "jwt:blacklist:";

        public Task BlacklistAsync(string jti, TimeSpan ttl) =>
            _cache.SetAsync(Prefix + jti, "1", new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = ttl });

        public async Task<bool> IsBlacklistedAsync(string jti) =>
            await _cache.GetAsync(Prefix + jti) is not null;
    }
}
