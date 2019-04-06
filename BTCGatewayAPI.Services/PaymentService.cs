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

        public PaymentService(DBContext dBContext, BitcoinClientFactory clientFactory, Infrastructure.GlobalConf conf) : base(dBContext)
        {
            this.clientFactory = clientFactory;
            this.conf = conf;
        }

        public async Task SendAsync(SendBtcRequest sendBtcRequest)
        {
            var wallet = await DBContext.GetFirstWithBalanceMoreThan(sendBtcRequest.Amount);
            var bitcoinClient = clientFactory.Create(new Uri(wallet.RPCAddress), wallet.RPCUsername, wallet.RPCPassword);
            var txHash = await CreateAndSignTransaction(bitcoinClient, wallet.Address, wallet.Passphrase, sendBtcRequest);
            var fee = await bitcoinClient.LoadEstimateSmartFee();

            var payment = new Models.OutcomeTransaction
            {
                Amount = sendBtcRequest.Amount,
                Recipient = sendBtcRequest.Account,
                WalletId = wallet.Id,
                TxHash = txHash,
                State = Models.OutcomeTransaction.WithdrawState,
                Fee = fee.Feerate
            };

            using (var tx = await DBContext.BeginTransaction(System.Data.IsolationLevel.Serializable))
            {
                try
                {
                    //Снимается сумма + комиссия
                    wallet.Withdraw(sendBtcRequest.Amount + fee.Feerate);

                    wallet = await DBContext.Update(wallet);
                    payment = await DBContext.Add(payment);

                    tx.Commit();

                    await bitcoinClient.SendRawTransaction(txHash);
                }
                catch (Exception)
                {
                    //removeprunedfunds 
                    await bitcoinClient.RemovePrunedFunds(txHash);
                    tx.Rollback();
                    throw;
                }
            }

            using (var tx = await DBContext.BeginTransaction(System.Data.IsolationLevel.Serializable))
            {
                try
                {
                    payment.State = Models.OutcomeTransaction.CompleteState;

                    await DBContext.Update(payment);
                }
                catch (Exception)
                {
                    tx.Rollback();
                    throw;
                }
            }
        }

        public async Task<string> CreateAndSignTransaction(BitcoinClient bitcoinClient, string address, string passphrase, SendBtcRequest sendBtcRequest)
        {
            var privateKey = await bitcoinClient.LoadWalletPrivateKeys(address, passphrase, conf.WalletUnlockTime);
            var fee = await bitcoinClient.LoadEstimateSmartFee();
            var unspent = await bitcoinClient.GetUnspentTransactionOutputs(address, sendBtcRequest.Amount + fee.Feerate);
            var rawTx = await bitcoinClient.CreateTransaction(address, unspent, sendBtcRequest);
            var signed = await bitcoinClient.SignRawTransactionWithKey(unspent, new[] { privateKey }, rawTx);

            return signed.Hex;
        }
    }
}