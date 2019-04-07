using BTCGatewayAPI.Bitcoin;
using BTCGatewayAPI.Bitcoin.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BTCGatewayAPI.Services
{
    public class BitcoinClient
    {
        private readonly RPCServer _rpcServer;
        private readonly Infrastructure.GlobalConf _conf;

        public BitcoinClient(RPCServer rpcServer, Infrastructure.GlobalConf conf)
        {
            _rpcServer = rpcServer;
            _conf = conf;
        }

        public Task<string> CreateTransaction(TXInfo[] inputs, Dictionary<string, decimal> outputs)
        {
            return _rpcServer.CreateRawtransaction(inputs, outputs);
        }

        public Task<List<WalletTransaction>> ListTransactions(int count = 0, int skip = 0, bool includeWatchOnly = false)
        {
            return _rpcServer.ListTransactions("*", count, skip, includeWatchOnly);
        }

        public Task<SignTransactionResult> SignRawTransactionWithKey(Unspent[] outputsRaw, string[] privateKeys, string rawTxHash)
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

            return _rpcServer.SignRawTransactionWithKey(rawTxHash, privateKeys, outputs.ToArray());
        }

        public Task<WalletInfoResult> GetWalletInfo()
        {
            return _rpcServer.GetWalletInfo();
        }

        public async Task<string> LoadWalletPrivateKeys(string address, string passphrase, int seconds = 10)
        {
            await _rpcServer.WalletPassphrase(passphrase, seconds);
            return await _rpcServer.DumpPrivKey(address);
        }

        public Task<string> SendRawTransaction(string txHash)
        {
            return _rpcServer.SendRawTransaction(txHash);
        }

        public Task<List<Unspent>> LoadUnspentForAddress(string address)
        {
            return _rpcServer.ListUnspent(address);
        }

        public Task<EstimateSmartFee> LoadEstimateSmartFee()
        {
            return _rpcServer.EstimateSmartFee(_conf.ConfTargetForEstimateSmartFee);
        }

        public Task<string> RemovePrunedFunds(string txHash)
        {
            return _rpcServer.RemovePrunedFunds(txHash);
        }

        public Task<FundRawTransactionResult> FundRawTransaction(string txHash, FundRawTransactionOptions options)
        {
            return _rpcServer.FundRawTransaction(txHash, options);
        }
    }
}