using BTCGatewayAPI.Infrastructure.Container;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace BTCGatewayAPI.Tests
{
    [TestClass]
    public class IncomeTransactinsServiceTests
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
        public async Task Test_DownloadingTransactionsWithoutError()
        {
            var service = _container.Create<Services.SyncBTCTransactinsService>();

            Assert.IsNotNull(service);

            var result = false;

            try
            {
                await service.DownloadAsync();
                result = true;
            }
            catch (Exception ex)
            {

            }

            Assert.IsTrue(result);

            service.Dispose();
        }
    }
}
