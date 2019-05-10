using BTCGatewayAPI.Bitcoin;
using BTCGatewayAPI.Common;
using BTCGatewayAPI.Models;
using BTCGatewayAPI.Models.Requests;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BTCGatewayAPI.Services
{
    public sealed class AutoFundTransactionStrategy : FundTransactionStrategy
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
            var rawTxStage1 = await BitcoinClient.CreateRawtransactionAsync(inputs, outputs);
            var rawTxStage2 = await BitcoinClient.FundRawTransactionAsync(rawTxStage1, options);
            var privateKey = await BitcoinClient.LoadWalletPrivateKeysAsync(hotWallet.Address
                , hotWallet.Passphrase, Conf.WalletUnlockTime);
            var signed = await BitcoinClient.SignRawTransactionWithKeyAsync(new Bitcoin.Models.Unspent[] { }
                , new string[] { privateKey }
                , rawTxStage2.Hex);
            var txInfo = await BitcoinClient.DecodeRawTransaction(signed.Hex);

            return new FundTransactionStrategyResult(signed.Hex, rawTxStage2.Fee, txInfo.Txid);
        }
    }
}