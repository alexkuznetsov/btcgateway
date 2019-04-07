﻿using BTCGatewayAPI.Bitcoin;
using BTCGatewayAPI.Infrastructure.DB;
using BTCGatewayAPI.Models.Requests;
using BTCGatewayAPI.Services.Extensions;
using System;
using System.Threading.Tasks;

namespace BTCGatewayAPI.Services
{
    public class PaymentService : BaseService
    {
        private readonly BitcoinClientFactory clientFactory;
        private readonly Infrastructure.GlobalConf conf;

        private static readonly Lazy<Infrastructure.Logging.ILogger> LoggerLazy = new Lazy<Infrastructure.Logging.ILogger>(Infrastructure.Logging.LoggerFactory.GetLogger);
        private static Infrastructure.Logging.ILogger Logger => LoggerLazy.Value;

        public PaymentService(DBContext dBContext, BitcoinClientFactory clientFactory, Infrastructure.GlobalConf conf) : base(dBContext)
        {
            this.clientFactory = clientFactory;
            this.conf = conf;
        }

        public async Task SendAsync(SendBtcRequest sendBtcRequest)
        {
            var wallet = await DBContext.GetFirstWithBalanceMoreThan(sendBtcRequest.Amount);
            var bitcoinClient = clientFactory.Create(new Uri(wallet.RPCAddress), wallet.RPCUsername, wallet.RPCPassword);
            (var txHash, var fee) = await CreateTransaction(bitcoinClient, wallet, sendBtcRequest);

            //В целом, это делать не нужно, т.к. синхронизация сделает это за нас, подже.
            var payment = new Models.OutcomeTransaction
            {
                Amount = sendBtcRequest.Amount,
                Recipient = sendBtcRequest.Account,
                WalletId = wallet.Id,
                TxHash = txHash,
                State = Models.OutcomeTransaction.WithdrawState,
                Fee = fee/*.Feerate*/
            };

            var isSuccess = false;

            using (var tx = await DBContext.BeginTransaction(System.Data.IsolationLevel.Serializable))
            {
                try
                {
                    //Снимается сумма + комиссия
                    wallet.Withdraw(sendBtcRequest.Amount + fee/*.Feerate*/);

                    (isSuccess, wallet) = await TryToPerform(
                        action: () => DBContext.Update(wallet),
                        onError: (ex) => Logger.Error(ex, "Error to update wallet information, trying again. Request: " + sendBtcRequest),
                        triesCount: 3);

                    if (!isSuccess)
                    {
                        Logger.Error("Wallet with id: " + wallet.Id + " can not be updated. Request: " + sendBtcRequest);
                    }

                    (isSuccess, payment) = await TryToPerform(
                        action: () => DBContext.Add(payment),
                        onError: (ex) => Logger.Error(ex, "Error to add output transaction information, trying again. Request: " + sendBtcRequest),
                        triesCount: 3);

                    if (!isSuccess)
                    {
                        Logger.Error("Output transaction fow wallet with id: " + wallet.Id + " can not be saved. Request: " + sendBtcRequest);
                    }

                    tx.Commit();

                    await bitcoinClient.SendRawTransaction(txHash);
                }
                catch (RPCServerException)
                {
                    //removeprunedfunds 
                    await bitcoinClient.RemovePrunedFunds(txHash);
                    throw;
                }
                catch (Exception)
                {
                    tx.Rollback();
                    throw;
                }
            }

            using (var tx = await DBContext.BeginTransaction(System.Data.IsolationLevel.Serializable))
            {
                try
                {
                    payment.State = Models.OutcomeTransaction.CompleteState;

                    (isSuccess, payment) = await TryToPerform(
                        action: () => DBContext.Update(payment),
                        onError: (ex) => Logger.Error(ex, "Error to update output transaction with id " + payment.Id + ". Changing state to compleate, trying again..."),
                        triesCount: 3);

                    if (!isSuccess)
                    {
                        Logger.Error("Error to update output transaction with id " + payment.Id + ". Changing state to compleate fails.");
                    }
                }
                catch (Exception)
                {
                    tx.Rollback();
                    throw;
                }
            }
        }

        public Task<(string, decimal)> CreateTransaction(BitcoinClient bitcoinClient, Models.HotWallet hotWallet, SendBtcRequest sendBtcRequest)
        {
            FundTransactionStrategy strategy;

            if (conf.UseFundRawTransaction)
            {
                strategy = new AutoFundTransactionStrategy(bitcoinClient, conf);
            }
            else
            {
                strategy = new ManualFundTransactionStrategy(bitcoinClient, conf);
            }

            return strategy.CreateAndSignTransaction(hotWallet, sendBtcRequest);
        }
    }
}