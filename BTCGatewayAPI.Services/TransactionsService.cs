using BTCGatewayAPI.Infrastructure.Logging;
using BTCGatewayAPI.Models.DTO;
using BTCGatewayAPI.Services.Extensions;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace BTCGatewayAPI.Services
{
    public class TransactionsService : BaseService
    {
        private readonly Infrastructure.GlobalConf _conf;

        private static readonly Lazy<ILogger> LoggerLazy = new Lazy<ILogger>(LoggerFactory.GetLogger);

        private static ILogger Logger => LoggerLazy.Value;

        public TransactionsService(DbConnection dBContext, Infrastructure.GlobalConf conf) : base(dBContext)
        {
            _conf = conf;
        }

        public async Task<IEnumerable<LastTransactionDTO>> GetLastTransactionsAsync()
        {
            if (DbCon.State != System.Data.ConnectionState.Open)
                await DbCon.OpenAsync();

            using (var tx = DbCon.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
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

                return result;
            }
        }
    }
}
