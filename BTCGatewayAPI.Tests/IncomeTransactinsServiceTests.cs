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
        public async Task Test_DownloadingTransactionsWithoutError()
        {
            var service = _container.Create(typeof(Services.SyncBTCTransactinsService))
                 as Services.SyncBTCTransactinsService;

            Assert.IsNotNull(service);

            var result = false;

            try
            {
                await service.Download();
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
