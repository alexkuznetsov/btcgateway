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
    public class RPCServer : IDisposable
    {
        const string BasicAuth = "Basic";

        private readonly Uri _address;
        private readonly string _basicAuthHeader;
        private readonly DelegatingHandler _sharedHandler;
        private bool _disposed;

        public RPCServer(DelegatingHandler sharedHandler, Uri address, string username, string password)
        {
            _address = address;
            _basicAuthHeader = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}"));
            _sharedHandler = sharedHandler;
        }

        #region IDisposable

        ~RPCServer()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                _sharedHandler.Dispose();
                _disposed = true;
            }
        }

        #endregion

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

        public async Task<string> CreateRawtransaction(TXInfo[] inputs, FundRecivier[] outputs)
        {
            return await ExecuteRequest<CreateRawTransaction, CreateRawTransactionResponse, string>(new Requests.CreateRawTransaction(inputs, outputs))
                .ConfigureAwait(false);
        }

        public async Task<string> SignTransaction(string transaxtionHash, string[] privateKeys, TxOutput[] outouts)
        {
            var request = new Requests.SignTransactionRequest(transaxtionHash, privateKeys);

            for (uint i = 0; i < outouts.Length; i++)
            {
                request.AddOutput(outouts[i]);
            }

            return await ExecuteRequest<SignTransactionRequest, SignTransactionResponse, string>(request)
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

        private async Task<TResponseMessage> ExecuteRequest<TRequest, TResponse, TResponseMessage>(TRequest command)
            where TRequest : CommandRequest
            where TResponse : CommandResponse<TResponseMessage>
        {
            using (var client = GetClient())
            {
                var response = await client.PostAsJsonAsync(_address, command);
                var responseObject = await response.Content.ReadAsAsync<TResponse>();

                if (responseObject.Error == null)
                    return responseObject.Result;

                throw new InvalidOperationException($"Method execution error: {responseObject.Error.Message} [{responseObject.Error.Code}]");
            }
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
