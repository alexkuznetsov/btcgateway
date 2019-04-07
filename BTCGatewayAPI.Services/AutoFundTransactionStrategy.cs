using BTCGatewayAPI.Infrastructure;
using BTCGatewayAPI.Models;
using BTCGatewayAPI.Models.Requests;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BTCGatewayAPI.Services
{
    public class AutoFundTransactionStrategy : FundTransactionStrategy
    {
        public AutoFundTransactionStrategy(BitcoinClient bitcoinClient, GlobalConf conf) : base(bitcoinClient, conf)
        {
        }

        public override async Task<(string, decimal)> CreateAndSignTransaction(HotWallet hotWallet, SendBtcRequest sendBtcRequest)
        {
            var outputs = new Dictionary<string, decimal>
            {
                [sendBtcRequest.Account] = sendBtcRequest.Amount
            };

            var inputs = new Bitcoin.Models.TXInfo[] { };
            var options = new Bitcoin.Models.FundRawTransactionOptions
            {
                ChangeAddress = hotWallet.Address
            };
            var rawTxStage1 = await bitcoinClient.CreateTransaction(inputs, outputs);
            var rawTxStage2 = await bitcoinClient.FundRawTransaction(rawTxStage1, options);
            var privateKey = await bitcoinClient.LoadWalletPrivateKeys(hotWallet.Address
                , hotWallet.Passphrase, conf.WalletUnlockTime);
            var signed = await bitcoinClient.SignRawTransactionWithKey(new Bitcoin.Models.Unspent[] { }
                , new string[] { privateKey }
                , rawTxStage2.Hex);

            return (signed.Hex, rawTxStage2.Fee);
        }
    }
}