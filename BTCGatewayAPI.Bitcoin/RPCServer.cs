using BTCGatewayAPI.Bitcoin.Models;
using BTCGatewayAPI.Bitcoin.Requests;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BTCGatewayAPI.Bitcoin
{
    public class RPCServer
    {
        const string BasicAuth = "Basic";

        private readonly Uri _address;
        private readonly string _basicAuthHeader;
        private readonly DelegatingHandler _sharedHandler;

        public RPCServer(DelegatingHandler sharedHandler, Uri address, string username, string password)
        {
            _address = address;
            _basicAuthHeader = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}"));
            _sharedHandler = sharedHandler;
        }

        public Task<List<Unspent>> ListUnspent(string address)
        {
            return ExecuteRequest<ListUnspent, ListUnspentResponse, List<Unspent>>(new Requests.ListUnspent(addresses: new string[] { address }))
;
        }

        public Task<string> RemovePrunedFunds(string txHash)
        {
            return ExecuteRequest<RemovePrunedFundsRequest, RemovePrunedFundsResponse, string>(new Requests.RemovePrunedFundsRequest(txHash))
;
        }

        public Task<string> SendRawTransaction(string txHash)
        {
            return ExecuteRequest<SendRawTransactionRequest, SendRawTransactionResponse, string>(new Requests.SendRawTransactionRequest(txHash))
;
        }

        public Task<EstimateSmartFee> EstimateSmartFee(int confTarget)
        {
            return ExecuteRequest<GetEstimateSmartFeeRequest, GetEstimateSmartFeeResponce, EstimateSmartFee>(new Requests.GetEstimateSmartFeeRequest(confTarget))
;
        }

        public Task<string> CreateRawtransaction(TXInfo[] inputs, Dictionary<string, decimal> outputs)
        {
            return ExecuteRequest<CreateRawTransaction, CreateRawTransactionResponse, string>(new Requests.CreateRawTransaction(inputs/*AsStr*/, outputs/*AsStr*/))
;
        }

        public Task<SignTransactionResult> SignRawTransactionWithKey(string transaxtionHash, string[] privateKeys, TxOutput[] outouts)
        {
            var request = new Requests.SignTransactionRequest(transaxtionHash, privateKeys, outouts);

            return ExecuteRequest<SignTransactionRequest, SignTransactionResponse, SignTransactionResult>(request)
;
        }

        public Task<string> WalletPassphrase(string passphrase, int seconds)
        {
            return ExecuteRequest<WalletPassphraseRequest, WalletPassphraseResponse, string>(new Requests.WalletPassphraseRequest(passphrase, seconds))
;
        }

        public Task<string> DumpPrivKey(string address)
        {
            return ExecuteRequest<DumpPrivKeyRequest, DumpPrivKeyResponse, string>(new Requests.DumpPrivKeyRequest(address))
;
        }

        public Task<List<WalletTransaction>> ListTransactions(string dummy, int count, int skip, bool includeWatchonly)
        {
            return ExecuteRequest<ListTransactionRequest, ListTransactionResponse, List<WalletTransaction>>(
                new Requests.ListTransactionRequest(dummy, count, skip, includeWatchonly))
;
        }

        public Task<FundRawTransactionResult> FundRawTransaction(string txHash, FundRawTransactionOptions options)
        {
            return ExecuteRequest<FundRawTransactionRequest, FundRawTransactionResponse, FundRawTransactionResult>(
                new Requests.FundRawTransactionRequest(txHash, options))
;
        }
 
        public Task<WalletInfoResult> GetWalletInfo()
        {
            return ExecuteRequest<WalletInfoRequest, WalletInfoResponse, WalletInfoResult>(
                new Requests.WalletInfoRequest())
;
        }

        private async Task<TResponse> ExecuteRequestRaw<TRequest, TResponse>(TRequest command)
        {
            using (var client = GetClient())
            {
                var response = await client.PostAsJsonAsync(_address, command);
                return await response.Content.ReadAsAsync<TResponse>();
            }
        }

        private async Task<TResponseMessage> ExecuteRequest<TRequest, TResponse, TResponseMessage>(TRequest command)
            where TRequest : CommandRequest
            where TResponse : CommandResponse<TResponseMessage>
        {

            var responseObject = await ExecuteRequestRaw<TRequest, TResponse>(command);
            if (responseObject.Error == null)
                return responseObject.Result;

            throw new RPCServerException($"Method execution error: {responseObject.Error.Message} [{responseObject.Error.Code}]");
        }

        private HttpClient GetClient()
        {
            return new HttpClient(_sharedHandler, disposeHandler: false)
            {
                DefaultRequestHeaders =
                {
                    Authorization = new AuthenticationHeaderValue(BasicAuth, _basicAuthHeader)
                },
            };
        }
    }
}
