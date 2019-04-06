﻿using BTCGatewayAPI.Infrastructure.Container;
using BTCGatewayAPI.Services.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace BTCGatewayAPI.Tests
{
    [TestClass]
    public class PaymentServiceTests
    {
        protected static ObjectFactory _container;

        [ClassInitialize]
        public static void Initialize(TestContext testContext)
        {
            if (_container == null)
            {
                _container = BTCGatewayAPI.ObjectRegistryConfig.Configure();
            }
        }

        [ClassCleanup]
        public static void Shutdown()
        {
            if (_container != null)
            {
                BTCGatewayAPI.ObjectRegistryConfig.Shutdown();
                _container = null;
            }
        }


        [TestMethod]
        public async Task Test_CreateTransaction()
        {
            var dbContext = _container.Create(typeof(Infrastructure.DB.DBContext)) as Infrastructure.DB.DBContext;
            var clientFactory = _container.Create(typeof(Services.BitcoinClientFactory)) as Services.BitcoinClientFactory;
            var paymentService = _container.Create(typeof(Services.PaymentService)) as Services.PaymentService;

            Assert.IsNotNull(dbContext);
            Assert.IsNotNull(clientFactory);
            Assert.IsNotNull(paymentService);

            var request = new Models.Requests.SendBtcRequest { Account = "mkHS9ne12qx9pS9VojpwU5xtRd4T7X7ZUt", Amount = 0.01M };
            var wallet = await dbContext.GetFirstWithBalanceMoreThan(request.Amount);

            Assert.IsNotNull(wallet);

            var bitcoinClient = clientFactory.Create(new Uri(wallet.RPCAddress), wallet.RPCUsername, wallet.RPCPassword);

            Assert.IsNotNull(bitcoinClient);

            var tx = await paymentService.CreateAndSignTransaction(bitcoinClient, wallet.Address, wallet.Passphrase, request);

            Assert.IsFalse(string.IsNullOrEmpty(tx));
        }

        [TestMethod]
        public async Task Test_DeserializeSingTransactionResponse()
        {
            using (var str = GetType().Assembly.GetManifestResourceStream("BTCGatewayAPI.Tests.SignedTxResponse.json"))
            using (var r = new System.IO.StreamReader(str))
            {
                var contents = await r.ReadToEndAsync();

                var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<Bitcoin.Requests.SignTransactionResponse>(contents);

                Assert.IsNotNull(obj);
                Assert.IsNotNull(obj.Result);
                Assert.IsNotNull(obj.Result.Hex);
                Assert.IsTrue(obj.Result.Complete);
            }
        }
    }
}