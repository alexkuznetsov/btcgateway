using BTCGatewayAPI.Infrastructure.Container;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BTCGatewayAPI.Tests
{
    [TestClass]
    public class UnitTestsForContainer_Config_And_CSString
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
        public void Test_IsConfigurationLoadedSuccessefully()
        {
            var conf = _container.Create<Infrastructure.GlobalConf>();
            Assert.IsNotNull(conf);
            Assert.IsNotNull(conf.ConnectionString);
            Assert.IsFalse(string.IsNullOrEmpty(conf.DefaultSQLCS), "Default connection string name");
            Assert.IsFalse(string.IsNullOrEmpty(conf.ConnectionString.ConnectionString), "Connection string settings");
            Assert.IsTrue(conf.ConfTargetForEstimateSmartFee >= 0);
        }
    }
}
