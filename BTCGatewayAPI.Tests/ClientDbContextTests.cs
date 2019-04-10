using BTCGatewayAPI.Infrastructure.Container;
using BTCGatewayAPI.Services.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Common;
using System.Threading.Tasks;

namespace BTCGatewayAPI.Tests
{
    [TestClass]
    public class ClientDbContextTests
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
        public async Task Test_GetExistsClientByLogin()
        {
            var dbContext = _container.Create<DbConnection>();
            Assert.IsNotNull(dbContext);

            if (dbContext.State != System.Data.ConnectionState.Open)
                await dbContext.OpenAsync();


            var client = await dbContext.FindClientByUserNameAsync("rex");

            Assert.IsNotNull(client);
            Assert.AreEqual(client.ClientId, "rex");

            dbContext.Dispose();
        }

        [TestMethod]
        public async Task Test_GetNotExistsClientByLogin()
        {
            var dbContext = _container.Create<DbConnection>();
            Assert.IsNotNull(dbContext);

            if (dbContext.State != System.Data.ConnectionState.Open)
                await dbContext.OpenAsync();


            var client = await dbContext.FindClientByUserNameAsync("rex1");

            Assert.IsNull(client);

            dbContext.Dispose();
        }
    }
}
