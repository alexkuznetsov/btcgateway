using BTCGatewayAPI.Infrastructure.Container;
using BTCGatewayAPI.Services.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Common;
using System.Threading.Tasks;

namespace BTCGatewayAPI.Tests
{
    [TestClass]
    public class OutputTransactionsSaveTests
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
        public async System.Threading.Tasks.Task Test_SaveOutputTx_Success()
        {
            var dbContext = _container.Create<DbConnection>();
            if (dbContext.State != System.Data.ConnectionState.Open)
                await dbContext.OpenAsync();

            var paymentService = _container.Create<Services.PaymentService>();
            var sendBtcRequest = new Models.Requests.SendBtcRequest { Account = "123456", Amount = 0.111111M };
            var wallet = await dbContext.GetFirstWithBalanceMoreThanAsync(sendBtcRequest.Amount);
            var walletAmount = wallet.Amount;
            var txHash = "02000000000101f7cf6ef75556555555cecbc117beb31d41f8dba86b47b3d2588926f3bdf0741a6900000017160014ba0d4968a7ac506d39bfb263dbe83a22a27b7161feffffff02c9e772000000000017a9141221c54021858e6726aa98d8028328415957d33287232ea800000000001976a9149f9a7abd600c0caa03983a77c8c3df8e062cb2fa88ac02473044022047bfb63bf33691e74ad313ae4e56f65f4d9ddd5649e7519c84fde24c27ddc6d102200b60df2aa60700ba0a8375da50b9bfc90a0675522b44f4407ab6984e763a8f29012103ac396066cce501a4aadff8370b9b39423fecca6a2f852d33eb0345d699a0cdd700000000";
            var txid = "6494d25f8367437173e8773c6ba063cf66ac04e75365e81364295d91fc9f3dc2";
            var txInfo = new Services.FundTransactionStrategy.FundTransactionStrategyResult(txHash, 0.000001M, txid);

            var result = await paymentService.SaveWalletAndPaymentAsync(sendBtcRequest,
                txInfo, wallet
                , onSuccess: (s) => Task.FromResult(0)
                , onError: (s) => Task.FromResult(0));

            Assert.IsTrue(result.Item1);

            using (var dbtx = dbContext.BeginTransaction())
            {
                var affected = await dbContext.DeleteSendTransactionAsync(dbtx, result.Item2);
                Assert.IsTrue(affected > 0);
            }


            using (var dbtx = dbContext.BeginTransaction())
            {
                wallet.Amount = walletAmount;

                var affected = await dbContext.UpdateWalletAsync(dbtx, wallet);

                Assert.IsTrue(affected > 0);
                Assert.IsTrue(wallet.Amount - walletAmount <= 0.0000000001M);
            }

        }

    }
}
