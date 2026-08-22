using Domain.Contracts;
using Microsoft.EntityFrameworkCore.Storage;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDatabase = StackExchange.Redis.IDatabase;
namespace Persistence.Repositories
{
    public class CacheRepository(IConnectionMultiplexer connection) : ICacheRepository
    {
        readonly IDatabase _database = connection.GetDatabase();
        public async Task<double> GetAsync(string CacheKey)
        {
            var CacheValue = await _database.StringGetAsync(CacheKey);
            return CacheValue.IsNullOrEmpty ? 0 : (double)CacheValue;
        }

        //public async Task SetAsync(string CacheKey, int CacheValue, TimeSpan timeToLive)
        //{
        //    await _database.StringSetAsync(CacheKey, CacheValue, timeToLive);
        //}
        public async Task ResetToZeroAsync(string CacheKey)
        {
            await _database.StringSetAsync(CacheKey, 0, TimeSpan.FromDays(30));
        }
        public async Task IncrementAmountAsync(string cacheKey, double amount)
        {
            await _database.StringIncrementAsync(cacheKey, amount);
        }

        public async Task IncrementCountAsync(string cacheKey)
        {
            await _database.StringIncrementAsync(cacheKey);
        }
    }
}
