using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using ErpShowroom.Application.Common.Interfaces;

namespace ErpShowroom.Infrastructure.Caching;

public class RedisCacheService : ICacheService
{
    private readonly IConnectionMultiplexer _connectionMultiplexer;
    private readonly IDatabase _database;
    private readonly ILogger<RedisCacheService> _logger;

    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    private readonly TimeSpan _defaultAbsoluteExpiration = TimeSpan.FromMinutes(10);
    private readonly TimeSpan _defaultSlidingExpiration = TimeSpan.FromMinutes(2);

    public RedisCacheService(IConnectionMultiplexer connectionMultiplexer, ILogger<RedisCacheService> logger)
    {
        _connectionMultiplexer = connectionMultiplexer;
        _database = _connectionMultiplexer.GetDatabase();
        _logger = logger;
    }

    public async Task<T?> GetAsync<T>(string key, CancellationToken ct = default)
    {
        try
        {
            var cachedValue = await _database.StringGetAsync(key);

            if (cachedValue.HasValue)
            {
                await _database.KeyExpireAsync(key, _defaultSlidingExpiration);
                return JsonSerializer.Deserialize<T>(cachedValue.ToString(), _jsonOptions);
            }
        }
        catch (RedisConnectionException ex)
        {
            _logger.LogWarning(ex, "Redis connection failed while getting key {Key}. Falling back to database.", key);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Error getting key {Key} from Redis.", key);
        }

        return default;
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? absoluteExpiration = null, TimeSpan? slidingExpiration = null, CancellationToken ct = default)
    {
        try
        {
            var serializedValue = JsonSerializer.Serialize(value, _jsonOptions);
            var expiration = absoluteExpiration ?? slidingExpiration ?? _defaultAbsoluteExpiration;

            await _database.StringSetAsync(key, serializedValue, expiration);
        }
        catch (RedisConnectionException ex)
        {
            _logger.LogWarning(ex, "Redis connection failed while setting key {Key}.", key);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Error setting key {Key} to Redis.", key);
        }
    }

    public async Task RemoveAsync(string key, CancellationToken ct = default)
    {
        try
        {
            await _database.KeyDeleteAsync(key);
        }
        catch (RedisConnectionException ex)
        {
            _logger.LogWarning(ex, "Redis connection failed while removing key {Key}.", key);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Error removing key {Key} from Redis.", key);
        }
    }

    public async Task RemoveByPrefixAsync(string prefix, CancellationToken ct = default)
    {
        try
        {
            foreach (var endpoint in _connectionMultiplexer.GetEndPoints())
            {
                var server = _connectionMultiplexer.GetServer(endpoint);
                if (!server.IsConnected) continue;

                var keys = server.Keys(pattern: prefix + "*");
                foreach (var key in keys)
                {
                    await _database.KeyDeleteAsync(key);
                }
            }
        }
        catch (RedisConnectionException ex)
        {
            _logger.LogWarning(ex, "Redis connection failed while removing keys with prefix {Prefix}.", prefix);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Error removing keys with prefix {Prefix} from Redis.", prefix);
        }
    }
}
