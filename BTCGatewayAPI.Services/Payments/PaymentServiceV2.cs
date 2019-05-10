using BTCGatewayAPI.Bitcoin;
using BTCGatewayAPI.Common.Logging;
using BTCGatewayAPI.Models.Requests;
using BTCGatewayAPI.Services.Extensions;
using System;
using System.Data.Common;
using System.Threading.Tasks;

namespace BTCGatewayAPI.Services.Payments
{
    public class PaymentServiceV2 : BaseService, IPaymentService
    {
        private readonly BitcoinClientFactory _clientFactory;
        private readonly Common.GlobalConf _conf;

        private static readonly Lazy<ILogger> LoggerLazy = new Lazy<ILogger>(LoggerFactory.GetLogger);

        private static ILogger Logger => LoggerLazy.Value;

        public PaymentServiceV2(DbConnection dBContext, BitcoinClientFactory clientFactory, Common.GlobalConf conf) : base(dBContext)
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

            await bitcoinClient.WalletPassphraseAsync(wallet.Passphrase, _conf.DefaultWalletUnlockTime);

            var txId = await bitcoinClient.SendToAddressAsync(sendBtcRequest.Account, sendBtcRequest.Amount);
            var txHash = await bitcoinClient.GetRawTransaction(txId);

            await SaveWalletAndPaymentAsync(sendBtcRequest, txId, txHash, wallet);
        }

        public async Task SaveWalletAndPaymentAsync(
            SendBtcRequest sendBtcRequest,
            string txid,
            string txhash,
            Models.HotWallet wallet)
        {
            var isSuccess = false;

            //В целом, это делать не нужно, т.к. синхронизация сделает это за нас, подже.
            var payment = new Models.OutcomeTransaction
            {
                Amount = sendBtcRequest.Amount,
                Address = sendBtcRequest.Account,
                WalletId = wallet.Id,
                TxHash = txhash,
                Txid = txid,
                State = Models.OutcomeTransaction.WithdrawState,
                Fee = 0/*.Feerate*/,
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
                    Logger.Debug("Try to withdraw: {0}, exists: {1}", sendBtcRequest.Amount, wallet.Amount);
                    wallet.Withdraw(sendBtcRequest.Amount/*.Feerate*/);

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
                }
                catch (Exception)
                {
                    tx.Rollback();
                    throw;
                }
            }
        }
    }
}