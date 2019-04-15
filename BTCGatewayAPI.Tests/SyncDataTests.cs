using BTCGatewayAPI.Infrastructure.Container;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace BTCGatewayAPI.Tests
{
    [TestClass]
    public class SyncDataTests
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
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
            }

            Assert.IsTrue(result);

            service.Dispose();
        }

        [TestMethod]
        public async Task Test_DownloadingHotWalletsWithoutError()
        {
            var service = _container.Create<Services.HotWalletsService>();

            Assert.IsNotNull(service);

            var result = false;

            try
            {
                await service.SyncWalletsInformationAsync();
                result = true;
            }
            catch (Exception)
            {

            }

            Assert.IsTrue(result);

            service.Dispose();
        }
    }
}
