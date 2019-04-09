using BTCGatewayAPI.Bitcoin.Models;
using BTCGatewayAPI.Bitcoin.Requests;
using BTCGatewayAPI.Bitcoin.Responses;
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

        public Task<List<Unspent>> ListUnspentAsync(string address)
        {
            return ExecuteRequestAsync<ListUnspent, ListUnspentResponse, List<Unspent>>(new ListUnspent(addresses: new string[] { address }))
;
        }

        public Task<string> RemovePrunedFundsAsync(string txHash)
        {
            return ExecuteRequestAsync<RemovePrunedFundsRequest, RemovePrunedFundsResponse, string>(new RemovePrunedFundsRequest(txHash))
;
        }

        public Task<string> SendRawTransactionAsync(string txHash)
        {
            return ExecuteRequestAsync<SendRawTransactionRequest, SendRawTransactionResponse, string>(new SendRawTransactionRequest(txHash))
;
        }

        public Task<EstimateSmartFee> EstimateSmartFeeAsync(int confTarget)
        {
            return ExecuteRequestAsync<GetEstimateSmartFeeRequest, GetEstimateSmartFeeResponce, EstimateSmartFee>(new GetEstimateSmartFeeRequest(confTarget))
;
        }

        public Task<string> CreateRawtransactionAsync(TXInfo[] inputs, Dictionary<string, decimal> outputs)
        {
            return ExecuteRequestAsync<CreateRawTransaction, CreateRawTransactionResponse, string>(new CreateRawTransaction(inputs/*AsStr*/, outputs/*AsStr*/))
;
        }

        public Task<SignTransactionResult> SignRawTransactionWithKeyAsync(string transaxtionHash, string[] privateKeys, TxOutput[] outouts)
        {
            var request = new SignTransactionRequest(transaxtionHash, privateKeys, outouts);

            return ExecuteRequestAsync<SignTransactionRequest, SignTransactionResponse, SignTransactionResult>(request)
;
        }

        public Task<string> WalletPassphraseAsync(string passphrase, int seconds)
        {
            return ExecuteRequestAsync<WalletPassphraseRequest, WalletPassphraseResponse, string>(new WalletPassphraseRequest(passphrase, seconds))
;
        }

        public Task<string> DumpPrivKeyAsync(string address)
        {
            return ExecuteRequestAsync<DumpPrivKeyRequest, DumpPrivKeyResponse, string>(new DumpPrivKeyRequest(address))
;
        }

        public Task<List<WalletTransaction>> ListTransactionsAsync(string dummy, int count, int skip, bool includeWatchonly)
        {
            return ExecuteRequestAsync<ListTransactionRequest, ListTransactionResponse, List<WalletTransaction>>(
                new ListTransactionRequest(dummy, count, skip, includeWatchonly))
;
        }

        public Task<FundRawTransactionResult> FundRawTransactionAsync(string txHash, FundRawTransactionOptions options)
        {
            return ExecuteRequestAsync<FundRawTransactionRequest, FundRawTransactionResponse, FundRawTransactionResult>(
                new FundRawTransactionRequest(txHash, options))
;
        }

        public Task<WalletInfoResult> GetWalletInfoAsync()
        {
            return ExecuteRequestAsync<WalletInfoRequest, WalletInfoResponse, WalletInfoResult>(
                new WalletInfoRequest())
;
        }

        private async Task<TResponse> ExecuteRequestRawAsync<TRequest, TResponse>(TRequest command)
        {
            using (var client = GetClient())
            {
                var response = await client.PostAsJsonAsync(_address, command);
                return await response.Content.ReadAsAsync<TResponse>();
            }
        }

        private async Task<TResponseMessage> ExecuteRequestAsync<TRequest, TResponse, TResponseMessage>(TRequest command)
            where TRequest : CommandRequest
            where TResponse : CommandResponse<TResponseMessage>
        {
            var responseObject = await ExecuteRequestRawAsync<TRequest, TResponse>(command);
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
