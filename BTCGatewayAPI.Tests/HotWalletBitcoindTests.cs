using BTCGatewayAPI.Infrastructure.Container;
using BTCGatewayAPI.Services.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace BTCGatewayAPI.Tests
{

    [TestClass]
    public class HotWalletBitcoindTests
    {
        protected static ObjectFactory _container;

        [ClassInitialize]
        public static void Initialize(TestContext testContext)
        {
            if (_container == null)
            {
                _container = ObjectRegistryConfig.Configure();
            }
        }

        [ClassCleanup]
        public static void Shutdown()
        {
            if (_container != null)
            {
                ObjectRegistryConfig.Shutdown();
                _container = null;
            }
        }


        [TestMethod]
        public async Task Test_GetWalletAndFeeRate()
        {
            var dbContext = _container.Create<DbConnection>();
            var wallet = await dbContext.GetFirstWithBalanceMoreThanAsync(0.001M);

            Assert.IsNotNull(wallet);

            var bitcoinClientFactory = _container.Create<Services.BitcoinClientFactory>();

            Assert.IsNotNull(bitcoinClientFactory);

            var btcClient = bitcoinClientFactory.Create(new Uri(wallet.RPCAddress), wallet.RPCUsername, wallet.RPCPassword);
            var fee = await btcClient.LoadEstimateSmartFeeAsync();

            Assert.IsNotNull(fee);
            Assert.IsNotNull(fee.Feerate);

            dbContext.Dispose();
        }

        [TestMethod]
        public async Task Test_GetWalletAndAllUnspentForWalletAddress()
        {
            var dbContext = _container.Create<DbConnection>();
            var wallet = await dbContext.GetFirstWithBalanceMoreThanAsync(0.001M);

            Assert.IsNotNull(wallet);

            var bitcoinClientFactory = _container.Create<Services.BitcoinClientFactory>();

            Assert.IsNotNull(bitcoinClientFactory);

            var btcClient = bitcoinClientFactory.Create(new Uri(wallet.RPCAddress), wallet.RPCUsername, wallet.RPCPassword);
            var unspents = await btcClient.ListUnspentAsync(wallet.Address);

            Assert.IsNotNull(unspents);
            Assert.IsTrue(unspents.Count > 0);
            Assert.IsTrue(string.Compare(unspents[0].Address, wallet.Address) == 0);

            dbContext.Dispose();
        }

        [TestMethod]
        public async Task Test_GetWalletAndMinimunUnspentForAmount()
        {
            var conf = _container.Create<Infrastructure.GlobalConf>();
            var dbContext = _container.Create<DbConnection>();
            var wallet = await dbContext.GetFirstWithBalanceMoreThanAsync(0.001M);

            Assert.IsNotNull(wallet);

            var bitcoinClientFactory = _container.Create<Services.BitcoinClientFactory>();

            Assert.IsNotNull(bitcoinClientFactory);

            var amountToTransfert = 0.0025M;
            var btcClient = bitcoinClientFactory.Create(new Uri(wallet.RPCAddress), wallet.RPCUsername, wallet.RPCPassword);
            var strategy = new Services.ManualFundTransactionStrategy(btcClient, conf);
            var unspents = await strategy.GetUnspentTransactionOutputsAsync(wallet.Address, amountToTransfert);

            Assert.IsNotNull(unspents);
            Assert.IsTrue(unspents.Count() > 0);
            Assert.IsTrue(string.Compare(unspents[0].Address, wallet.Address) == 0);
            Assert.IsTrue(unspents.Sum(x => x.Amount) > amountToTransfert);

            dbContext.Dispose();
        }

        [TestMethod]
        public async Task Test_GetWalletPrivateKey()
        {
            var dbContext = _container.Create<DbConnection>();
            var wallet = await dbContext.GetFirstWithBalanceMoreThanAsync(0.001M);

            Assert.IsNotNull(wallet);

            var bitcoinClientFactory = _container.Create<Services.BitcoinClientFactory>();

            Assert.IsNotNull(bitcoinClientFactory);

            var btcClient = bitcoinClientFactory.Create(new Uri(wallet.RPCAddress), wallet.RPCUsername, wallet.RPCPassword);
            var privateKey = await btcClient.LoadWalletPrivateKeysAsync(wallet.Address, wallet.Passphrase);

            Assert.IsFalse(string.IsNullOrEmpty(privateKey));
            //Assert.IsTrue(string.Compare(privateKey, "cP4BA85Vv5BX9nvuY9oZa4S5xFPaZMwv68tNoAWesYSy8KzG7Ji9") == 0);

            dbContext.Dispose();
        }
    }




}
