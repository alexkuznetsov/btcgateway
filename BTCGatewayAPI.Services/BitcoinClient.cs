using BTCGatewayAPI.Bitcoin;
using BTCGatewayAPI.Bitcoin.Models;
using BTCGatewayAPI.Models.Requests;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.Net.Http;

namespace BTCGatewayAPI.Services
{

    public class BitcoinClientFactory
    {
        private readonly int confTargetForEstimateSmartFee;
        private readonly DelegatingHandler sharedHadler;

        public BitcoinClientFactory(int confTargetForEstimateSmartFee, DelegatingHandler sharedHadler)
        {
            this.confTargetForEstimateSmartFee = confTargetForEstimateSmartFee;
            this.sharedHadler = sharedHadler;
        }

        public BitcoinClient Create(Uri uri, string username, string password)
        {
            var server = new RPCServer(sharedHadler, uri, username, password);
            return new BitcoinClient(server, confTargetForEstimateSmartFee);
        }
    }

    public class BitcoinClient
    {
        private readonly RPCServer rpcServer;
        private readonly int confTargetForEstimateSmartFee;

        public BitcoinClient(RPCServer rpcServer, int confTargetForEstimateSmartFee)
        {
            this.rpcServer = rpcServer;
            this.confTargetForEstimateSmartFee = confTargetForEstimateSmartFee;
        }

        public async Task<string> CreateTransaction(string address, Unspent[] unspentForWallet, SendBtcRequest sendBtcRequest)
        {
            var totalUnspentSum = unspentForWallet.Sum(x => x.Amount);
            var spentResponse = await LoadEstimateSmartFee();//Т.е. мы уже потратили, это комиссия на транзацкию
            var spent = spentResponse.Feerate;

            if (totalUnspentSum < (sendBtcRequest.Amount + spent))
                throw new InvalidOperationException("Operation not allowed, there are not enouth money for transaction");

            var tx = new List<TXInfo>();
            var reciviers = new List<FundRecivier>();
            var change = 0M;

            foreach (var u in unspentForWallet)
            {
                tx.Add(u);
                spent += u.Amount;

                if (spent >= sendBtcRequest.Amount)
                {
                    change = spent - sendBtcRequest.Amount;
                    break;
                }
            }

            reciviers.Add(new FundRecivier
            {
                Address = sendBtcRequest.Account,
                Amount = sendBtcRequest.Amount
            });

            if (change > 0M)
            {
                reciviers.Add(new FundRecivier
                {
                    Address = address,
                    Amount = change
                });
            }

            return await rpcServer.CreateRawtransaction(tx.ToArray(), reciviers.ToArray());
        }

        public async Task<string> SignTransaction(Unspent[] outputsRaw, string[] privateKeys, string rawTxHash)
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

            return await rpcServer.SignTransaction(rawTxHash, privateKeys, outputs.ToArray());
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

        public async Task<Unspent[]> GetUnspentTransactionOutputs(string address, decimal minimalFunds)
        {
            var unspentForWallet = await LoadUnspentForAddress(address);
            var lessers = unspentForWallet.Where(x => x.Amount < minimalFunds);
            var greaters = unspentForWallet.Where(x => x.Amount >= minimalFunds);

            if (greaters.Any())
            {
                return new[]
                {
                    greaters.OrderBy(x => x.Amount).First()
                };
            }

            var result = new List<Unspent>();
            var sum = 0M;

            foreach (var u in lessers.OrderByDescending(x => x.Amount))
            {
                result.Add(u);
                sum += u.Amount;

                if (sum >= minimalFunds)
                    break;
            }

            return result.ToArray();
        }
    }
}