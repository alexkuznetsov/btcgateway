using BTCGatewayAPI.Bitcoin;
using BTCGatewayAPI.Bitcoin.Models;
using BTCGatewayAPI.Models.Requests;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace BTCGatewayAPI.Services
{
    public class BitcoinClient
    {
        private readonly RPCServer rpcServer;
        private readonly int confTargetForEstimateSmartFee;

        public BitcoinClient(RPCServer rpcServer, int confTargetForEstimateSmartFee)
        {
            this.rpcServer = rpcServer;
            this.confTargetForEstimateSmartFee = confTargetForEstimateSmartFee;
        }

        public async Task<string> CreateTransaction(TXInfo[] inputs, Dictionary<string, decimal> outputs)
        {
            return await rpcServer.CreateRawtransaction(inputs, outputs);
        }

        public async Task<List<WalletTransaction>> ListTransactions(int count = 0, int skip = 0, bool includeWatchOnly = false)
        {
            return await rpcServer.ListTransactions("*", count, skip, includeWatchOnly);
        }

        public async Task<SignTransactionResult> SignRawTransactionWithKey(Unspent[] outputsRaw, string[] privateKeys, string rawTxHash)
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

            return await rpcServer.SignRawTransactionWithKey(rawTxHash, privateKeys, outputs.ToArray());
        }

        public async Task<string> LoadWalletPrivateKeys(string address, string passphrase, int seconds = 10)
        {
            await rpcServer.WalletPassphrase(passphrase, seconds);
            return await rpcServer.DumpPrivKey(address);
        }

        public async Task<string> SendRawTransaction(string txHash)
        {
            return await rpcServer.SendRawTransaction(txHash);
        }

        public async Task<List<Unspent>> LoadUnspentForAddress(string address)
        {
            return await rpcServer.ListUnspent(address);
        }

        public async Task<EstimateSmartFee> LoadEstimateSmartFee()
        {
            return await rpcServer.EstimateSmartFee(confTargetForEstimateSmartFee);
        }

        public async Task<string> RemovePrunedFunds(string txHash)
        {
            return await rpcServer.RemovePrunedFunds(txHash);
        }

        public async Task<FundRawTransactionResult> FundRawTransaction(string txHash, FundRawTransactionOptions options)
        {
            return await rpcServer.FundRawTransaction(txHash, options);
        }
    }
}