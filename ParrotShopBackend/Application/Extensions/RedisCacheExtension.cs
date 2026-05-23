
using System.Diagnostics;
using System.Reflection;
using ParrotShopBackend.Application.DTO;
using ParrotShopBackend.Domain;
using StackExchange.Redis;

namespace ParrotShopBackend.Application.Extensions;


public class RedisCacheExtension
{
    private IConnectionMultiplexer _muxer;
    public IDatabase _redis;
    private IServer _srv;
    public RedisCacheExtension(IConnectionMultiplexer muxer)
    {
        _muxer = muxer;
        _redis = _muxer.GetDatabase();
        _srv = _muxer.GetServer(_muxer.GetEndPoints().FirstOrDefault()!);
        _srv.FlushAllDatabases();
    }


    public async Task SetStringAsync(string key, string value, TimeSpan? ttl = null)
    {
        await _redis.StringSetAsync(key, value, (Expiration)ttl!);
    }

    public async Task<string?> GetStringAsync(string key)
    {
        return await _redis.StringGetAsync(key);
    }

    public async Task SADD(string key, long value)
    {
        await _redis.SetAddAsync(key, value);
    }

    public async Task SREM(string key, long value)
    {
        await _redis.SetRemoveAsync(key, value);
    }

    public async Task<List<long>> ApplyFilterAsync(ParrotFilterDTO pfDTO)
    {
        RedisKey[] keys = PrepareSetKeys(pfDTO);
        HashSet<long>? finalResults = null;

        if (keys.Length > 0)
        {
            RedisValue[] filteredByTrait = await _redis.SetCombineAsync(SetOperation.Intersect, keys);
            if (filteredByTrait.Length == 0) return [];

            finalResults = [.. filteredByTrait.Select(x => (long)x)];
        }
        if (pfDTO.PriceFrom.HasValue || pfDTO.PriceTo.HasValue)
        {
            RedisValue[] filteredByPrice = await _redis.SortedSetRangeByScoreAsync(
                "Set_Price",
                pfDTO.PriceFrom ?? double.NegativeInfinity,
                pfDTO.PriceTo ?? double.PositiveInfinity);

            if (filteredByPrice.Length == 0) return [];

            HashSet<long> byPrice = [.. filteredByPrice.Select(x => (long)x)];

            if (finalResults == null)
            {
                finalResults = byPrice;
            }
            else
            {
                finalResults.IntersectWith(byPrice);
                if (finalResults.Count == 0) return [];
            }
        }

        if (finalResults == null)
        {
            return [.. (await _redis.SetMembersAsync("Set_AllParrots")).Select(x => (long)x)];
        }

        return [.. finalResults];
    }

    private List<string> GetParrotRedisKeys(Parrot p)
    {
        var keys = new List<string>();

        if (p.Traits is not null)
        {
            if (p.Traits.KidSafety.HasValue) keys.Add($"Set_KidSafety_{p.Traits.KidSafety.Value}");
            if (p.Traits.CareComplexity.HasValue) keys.Add($"Set_CareComplexity_{p.Traits.CareComplexity.Value}");
            if (p.Traits.ChewingRisk.HasValue) keys.Add($"Set_ChewingRisk_{p.Traits.ChewingRisk.Value}");
            if (p.Traits.NoiseLevel.HasValue) keys.Add($"Set_NoiseLevel_{p.Traits.NoiseLevel.Value}");
            if (p.Traits.Size.HasValue) keys.Add($"Set_Size_{p.Traits.Size.Value}");
            if (p.Traits.Sociability.HasValue) keys.Add($"Set_Sociability_{p.Traits.Sociability.Value}");
            if (p.Traits.Talkativeness.HasValue) keys.Add($"Set_Talkativeness_{p.Traits.Talkativeness.Value}");
            if (p.Traits.Trainability.HasValue) keys.Add($"Set_Trainability_{p.Traits.Trainability.Value}");
        }

        foreach (Color col in Enum.GetValues<Color>())
        {
            if (p.ColorType.HasFlag(col))
            {
                keys.Add($"Set_Color_{col}");
            }
        }

        keys.Add($"Set_Gender_{p.GenderType}");
        keys.Add($"Set_Species_{p.SpeciesType}");

        return keys;
    }

