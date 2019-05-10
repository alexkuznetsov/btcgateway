using BTCGatewayAPI.Bitcoin;
using BTCGatewayAPI.Services.Extensions;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Data.Common;
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
        private readonly Common.GlobalConf _сonf;
        private readonly IMemoryCache _memoryCache;

        public SyncBTCTransactinsService(DbConnection dbContext
            , BitcoinClientFactory clientFactory
            , Common.GlobalConf globalConf
            , IMemoryCache memoryCache) : base(dbContext)
        {
            _clientFactory = clientFactory;
            _сonf = globalConf;
            _memoryCache = memoryCache;
        }

        public async Task DownloadAsync()
        {
            var allHotWallets = await DbCon.GetAllHotWalletsAsync();
            var status = false;

            foreach (var wallet in allHotWallets)
            {
                var bitcoinClient = _clientFactory.Create(new Uri(wallet.RPCAddress), wallet.RPCUsername, wallet.RPCPassword);
                var transactions = await bitcoinClient.ListTransactionsAsync();
                //var addressTransactions = transactions.Where(x => x.Address == wallet.Address).ToArray();

                foreach (var tx in transactions)
                {
                    status ^= await ProcessTransactionAsync(bitcoinClient, wallet, tx);
                }

                if (status)
                {
                    _memoryCache.Remove(CacheKeys.LastTransactions);
                }
            }
        }

        private async Task<bool> ProcessTransactionAsync(BitcoinClient bitcoinClient, Models.HotWallet wallet, Bitcoin.Models.Transaction tx)
        {
            string rawHex;
            try
            {
                rawHex = await bitcoinClient.GetRawTransaction(tx.Txid);
            }
            catch (Bitcoin.RPCServerException)
            {
                var txInfo = await bitcoinClient.GetTransaction(tx.Txid);
                rawHex = txInfo.Hex;
            }

            return await SaveAsync(tx, wallet, rawHex);
        }

        private async Task<bool> SaveAsync(Bitcoin.Models.Transaction tx, Models.HotWallet wallet, string rawHex)
        {
            if (DbCon.State != System.Data.ConnectionState.Open)
                await DbCon.OpenAsync();

            using (var dbtx = DbCon.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
            {
                try
                {
                    var isSaved = false;
                    if (tx.IsReceive())
                    {
                        isSaved = await ProcessReceiveTxAsync(dbtx, wallet.Id, tx, rawHex);
                    }
                    else if (tx.IsSend())
                    {
                        isSaved = await ProcessSendTxAsync(dbtx, wallet.Id, tx, rawHex);
                    }

                    dbtx.Commit();

                    return isSaved;
                }
                catch (Exception)
                {
                    dbtx.Rollback();
                    throw;
                }
            }
        }

        private async Task<bool> ProcessSendTxAsync(DbTransaction dbtx, int walletId, Bitcoin.Models.Transaction tx, string rawHex)
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
                return true;
            }
            else if (outcomeTx.Confirmations <= _сonf.MinimalConfirmations)
            {
                outcomeTx.Confirmations = tx.Confirmations;
                outcomeTx.UpdatedAt = DateTime.Now;

                await DbCon.UpdateSendTransactionAsync(dbtx, outcomeTx);
                return true;
            }
            return false;
        }

        private async Task<bool> ProcessReceiveTxAsync(DbTransaction dbtx, int walletId, Bitcoin.Models.Transaction tx, string rawHex)
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
                return true;
            }
            else if (reciveTx.Confirmations <= _сonf.MinimalConfirmations)
            {
                reciveTx.Confirmations = tx.Confirmations;
                reciveTx.UpdatedAt = DateTime.Now;

                await DbCon.UpdateReceiveTransactionAsync(dbtx, reciveTx);
                return true;
            }
            return false;
        }
    }
}
