using BTCGatewayAPI.Infrastructure.Logging;
using BTCGatewayAPI.Models.DTO;
using BTCGatewayAPI.Services.Extensions;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Runtime.Caching;
using System.Threading.Tasks;

namespace BTCGatewayAPI.Services
{
    public class TransactionsService : BaseService
    {
        private readonly Infrastructure.GlobalConf _conf;
        private readonly MemoryCache _memoryCache;
        private static readonly Lazy<ILogger> LoggerLazy = new Lazy<ILogger>(LoggerFactory.GetLogger);
        private static readonly string CacheEntryName = "lasttx";

        private static ILogger Logger => LoggerLazy.Value;

        public TransactionsService(DbConnection dBContext, Infrastructure.GlobalConf conf, MemoryCache memoryCache) : base(dBContext)
        {
            _conf = conf;
            _memoryCache = memoryCache;
        }

        public async Task<IEnumerable<LastTransactionDTO>> GetLastTransactionsAsync()
        {
            if (_memoryCache.Contains(CacheEntryName))
            {
                return (IEnumerable<LastTransactionDTO>)_memoryCache.Get(CacheEntryName);
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

                _memoryCache.Add(new CacheItem(CacheEntryName, result), new CacheItemPolicy
                {
                    AbsoluteExpiration = new DateTimeOffset(DateTime.Now, TimeSpan.FromSeconds(5))
                });

                return result;
            }
        }
    }
}
