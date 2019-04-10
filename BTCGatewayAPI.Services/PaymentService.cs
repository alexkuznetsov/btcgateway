using BTCGatewayAPI.Bitcoin;
using BTCGatewayAPI.Infrastructure.Logging;
using BTCGatewayAPI.Models.Requests;
using BTCGatewayAPI.Services.Extensions;
using System;
using System.Data.Common;
using System.Threading.Tasks;

namespace BTCGatewayAPI.Services
{
    public class PaymentService : BaseService
    {
        private readonly BitcoinClientFactory _clientFactory;
        private readonly Infrastructure.GlobalConf _conf;

        private static readonly Lazy<ILogger> LoggerLazy = new Lazy<ILogger>(LoggerFactory.GetLogger);

        private static ILogger Logger => LoggerLazy.Value;

        public PaymentService(DbConnection dBContext, BitcoinClientFactory clientFactory, Infrastructure.GlobalConf conf) : base(dBContext)
        {
            _clientFactory = clientFactory;
            _conf = conf;
        }

        public async Task SendAsync(SendBtcRequest sendBtcRequest)
        {
            if (DbCon.State != System.Data.ConnectionState.Open)
                await DbCon.OpenAsync();

            var wallet = await DbCon.GetFirstWithBalanceMoreThanAsync(sendBtcRequest.Amount);
            var bitcoinClient = _clientFactory.Create(new Uri(wallet.RPCAddress), wallet.RPCUsername, wallet.RPCPassword);
            var txInfo = await CreateTransactionAsync(bitcoinClient, wallet, sendBtcRequest);

            (var isSuccess, var payment) = await SaveWalletAndPaymentAsync(sendBtcRequest, txInfo, wallet
                , onSuccess: (s) => bitcoinClient.SendRawTransactionAsync(s)
                , onError: (s) => bitcoinClient.RemovePrunedFundsAsync(s));

            if (isSuccess)
            {
                await ChangePaymentStatusAsync(payment);
            }
        }

        private async Task ChangePaymentStatusAsync(Models.OutcomeTransaction payment)
        {
            var isSuccess = false;

            using (var tx = DbCon.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
            {
                try
                {
                    payment.State = Models.OutcomeTransaction.CompleteState;

                    (isSuccess, _) = await TryToPerformAsync(
                        action: () => DbCon.UpdateSendTransactionAsync(tx, payment),
                        onError: (ex) => Logger.Error(ex, Messages.ErrExceptionWhenUpdateOutputTx, payment.Id),
                        triesCount: _conf.RetryActionCnt);

                    if (!isSuccess)
                    {
                        Logger.Error(Messages.ErrExceptionWhenChangingStateForOutputTx, payment.Id);
                        throw new InvalidOperationException(string.Format(Messages.ErrExceptionWhenChangingStateForOutputTx, payment.Id));
                    }
                }
                catch (Exception)
                {
                    tx.Rollback();
                    throw;
                }
            }
        }

        public async Task<(bool, Models.OutcomeTransaction)> SaveWalletAndPaymentAsync(
            SendBtcRequest sendBtcRequest,
            FundTransactionStrategy.FundTransactionStrategyResult txInfo,
            Models.HotWallet wallet,
            Func<string, Task> onSuccess,
            Func<string, Task> onError)
        {
            var isSuccess = false;

            //В целом, это делать не нужно, т.к. синхронизация сделает это за нас, подже.
            var payment = new Models.OutcomeTransaction
            {
                Amount = sendBtcRequest.Amount,
                Address = sendBtcRequest.Account,
                WalletId = wallet.Id,
                TxHash = txInfo.Hex,
                Txid = txInfo.Txid,
                State = Models.OutcomeTransaction.WithdrawState,
                Fee = txInfo.Fee/*.Feerate*/,
                CreatedAt = DateTime.Now,
                Confirmations = 0,
                Id = 0,
                UpdatedAt = null
            };

            if (DbCon.State != System.Data.ConnectionState.Open)
                await DbCon.OpenAsync();

            using (var tx = DbCon.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
            {
                try
                {
                    int paymentId = 0;
                    //Снимается сумма + комиссия
                    Logger.Debug("Try to withdraw: {0}, exists: {1}", sendBtcRequest.Amount + txInfo.Fee, wallet.Amount);
                    wallet.Withdraw(sendBtcRequest.Amount + txInfo.Fee/*.Feerate*/);

                    (isSuccess, _) = await TryToPerformAsync(
                        action: () => DbCon.UpdateWalletAsync(tx, wallet),
                        onError: (ex) => Logger.Error(ex, Messages.ErrUpdateWalletInformation, sendBtcRequest),
                        triesCount: _conf.RetryActionCnt);

                    if (!isSuccess)
                    {
                        Logger.Error(Messages.ErrorUpdateWalletFails, wallet.Id, sendBtcRequest);
                        throw new InvalidOperationException(string.Format(Messages.ErrorUpdateWalletFails, wallet.Id, sendBtcRequest));
                    }

                    (isSuccess, paymentId) = await TryToPerformAsync(
                        action: () => DbCon.AddSendTransactionAsync(tx, payment),
                        onError: (ex) => Logger.Error(ex, Messages.ErrAddOutputTransaction, sendBtcRequest),
                        triesCount: _conf.RetryActionCnt);

                    if (!isSuccess)
                    {
                        Logger.Error(Messages.ErrorSaveOutputTransaction, wallet.Id, sendBtcRequest);
                        throw new InvalidOperationException(string.Format(Messages.ErrorSaveOutputTransaction, wallet.Id, sendBtcRequest));
                    }
                    else
                    {
                        payment.Id = paymentId;
                    }

                    tx.Commit();

                    await onSuccess(txInfo.Hex);

                    return (isSuccess, payment);
                }
                catch (RPCServerException)
                {
                    await onError(txInfo.Hex);
                    throw;
                }
                catch (Exception)
                {
                    tx.Rollback();
                    throw;
                }
            }
        }

        public Task<FundTransactionStrategy.FundTransactionStrategyResult> CreateTransactionAsync(BitcoinClient bitcoinClient, Models.HotWallet hotWallet, SendBtcRequest sendBtcRequest)
        {
            FundTransactionStrategy strategy;

            if (_conf.UseFundRawTransaction)
            {
                strategy = new AutoFundTransactionStrategy(bitcoinClient, _conf);
            }
            else
            {
                strategy = new ManualFundTransactionStrategy(bitcoinClient, _conf);
            }

            return strategy.CreateAndSignTransactionAsync(hotWallet, sendBtcRequest);
        }
    }
}