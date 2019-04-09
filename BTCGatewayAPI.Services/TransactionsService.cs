using BTCGatewayAPI.Models.DTO;
using BTCGatewayAPI.Services.Extensions;
using System;
using System.Threading.Tasks;

namespace BTCGatewayAPI.Services
{
    public class TransactionsService : BaseService
    {
        private readonly Infrastructure.GlobalConf conf;

        private static readonly Lazy<Infrastructure.Logging.ILogger> LoggerLazy = new Lazy<Infrastructure.Logging.ILogger>(Infrastructure.Logging.LoggerFactory.GetLogger);

        private static Infrastructure.Logging.ILogger Logger => LoggerLazy.Value;

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
                        onError: (ex) => Logger.Error(ex, "Error to get last updates, trying again..."),
                        triesCount: conf.RetryActionCnt);

                if (!isSuccess)
                {
                    Logger.Error("Error to get last updates, something went wrong");
                }

                tx.Commit();

                return result;
            }
        }
    }
}