    public async Task AddParrotToCacheAsync(Parrot p)
    {
        if (p == null) return;

        var redisBatch = _redis.CreateBatch();
        var redisTasks = new List<Task>();

        redisTasks.Add(redisBatch.SetAddAsync("Set_AllParrots", p.Id));

        var keys = GetParrotRedisKeys(p);
        foreach (var key in keys)
        {
            redisTasks.Add(redisBatch.SetAddAsync(key, p.Id));
        }

        redisTasks.Add(redisBatch.SortedSetAddAsync("Set_Price", p.Id, (double)p.Price));

        redisBatch.Execute();
        await Task.WhenAll(redisTasks);
    }

    public async Task RemoveParrotFromCacheAsync(Parrot p)
    {
        if (p == null) return;

        var redisBatch = _redis.CreateBatch();
        var redisTasks = new List<Task>();

        redisTasks.Add(redisBatch.SetRemoveAsync("Set_AllParrots", p.Id));

        var keys = GetParrotRedisKeys(p);
        foreach (var key in keys)
        {
            redisTasks.Add(redisBatch.SetRemoveAsync(key, p.Id));
        }

        redisTasks.Add(redisBatch.SortedSetRemoveAsync("Set_Price", p.Id));

        redisBatch.Execute();
        await Task.WhenAll(redisTasks);
    }

    public async Task UpdateParrotInCacheAsync(Parrot oldParrot, Parrot newParrot)
    {
        var redisBatch = _redis.CreateBatch();
        var redisTasks = new List<Task>();

        if (oldParrot != null)
        {
            var oldKeys = GetParrotRedisKeys(oldParrot);
            foreach (var key in oldKeys)
            {
                redisTasks.Add(redisBatch.SetRemoveAsync(key, oldParrot.Id));
            }
            redisTasks.Add(redisBatch.SortedSetRemoveAsync("Set_Price", oldParrot.Id));
        }

        if (newParrot != null)
        {
            redisTasks.Add(redisBatch.SetAddAsync("Set_AllParrots", newParrot.Id)); // Ensures it exists
            var newKeys = GetParrotRedisKeys(newParrot);
            foreach (var key in newKeys)
            {
                redisTasks.Add(redisBatch.SetAddAsync(key, newParrot.Id));
            }
            redisTasks.Add(redisBatch.SortedSetAddAsync("Set_Price", newParrot.Id, (double)newParrot.Price));
        }
        redisBatch.Execute();
        await Task.WhenAll(redisTasks);
    }

    public RedisKey[] PrepareSetKeys<T>(T obj)
    {
        if (obj is null) return Array.Empty<RedisKey>();
        PropertyInfo[] PList = obj.GetType().GetProperties();
        List<string> RedisSets = [];
        foreach (PropertyInfo pi in PList)
        {
            if (pi.Name == "PriceFrom" || pi.Name == "PriceTo" || pi.Name == "AscendingPrice")
                continue;
            if (pi.GetValue(obj) is null) continue;
            if (pi.Name == "Color")
            {
                foreach (Color col in pi.GetValue(obj) as List<Color>)
                {
                    RedisSets.Add($"Set_{pi.Name}_{col}");
                }
                continue;
            }
            RedisSets.Add($"Set_{pi.Name}_{pi.GetValue(obj)}");
        }
        return RedisSets.Select(x => (RedisKey)x).ToArray();
    }

    private Dictionary<int, string> levels = new Dictionary<int, string>{
        {0, "Low"},
        {1, "Mid"},
        {2, "High"}
    };

    private Dictionary<int, string> size = new Dictionary<int, string>{
        {1, "Small"},
        {2, "Medium"},
        {3, "Large"}
    };

    private Dictionary<int, string> kidSafety = new Dictionary<int, string>{
        {0, "Yes"},
        {2, "No"},
        {1, "Cautious"}
    };

