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

        public override async Task<FundTransactionStrategyResult> CreateAndSignTransactionAsync(HotWallet hotWallet, SendBtcRequest sendBtcRequest)
        {
            var outputs = new Dictionary<string, decimal>
            {
                [sendBtcRequest.Account] = sendBtcRequest.Amount
            };

            var inputs = new Bitcoin.Models.TXInfo[] { };
            var options = new Bitcoin.Models.FundRawTransactionOptions { ChangeAddress = hotWallet.Address };
            var rawTxStage1 = await bitcoinClient.CreateRawtransactionAsync(inputs, outputs);
            var rawTxStage2 = await bitcoinClient.FundRawTransactionAsync(rawTxStage1, options);
            var privateKey = await bitcoinClient.LoadWalletPrivateKeysAsync(hotWallet.Address
                , hotWallet.Passphrase, conf.WalletUnlockTime);
            var signed = await bitcoinClient.SignRawTransactionWithKeyAsync(new Bitcoin.Models.Unspent[] { }
                , new string[] { privateKey }
                , rawTxStage2.Hex);
            var txInfo = await bitcoinClient.DecodeRawTransaction(signed.Hex);

            return new FundTransactionStrategyResult(signed.Hex, rawTxStage2.Fee, txInfo.Txid);
        }
    }
}