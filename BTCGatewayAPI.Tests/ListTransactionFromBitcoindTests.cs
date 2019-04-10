using BTCGatewayAPI.Infrastructure.Container;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace BTCGatewayAPI.Tests
{
    [TestClass]
    public class ListTransactionFromBitcoindTests
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
        public async System.Threading.Tasks.Task Test_DeserializeListOfTransactionAndEnsureTxIdIsUnuque()
        {
            using (var str = GetType().Assembly.GetManifestResourceStream("BTCGatewayAPI.Tests.ListTransactions.json"))
            using (var r = new System.IO.StreamReader(str))
            {
                var contents = await r.ReadToEndAsync();

                var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<Bitcoin.Responses.ListTransactionResponse>(contents);

                Assert.IsNotNull(obj);
                Assert.IsNotNull(obj.Result);
                Assert.IsTrue(obj.Result.Count>0);

                var txList = obj.Result;
                var idsx = txList.Select(x => x.Txid).Distinct().ToArray();

                Assert.IsTrue(idsx.Count() > 0);

                var groupped = txList.GroupBy(x => x.Txid)
                    .ToDictionary(x => x.Key, x => x.ToList());

                Assert.IsTrue(groupped.Count() > 0);
            }
        }

    }




}
