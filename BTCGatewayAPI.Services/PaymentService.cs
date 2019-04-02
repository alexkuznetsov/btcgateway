using BTCGatewayAPI.Infrastructure.DB;
using BTCGatewayAPI.Models;
using BTCGatewayAPI.Models.Requests;
using BTCGatewayAPI.Services.Extensions;
using System;
using System.Threading.Tasks;

namespace BTCGatewayAPI.Services
{
    public class PaymentService : BaseService
    {
        private readonly BitcoinClientFactory clientFactory;

        public PaymentService(DBContext dBContext, BitcoinClientFactory clientFactory) : base(dBContext)
        {
            this.clientFactory = clientFactory;
        }

        public async Task SendAsync(SendBtcRequest sendBtcRequest)
        {
            var wallet = await GetWallet(sendBtcRequest);
            var bitcoinClient = clientFactory.Create(wallet.RPCAddress, wallet.RPCUsername, wallet.RPCPassword);
            var fee = await bitcoinClient.LoadEstimateSmartFee();
            var privateKey = await LoadWalletPrivateKeys(bitcoinClient, wallet);
            var txHash = await CreateAndSignTransaction(bitcoinClient, wallet, new[] { privateKey }, sendBtcRequest);

            using (var tx = DBContext.BeginTransaction(System.Data.IsolationLevel.Serializable))
            {
                try
                {
                    var payment = new Models.OutcomeTransaction
                    {
                        Amount = sendBtcRequest.Amount+ fee.Feerate,
                        Recipient = sendBtcRequest.Account,
                        WalletId = wallet.Id,
                        TxHash = txHash
                    };

                    wallet.Withdraw(sendBtcRequest.Amount- fee.Feerate);

                    await DBContext.Update(wallet);
                    await DBContext.Add(payment);

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
        }

        private async Task<string> LoadWalletPrivateKeys(BitcoinClient bitcoinClient, HotWallet wallet)
        {
            return await bitcoinClient.LoadWalletPrivateKeys(wallet.Address, wallet.Passphrase);
        }

        private async Task<HotWallet> GetWallet(SendBtcRequest sendBtcRequest)
        {
            var wallet = await DBContext.GetFirstWithBalanceMoreThan(sendBtcRequest.Amount);

            if (wallet != null)
            {
                return wallet;
            }

            throw new InvalidOperationException($"No any wallet with the cache balance more or equal {sendBtcRequest.Amount}");
        }

        private async Task<string> CreateAndSignTransaction(BitcoinClient bitcoinClient, HotWallet w, string[] privateKeys, SendBtcRequest sendBtcRequest)
        {
            var unspent = await bitcoinClient.GetUnspentTransactionOutputs(w.Address, sendBtcRequest.Amount);
            var rawTx = await bitcoinClient.CreateTransaction(w.Address, unspent, sendBtcRequest);
            var signed = await bitcoinClient.SignTransaction(unspent, privateKeys, rawTx);

            return signed;
        }
    }
}