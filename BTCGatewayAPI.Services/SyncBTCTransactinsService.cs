﻿using BTCGatewayAPI.Services.Extensions;
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
            var allHotWallets = await DBContext.GetAllHotWalletsAsync();

            foreach (var wallet in allHotWallets)
            {
                await ProcessWalletAsync(wallet);
            }
        }

        private async Task ProcessWalletAsync(Models.HotWallet wallet)
        {
            var bitcoinClient = clientFactory.Create(new Uri(wallet.RPCAddress), wallet.RPCUsername, wallet.RPCPassword);
            var transactions = await bitcoinClient.ListTransactionsAsync();
            var addressTransactions = transactions.Where(x => x.Address == wallet.Address).ToArray();

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
            var isSuccess = false;

            using (var dbtx = await DBContext.BeginTransactionAsync(System.Data.IsolationLevel.ReadCommitted))
            {
                try
                {
                    if (tx.IsReceive())
                    {
                        await ProcessReceiveTxAsync(wallet.Id, tx, rawHex);
                    }
                    else if (tx.IsSend())
                    {
                        await ProcessSendTxAsync(wallet.Id, tx, rawHex);
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

        private async Task ProcessSendTxAsync(int walletId, Bitcoin.Models.Transaction tx, string rawHex)
        {
            var outcomeTx = await DBContext.FindSendTransactionByTxidAndAddress(tx.Txid, tx.Address);

            if (outcomeTx == null)
            {
                await DBContext.AddAsync(new Models.OutcomeTransaction
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
            else if (outcomeTx.Confirmations <= globalConf.MinimalConfirmations)
            {
                outcomeTx.Confirmations = tx.Confirmations;
                outcomeTx.UpdatedAt = DateTime.Now;

                await DBContext.UpdateAsync(outcomeTx);
            }
        }

        private async Task ProcessReceiveTxAsync(int walletId, Bitcoin.Models.Transaction tx, string rawHex)
        {
            var reciveTx = await DBContext.FindReceiveTxIdByTxidAndAddressAsync(tx.Txid, tx.Address);

            if (reciveTx == null)
            {
                await DBContext.AddAsync(new Models.IncomeTransaction
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
            else if (reciveTx.Confirmations <= globalConf.MinimalConfirmations)
            {
                reciveTx.Confirmations = tx.Confirmations;
                reciveTx.UpdatedAt = DateTime.Now;

                await DBContext.UpdateAsync(reciveTx);
            }
        }
    }
}
