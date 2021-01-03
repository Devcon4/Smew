using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SmewApi.Infrastructure;
using StackExchange.Redis;

namespace Smew.Infrastructure
{
    public interface IRedisProvider {
        IConnectionMultiplexer redis {get;init;}
    }
    
    public class RedisProvider: IRedisProvider {
        public IConnectionMultiplexer redis {get;init;}

        public RedisProvider(ILogger<RedisProvider> logger, IOptions<RedisConfig> redisConfig) {
            redis = ConnectionMultiplexer.Connect(ConfigurationOptions.Parse(redisConfig.Value.ConnectionString));
        }
    }
}