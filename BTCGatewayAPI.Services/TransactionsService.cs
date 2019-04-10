using BTCGatewayAPI.Infrastructure.Logging;
using BTCGatewayAPI.Models.DTO;
using BTCGatewayAPI.Services.Extensions;
using System;
using System.Threading.Tasks;

namespace BTCGatewayAPI.Services
{
    public class TransactionsService : BaseService
    {
        private readonly Infrastructure.GlobalConf conf;

        private static readonly Lazy<ILogger> LoggerLazy = new Lazy<ILogger>(LoggerFactory.GetLogger);

        private static ILogger Logger => LoggerLazy.Value;

        public TransactionsService(Infrastructure.DB.DBContext dBContext, Infrastructure.GlobalConf conf) : base(dBContext)
        {
            this.conf = conf;
        }

        public async Task<LastTransactionDTO[]> GetLastTransactionsAsync()
        {
            using (var tx = await DBContext.BeginTransactionAsync(System.Data.IsolationLevel.ReadUncommitted))
            {
                (var isSuccess, var result) = await TryToPerformAsync(
                        action: () => DBContext.GetNewIncomeTransactionsAsync(conf.MinimalConfirmations),
                        onError: (ex) => Logger.Error(ex, Messages.ErrGetLastUpdatesRetryAgain),
                        triesCount: conf.RetryActionCnt);

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
