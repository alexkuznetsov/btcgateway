using BTCGatewayAPI.Infrastructure.Container;
using BTCGatewayAPI.Services.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace BTCGatewayAPI.Tests
{
    [TestClass]
    public class IncomeTransactionDbContextTests
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
        public async Task Test_GetNewIncomeTransactionsAsync()
        {
            var dbContext = _container.Create<DbConnection>();
            Assert.IsNotNull(dbContext);

            if (dbContext.State != System.Data.ConnectionState.Open)
                await dbContext.OpenAsync();


            using (var tx = dbContext.BeginTransaction())
            {
                var txList = await dbContext.GetNewIncomeTransactionsAsync(tx, 3);

                Assert.IsNotNull(txList);
                Assert.IsTrue(txList.Count() >= 0);
                tx.Rollback();
            }

            dbContext.Dispose();
        }
    }
}
