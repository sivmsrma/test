using System;
using System.Collections.Concurrent;

namespace Terret_Billing.Application.Security
{
    public class RateLimiter
    {
        private readonly ConcurrentDictionary<string, DateTime> _accessTimes = new();
        private readonly TimeSpan _limit;
        public RateLimiter(TimeSpan limit) { _limit = limit; }
        public bool IsAllowed(string key)
        {
            var now = DateTime.UtcNow;
            return _accessTimes.AddOrUpdate(key, now, (k, last) =>
                (now - last) > _limit ? now : last) == now;
        }
    }
}
