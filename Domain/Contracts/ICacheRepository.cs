using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface ICacheRepository
    {
        Task<double> GetAsync(string CacheKey);
        //Task SetAsync(string CacheKey, int CacheValue, TimeSpan timeToLive);
        Task ResetToZeroAsync(string CacheKey);
        Task IncrementAmountAsync(string cacheKey, double amount);
        Task IncrementCountAsync(string cacheKey);
    }
}
