using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BTCGatewayAPI.Common.Authentication
{
    public class AccessTokenService : IAccessTokenService
    {
        private readonly IDistributedCache _cache;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IOptions<JwtOptions> _jwtOptions;

        public AccessTokenService(IDistributedCache cache,
                IHttpContextAccessor httpContextAccessor,
                IOptions<JwtOptions> jwtOptions)
        {
            _cache = cache;
            _httpContextAccessor = httpContextAccessor;
            _jwtOptions = jwtOptions;
        }

        public Task<bool> IsTokenAlive()
            => IsActiveAsync(GetCurrent());

        public Task DeactivateCurrentAsync(string userId)
            => DeactivateAsync(userId, GetCurrent());

        public async Task<bool> IsActiveAsync(string token)
            => string.IsNullOrWhiteSpace(await _cache.GetStringAsync(GetKey(token)));

        public Task DeactivateAsync(string userId, string token)
        {
            return _cache.SetStringAsync(GetKey(token),
                    "deactivated", new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow =
                            TimeSpan.FromMinutes(_jwtOptions.Value.ExpiryMinutes)
                    });
        }

        private string GetCurrent()
        {
            var authorizationHeader = _httpContextAccessor
                .HttpContext.Request.Headers["authorization"];

            return authorizationHeader == StringValues.Empty
                ? string.Empty
                : authorizationHeader.Single().Split(' ').Last();
        }

        private static string GetKey(string token)
            => $"tokens:{token}";
    }
}
