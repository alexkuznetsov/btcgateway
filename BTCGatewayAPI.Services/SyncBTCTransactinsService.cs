using BTCGatewayAPI.Infrastructure.Logging;
using BTCGatewayAPI.Services.Extensions;
using System;
using System.Data.Common;
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
        private readonly BitcoinClientFactory _clientFactory;
        private readonly Infrastructure.GlobalConf _сonf;

        private static readonly Lazy<ILogger> LoggerLazy = new Lazy<ILogger>(LoggerFactory.GetLogger);

        private static ILogger Logger => LoggerLazy.Value;

        public SyncBTCTransactinsService(DbConnection dbContext
            , BitcoinClientFactory clientFactory
            , Infrastructure.GlobalConf globalConf) : base(dbContext)
        {
            _clientFactory = clientFactory;
            _сonf = globalConf;
        }

        public async Task DownloadAsync()
        {
            var allHotWallets = await DbCon.GetAllHotWalletsAsync();

            foreach (var wallet in allHotWallets)
            {
                await ProcessWalletAsync(wallet);
            }
        }

        private async Task ProcessWalletAsync(Models.HotWallet wallet)
        {
            var bitcoinClient = _clientFactory.Create(new Uri(wallet.RPCAddress), wallet.RPCUsername, wallet.RPCPassword);
            var transactions = await bitcoinClient.ListTransactionsAsync();
            //var addressTransactions = transactions.Where(x => x.Address == wallet.Address).ToArray();

            foreach (var tx in transactions)
            {
                string rawHex = string.Empty;

                try
                {
                    rawHex = await bitcoinClient.GetRawTransaction(tx.Txid);
                }
                catch (Bitcoin.RPCServerException)
                {
                    var txInfo = await bitcoinClient.GetTransaction(tx.Txid);
                    rawHex = txInfo.Hex;
                }

                await SaveAsync(tx, wallet, rawHex);
            }
        }

        private async Task SaveAsync(Bitcoin.Models.Transaction tx, Models.HotWallet wallet, string rawHex)
        {
            if (DbCon.State != System.Data.ConnectionState.Open)
                await DbCon.OpenAsync();

            using (var dbtx = DbCon.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
            {
                try
                {
                    if (tx.IsReceive())
                    {
                        await ProcessReceiveTxAsync(dbtx, wallet.Id, tx, rawHex);
                    }
                    else if (tx.IsSend())
                    {
                        await ProcessSendTxAsync(dbtx, wallet.Id, tx, rawHex);
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

        private async Task ProcessSendTxAsync(DbTransaction dbtx, int walletId, Bitcoin.Models.Transaction tx, string rawHex)
        {
            var outcomeTx = await DbCon.FindSendTransactionByTxidAndAddressAsync(dbtx, tx.Txid, tx.Address);

            if (outcomeTx == null)
            {
                await DbCon.AddSendTransactionAsync(dbtx, new Models.OutcomeTransaction
                {
                    Amount = tx.Amount,
                    Confirmations = tx.Confirmations,
                    CreatedAt = TimeUtils.FromUnixTime(tx.Time),
                    Fee = tx.Fee,
                    Id = 0,
                    Address = tx.Address,
                    State = Models.OutcomeTransaction.CompleteState,
                    Txid = tx.Txid,
                    TxHash = rawHex,
                    WalletId = walletId,
                    UpdatedAt = null
                });
            }
            else if (outcomeTx.Confirmations <= _сonf.MinimalConfirmations)
            {
                outcomeTx.Confirmations = tx.Confirmations;
                outcomeTx.UpdatedAt = DateTime.Now;

                await DbCon.UpdateSendTransactionAsync(dbtx, outcomeTx);
            }
        }

        private async Task ProcessReceiveTxAsync(DbTransaction dbtx, int walletId, Bitcoin.Models.Transaction tx, string rawHex)
        {
            var reciveTx = await DbCon.FindReceiveTxIdByTxidAndAddressAsync(dbtx, tx.Txid, tx.Address);

            if (reciveTx == null)
            {
                await DbCon.AddReceiveTransactionAsync(dbtx, new Models.IncomeTransaction
                {
                    Amount = tx.Amount,
                    Confirmations = tx.Confirmations,
                    CreatedAt = TimeUtils.FromUnixTime(tx.Time),
                    Id = 0,
                    Address = tx.Address,
                    Txid = tx.Txid,
                    TxHash = rawHex,
                    WalletId = walletId,
                    ViewCount = 0,
                    UpdatedAt = null
                });
            }
            else if (reciveTx.Confirmations <= _сonf.MinimalConfirmations)
            {
                reciveTx.Confirmations = tx.Confirmations;
                reciveTx.UpdatedAt = DateTime.Now;

                await DbCon.UpdateReceiveTransactionAsync(dbtx, reciveTx);
            }
        }
    }
}
