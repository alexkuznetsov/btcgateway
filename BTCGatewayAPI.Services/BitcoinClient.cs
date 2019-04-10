using BTCGatewayAPI.Bitcoin;
using BTCGatewayAPI.Bitcoin.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace BTCGatewayAPI.Services
{
    public class BitcoinClient : RPCServer
    {
        private readonly Infrastructure.GlobalConf _conf;

        public BitcoinClient(DelegatingHandler sharedHandler, Infrastructure.GlobalConf conf, Uri address, string username, string password)
            : base(sharedHandler, address, username, password)
        {
            _conf = conf;
        }

        public Task<List<Transaction>> ListTransactionsAsync(int count = 0, int skip = 0, bool includeWatchOnly = false)
        {
            return ListTransactionsAsync("*", count, skip, includeWatchOnly);
        }

        public Task<SignTransactionResult> SignRawTransactionWithKeyAsync(Unspent[] outputsRaw, string[] privateKeys, string rawTxHash)
        {
            var outputs = new List<TxOutput>();

            for (uint i = 0; i < outputsRaw.Length; i++)
            {
                outputs.Add(new TxOutput
                {
                    RedeemScript = outputsRaw[i].RedeemScript,
                    ScriptPubKey = outputsRaw[i].ScriptPubKey,
                    Txid = outputsRaw[i].Txid,
                    Vout = outputsRaw[i].Vout
                });
            }

            return SignRawTransactionWithKeyAsync(rawTxHash, privateKeys, outputs.ToArray());
        }

        public async Task<string> LoadWalletPrivateKeysAsync(string address, string passphrase, int seconds = 10)
        {
            await WalletPassphraseAsync(passphrase, seconds);
            return await DumpPrivKeyAsync(address);
        }

        public Task<EstimateSmartFee> LoadEstimateSmartFeeAsync()
        {
            return EstimateSmartFeeAsync(_conf.ConfTargetForEstimateSmartFee);
        }
    }
}