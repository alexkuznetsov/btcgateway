using BTCGatewayAPI.Common.Logging;
using BTCGatewayAPI.Models.DTO;
using BTCGatewayAPI.Services.Extensions;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace BTCGatewayAPI.Services
{
    public class TransactionsService : BaseService
    {
        private readonly Common.GlobalConf _conf;
        private readonly IMemoryCache _cache;
        private static readonly Lazy<ILogger> LoggerLazy = new Lazy<ILogger>(LoggerFactory.GetLogger);

        private static ILogger Logger => LoggerLazy.Value;

        public TransactionsService(DbConnection dBContext, Common.GlobalConf conf, IMemoryCache cache) : base(dBContext)
        {
            _conf = conf;
            _cache = cache;
        }

        public async Task<IEnumerable<LastTransactionDTO>> GetLastTransactionsAsync()
        {
            if (_cache.TryGetValue(CacheKeys.LastTransactions, out LastTransactionDTO[] cacheEntry))
            {
                return cacheEntry;
            }

            if (DbCon.State != System.Data.ConnectionState.Open)
                await DbCon.OpenAsync();

            using (var tx = DbCon.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
            {
                (var isSuccess, var result) = await TryToPerformAsync(
                        action: () => DbCon.GetNewIncomeTransactionsAsync(tx, _conf.MinimalConfirmations),
                        onError: (ex) => Logger.Error(ex, Messages.ErrGetLastUpdatesRetryAgain),
                        triesCount: _conf.RetryActionCnt);

                if (!isSuccess)
                {
                    Logger.Error(Messages.ErrGetLastUpdatesGeneralFail);
                    throw new InvalidOperationException(Messages.ErrGetLastUpdatesGeneralFail);
                }

                tx.Commit();

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(5));

                _cache.Set(CacheKeys.LastTransactions, result.ToArray(), cacheEntryOptions);

                return result;
            }
        }
    }
}
