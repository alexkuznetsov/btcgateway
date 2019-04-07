using BTCGatewayAPI.Services.Extensions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BTCGatewayAPI.Services
{
    /// <summary>
    /// Данный класс забирает все транзации с bitcoind и складывает их в базу.
    /// От данного подохода лучше избавиться и получать информацию по транзакциям через ZMQ, что архитектурно вернее
    /// и позволит значительно упростить код.
    /// </summary>
    public class SyncBTCTransactinsService : BaseService
    {
        private readonly BitcoinClientFactory clientFactory;
        private readonly Infrastructure.GlobalConf globalConf;

        private static readonly Lazy<Infrastructure.Logging.ILogger> LoggerLazy = new Lazy<Infrastructure.Logging.ILogger>(Infrastructure.Logging.LoggerFactory.GetLogger);

        private static Infrastructure.Logging.ILogger Logger => LoggerLazy.Value;

        public SyncBTCTransactinsService(Infrastructure.DB.DBContext dbContext
            , BitcoinClientFactory clientFactory
            , Infrastructure.GlobalConf globalConf) : base(dbContext)
        {
            this.clientFactory = clientFactory;
            this.globalConf = globalConf;
        }

        public async Task DownloadAsync()
        {
            var allHotWallets = await DBContext.GetAllHotWallets();

            using (var dbtx = await DBContext.BeginTransaction(System.Data.IsolationLevel.Serializable))
            {
                try
                {
                    foreach (var wallet in allHotWallets)
                    {
                        var bitcoinClient = clientFactory.Create(new Uri(wallet.RPCAddress), wallet.RPCUsername, wallet.RPCPassword);
                        var transactions = await bitcoinClient.ListTransactions();

                        var isSuccess = false;

                        foreach (var tx in transactions.Where(x => x.Address == wallet.Address))
                        {
                            if (tx.IsRecive())
                            {
                                isSuccess = await TryToPerform(
                                    action: () => ProcessRecieveTx(wallet.Id, tx),
                                    onError: (ex) => Logger.Error(ex, "Error to process income transaction"),
                                    triesCount: 3);

                                if (!isSuccess)
                                {
                                    Logger.Error("Can not to process input tx with txid" + tx.Txid);
                                }
                            }
                            else if (tx.IsSend())
                            {
                                isSuccess = await TryToPerform(
                                    action: () => ProcessSendTx(wallet.Id, tx),
                                    onError: (ex) => Logger.Error(ex, "Error to process outcome transaction"),
                                    triesCount: 3);

                                if (!isSuccess)
                                {
                                    Logger.Error("Can not to process output tx with txid" + tx.Txid);
                                }
                            }
                            else
                                throw new InvalidOperationException("Transaction type without process handler: " + tx.Category);
                        }
                    }

                    dbtx.Commit();
                }
                catch (Exception)
                {
                    dbtx.Rollback();
                    throw;
                }
            }
        }

        private async Task ProcessSendTx(int walletId, Bitcoin.Models.WalletTransaction tx)
        {
            var outcomeTx = await DBContext.FindOutputTxByTxid(tx.Txid);

            if (outcomeTx == null)
            {
                await DBContext.Add(new Models.OutcomeTransaction
                {
                    Amount = tx.Amount,
                    Confirmations = tx.Confirmations,
                    CreatedAt = TimeUtils.FromUnixTime(tx.Time),
                    Fee = tx.Fee,
                    Id = 0,
                    Recipient = tx.Address,
                    State = Models.OutcomeTransaction.CompleteState,
                    TxHash = tx.Txid,
                    WalletId = walletId
                });
            }
            else if (outcomeTx.Confirmations <= globalConf.MinimalConfirmations)
            {
                outcomeTx.Confirmations = tx.Confirmations;
                outcomeTx.UpdatedAt = DateTime.Now;

                await DBContext.Update(outcomeTx);
            }
        }

        private async Task ProcessRecieveTx(int walletId, Bitcoin.Models.WalletTransaction tx)
        {
            var txId = await DBContext.FindIncomeTxIdByTxid(tx.Txid);

            var iTx = new Models.IncomeTransaction
            {
                Amount = tx.Amount,
                Confirmations = tx.Confirmations,
                CreatedAt = TimeUtils.FromUnixTime(tx.Time),
                Id = txId?.Id ?? 0,
                Sender = tx.Address,
                TxHash = tx.Txid,
                UpdatedAt = null,
                WalletId = walletId
            };

            if (txId == null || tx.Confirmations <= globalConf.MinimalConfirmations)
            {
                if (txId != null)
                    await DBContext.Update(iTx);
                else
                    await DBContext.Add(iTx);
            }
        }
    }
}