    public async Task<List<long>> GetRecommendationsAsync(ParrotFilterDTO pfDTO)
    {
        HashSet<long>? finalResults = null;

        async Task ApplyRegressiveFilterAsync(string prefix, int maxValue, int minValue = 0)
        {
            if (finalResults != null && finalResults.Count == 0) return;

            var unionIds = new HashSet<long>();

            for (int i = minValue; i <= maxValue; i++)
            {
                RedisValue[] members;
                if (prefix == "Set_Size")
                {
                    members = await _redis.SetMembersAsync($"{prefix}_{size[i]}");
                }
                else if (prefix == "Set_KidSafety")
                {
                    members = await _redis.SetMembersAsync($"{prefix}_{kidSafety[i]}");
                }
                else
                {
                    members = await _redis.SetMembersAsync($"{prefix}_{levels[i]}");
                }

                unionIds.UnionWith(members.Select(x => (long)x));
            }

            if (finalResults == null)
                finalResults = unionIds;
            else
                finalResults.IntersectWith(unionIds);
        }

        if (pfDTO.Size.HasValue) await ApplyRegressiveFilterAsync("Set_Size", (int)pfDTO.Size.Value + 1, 1);
        if (pfDTO.NoiseLevel.HasValue) await ApplyRegressiveFilterAsync("Set_NoiseLevel", (int)pfDTO.NoiseLevel.Value);
        if (pfDTO.Sociability.HasValue) await ApplyRegressiveFilterAsync("Set_Sociability", (int)pfDTO.Sociability.Value);
        if (pfDTO.Talkativeness.HasValue) await ApplyRegressiveFilterAsync("Set_Talkativeness", (int)pfDTO.Talkativeness.Value);
        if (pfDTO.Trainability.HasValue) await ApplyRegressiveFilterAsync("Set_Trainability", (int)pfDTO.Trainability.Value);
        if (pfDTO.ChewingRisk.HasValue) await ApplyRegressiveFilterAsync("Set_ChewingRisk", (int)pfDTO.ChewingRisk.Value);
        if (pfDTO.CareComplexity.HasValue) await ApplyRegressiveFilterAsync("Set_CareComplexity", (int)pfDTO.CareComplexity.Value);
        if (pfDTO.KidSafety.HasValue) await ApplyRegressiveFilterAsync("Set_KidSafety", (int)pfDTO.KidSafety.Value);

        if (finalResults != null && finalResults.Count == 0) return [];

        async Task ApplyExactMatchAsync(string key)
        {
            if (finalResults != null && finalResults.Count == 0) return;
            var members = await _redis.SetMembersAsync(key);
            var ids = new HashSet<long>(members.Select(x => (long)x));

            if (finalResults == null) finalResults = ids;
            else finalResults.IntersectWith(ids);
        }

        if (pfDTO.Species.HasValue) await ApplyExactMatchAsync($"Set_Species_{pfDTO.Species.Value}");
        if (pfDTO.Gender.HasValue) await ApplyExactMatchAsync($"Set_Gender_{pfDTO.Gender.Value}");

        if (pfDTO.Color != null && pfDTO.Color.Count > 0)
        {
            var colorUnion = new HashSet<long>();
            foreach (var col in pfDTO.Color)
            {
                var members = await _redis.SetMembersAsync($"Set_Color_{col.ToString()}");
                colorUnion.UnionWith(members.Select(x => (long)x));
            }

            if (finalResults == null)
            {
                if (colorUnion.Count > 0)
                {
                    finalResults = colorUnion;
                }
            }
            else
            {
                var testIntersection = new HashSet<long>(finalResults);

                testIntersection.IntersectWith(colorUnion);
                if (testIntersection.Count > 0)
                {
                    finalResults = testIntersection;
                }
            }
        }

        if (pfDTO.PriceFrom.HasValue || pfDTO.PriceTo.HasValue)
        {
            var priceMembers = await _redis.SortedSetRangeByScoreAsync(
                "Set_Price",
                pfDTO.PriceFrom ?? double.NegativeInfinity,
                pfDTO.PriceTo ?? double.PositiveInfinity);

            var priceIds = new HashSet<long>(priceMembers.Select(x => (long)x));

            if (finalResults == null) finalResults = priceIds;
            else finalResults.IntersectWith(priceIds);
        }
        if (finalResults == null)
        {
            return [.. (await _redis.SetMembersAsync("Set_AllParrots")).Select(x => (long)x)];
        }

        return [.. finalResults];
    }
}
