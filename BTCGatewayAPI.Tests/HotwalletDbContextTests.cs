using BTCGatewayAPI.Infrastructure.Container;
using BTCGatewayAPI.Services.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace BTCGatewayAPI.Tests
{
    [TestClass]
    public class HotwalletDbContextTests
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
        public async Task Test_GetFirstWithBalanceMoreThanZero()
        {
            var dbContext = _container.Create<DbConnection>();
            Assert.IsNotNull(dbContext);

            if (dbContext.State != System.Data.ConnectionState.Open)
                await dbContext.OpenAsync();
            
            var wallet = await dbContext.GetFirstWithBalanceMoreThanAsync(0.000M);

            Assert.IsNotNull(wallet);
            Assert.IsTrue(wallet.Amount > 0.000M);

            dbContext.Dispose();
        }

        [TestMethod]
        public async Task Test_GetAllHotWallets()
        {
            var dbContext = _container.Create<DbConnection>();
            Assert.IsNotNull(dbContext);

            if (dbContext.State != System.Data.ConnectionState.Open)
                await dbContext.OpenAsync();

            var wallets = await dbContext.GetAllHotWalletsAsync();

            Assert.IsTrue(wallets.Count() > 0);

            dbContext.Dispose();
        }

        [TestMethod]
        public async Task Test_UpdateWallet()
        {
            var dbContext = _container.Create<DbConnection>();
            Assert.IsNotNull(dbContext);

            if (dbContext.State != System.Data.ConnectionState.Open)
                await dbContext.OpenAsync();

            var amount = 0.00001M;
            var wallet = await dbContext.GetFirstWithBalanceMoreThanAsync(0.000M);

            Assert.IsNotNull(wallet);
            Assert.IsTrue(wallet.Amount > 0.000M);
            
            using (var tx = dbContext.BeginTransaction())
            {
                wallet.Withdraw(amount);
                await dbContext.UpdateWalletAsync(tx, wallet);
            }

            wallet.Amount += amount;

            using (var tx = dbContext.BeginTransaction())
            {
                await dbContext.UpdateWalletAsync(tx, wallet);
            }

            dbContext.Dispose();
        }
    }
}
