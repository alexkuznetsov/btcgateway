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

        public async Task<List<Unspent>> ListUnspent(string address)
        {
            return await ExecuteRequest<ListUnspent, ListUnspentResponse, List<Unspent>>(new Requests.ListUnspent(addresses: new string[] { address }))
                .ConfigureAwait(false);
        }

        public async Task<string> RemovePrunedFunds(string txHash)
        {
            return await ExecuteRequest<RemovePrunedFundsRequest, RemovePrunedFundsResponse, string>(new Requests.RemovePrunedFundsRequest(txHash))
                .ConfigureAwait(false);
        }

        public async Task<string> SendRawTransaction(string txHash)
        {
            return await ExecuteRequest<SendRawTransactionRequest, SendRawTransactionResponse, string>(new Requests.SendRawTransactionRequest(txHash))
                .ConfigureAwait(false);
        }

        public async Task<EstimateSmartFee> EstimateSmartFee(int confTarget)
        {
            return await ExecuteRequest<GetEstimateSmartFeeRequest, GetEstimateSmartFeeResponce, EstimateSmartFee>(new Requests.GetEstimateSmartFeeRequest(confTarget))
                .ConfigureAwait(false);
        }

        public async Task<string> CreateRawtransaction(TXInfo[] inputs, Dictionary<string, decimal> outputs)
        {
            return await ExecuteRequest<CreateRawTransaction, CreateRawTransactionResponse, string>(new Requests.CreateRawTransaction(inputs/*AsStr*/, outputs/*AsStr*/))
                .ConfigureAwait(false);
        }

        public async Task<SignTransactionResult> SignRawTransactionWithKey(string transaxtionHash, string[] privateKeys, TxOutput[] outouts)
        {
            var request = new Requests.SignTransactionRequest(transaxtionHash, privateKeys, outouts);

            return await ExecuteRequest<SignTransactionRequest, SignTransactionResponse, SignTransactionResult>(request)
                .ConfigureAwait(false);
        }

        public async Task<string> WalletPassphrase(string passphrase, int seconds)
        {
            return await ExecuteRequest<WalletPassphraseRequest, WalletPassphraseResponse, string>(new Requests.WalletPassphraseRequest(passphrase, seconds))
                .ConfigureAwait(false);
        }

        public async Task<string> DumpPrivKey(string address)
        {
            return await ExecuteRequest<DumpPrivKeyRequest, DumpPrivKeyResponse, string>(new Requests.DumpPrivKeyRequest(address))
                .ConfigureAwait(false);
        }

        public async Task<List<WalletTransaction>> ListTransactions(string dummy, int count, int skip, bool includeWatchonly)
        {
            return await ExecuteRequest<ListTransactionRequest, ListTransactionResponse, List<WalletTransaction>>(
                new Requests.ListTransactionRequest(dummy, count, skip, includeWatchonly))
                .ConfigureAwait(false);
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

            throw new InvalidOperationException($"Method execution error: {responseObject.Error.Message} [{responseObject.Error.Code}]");
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
