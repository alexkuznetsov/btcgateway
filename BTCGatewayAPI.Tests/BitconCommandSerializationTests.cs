using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BTCGatewayAPI.Tests
{
    [TestClass]
    public class BitconCommandSerializationTests
    {
        [TestMethod]
        public void Test_CreateRawTransactionSerializeTest()
        {
            var inputs = new Bitcoin.Models.TXInfo[] { new Bitcoin.Models.TXInfo { Txid = "1234560", Vout = 1 } };
            var req = new Bitcoin.Requests.CreateRawTransaction(inputs, new System.Collections.Generic.Dictionary<string, decimal>
            {
                ["a"] = 0.00001M
            });

            var txt = Newtonsoft.Json.JsonConvert.SerializeObject(req);
            var expected = "{\"jsonrpc\":\"1.0\",\"id\":\"" + req.Id + "\",\"method\":\"createrawtransaction\",\"params\":[[{\"txid\":\"1234560\",\"vout\":1}],{\"a\":0.00001}]}";
            Assert.IsFalse(string.IsNullOrEmpty(txt));
            Assert.IsTrue(txt.Length > 4);
            Assert.IsTrue(txt.Contains("["));
            Assert.IsTrue(txt.Contains("]"));
            Assert.IsTrue(txt.Contains("\"a\""));
            Assert.IsTrue(txt.Contains("0.00001"));
            Assert.IsTrue(string.Compare(txt, expected) == 0);
        }

        [TestMethod]
        public void Test_DecodeRawTransactionRequestSerializeTest()
        {
            var req = new Bitcoin.Requests.DecodeRawTransactionRequest("hash", false);
            var txt = Newtonsoft.Json.JsonConvert.SerializeObject(req);
            var expected = "{\"jsonrpc\":\"1.0\",\"id\":\"" + req.Id + "\",\"method\":\"decoderawtransaction\",\"params\":[\"hash\"]}";
            Assert.IsFalse(string.IsNullOrEmpty(txt));
            Assert.IsTrue(txt.Length > 4);
            Assert.IsTrue(txt.Contains("\"hash\""));
            Assert.IsTrue(string.Compare(txt, expected) == 0);
        }

        [TestMethod]
        public void Test_DumpPrivKeyRequestSerializeTest()
        {
            var req = new Bitcoin.Requests.DumpPrivKeyRequest("address");
            var txt = Newtonsoft.Json.JsonConvert.SerializeObject(req);
            var expected = "{\"jsonrpc\":\"1.0\",\"id\":\"" + req.Id + "\",\"method\":\"dumpprivkey\",\"params\":[\"address\"]}";
            Assert.IsFalse(string.IsNullOrEmpty(txt));
            Assert.IsTrue(txt.Length > 4);
            Assert.IsTrue(txt.Contains("\"address\""));
            Assert.IsTrue(string.Compare(txt, expected) == 0);
        }

        [TestMethod]
        public void Test_FundRawTransactionRequestSerializeTest()
        {
            var options = new Bitcoin.Models.FundRawTransactionOptions { ChangeAddress = "chad" };
            var req = new Bitcoin.Requests.FundRawTransactionRequest("address", options);
            var txt = Newtonsoft.Json.JsonConvert.SerializeObject(req);
            var expected = "{\"jsonrpc\":\"1.0\",\"id\":\"" + req.Id + "\",\"method\":\"fundrawtransaction\",\"params\":[\"address\",{\"changeAddress\":\"chad\"}]}";
            Assert.IsFalse(string.IsNullOrEmpty(txt));
            Assert.IsTrue(txt.Length > 4);
            Assert.IsTrue(txt.Contains("\"address\""));
            Assert.IsTrue(string.Compare(txt, expected) == 0);
        }

        [TestMethod]
        public void Test_GetEstimateSmartFeeRequestSerializeTest()
        {
            var req = new Bitcoin.Requests.GetEstimateSmartFeeRequest(6);
            var txt = Newtonsoft.Json.JsonConvert.SerializeObject(req);
            var expected = "{\"jsonrpc\":\"1.0\",\"id\":\"" + req.Id + "\",\"method\":\"estimatesmartfee\",\"params\":[6]}";
            Assert.IsFalse(string.IsNullOrEmpty(txt));
            Assert.IsTrue(txt.Length > 4);
            Assert.IsTrue(txt.Contains("[6]"));
            Assert.IsTrue(string.Compare(txt, expected) == 0);
        }

        [TestMethod]
        public void Test_GetRawTransactionRequestSerializeTest()
        {
            var req = new Bitcoin.Requests.GetRawTransactionRequest("txid", false);
            var txt = Newtonsoft.Json.JsonConvert.SerializeObject(req);
            var expected = "{\"jsonrpc\":\"1.0\",\"id\":\"" + req.Id + "\",\"method\":\"getrawtransaction\",\"params\":[\"txid\",false]}";
            Assert.IsFalse(string.IsNullOrEmpty(txt));
            Assert.IsTrue(txt.Length > 4);
            Assert.IsTrue(txt.Contains("\"txid\""));
            Assert.IsTrue(string.Compare(txt, expected) == 0);
        }

        [TestMethod]
        public void Test_ListTransactionRequestSerializeTest()
        {
            var req = new Bitcoin.Requests.ListTransactionRequest("*", 0, 99999, false);
            var txt = Newtonsoft.Json.JsonConvert.SerializeObject(req);
            var expected = "{\"jsonrpc\":\"1.0\",\"id\":\"" + req.Id + "\",\"method\":\"listtransactions\",\"params\":[]}";
            Assert.IsFalse(string.IsNullOrEmpty(txt));
            Assert.IsTrue(txt.Length > 4);
            Assert.IsTrue(txt.Contains("[]"));
            Assert.IsTrue(string.Compare(txt, expected) == 0);
        }

        [TestMethod]
        public void Test_ListUnspentSerializeTest()
        {
            var req = new Bitcoin.Requests.ListUnspent(0, 99999, new[] { "address" });
            var txt = Newtonsoft.Json.JsonConvert.SerializeObject(req);
            var expected = "{\"jsonrpc\":\"1.0\",\"id\":\"" + req.Id + "\",\"method\":\"listunspent\",\"params\":[0,99999,[\"address\"]]}";
            Assert.IsFalse(string.IsNullOrEmpty(txt));
            Assert.IsTrue(txt.Length > 4);
            Assert.IsTrue(txt.Contains("[0,99999,[\"address\"]]"));
            Assert.IsTrue(string.Compare(txt, expected) == 0);
        }

        [TestMethod]
        public void Test_RemovePrunedFundsRequestSerializeTest()
        {
            var req = new Bitcoin.Requests.RemovePrunedFundsRequest("hash");
            var txt = Newtonsoft.Json.JsonConvert.SerializeObject(req);
            var expected = "{\"jsonrpc\":\"1.0\",\"id\":\"" + req.Id + "\",\"method\":\"removeprunedfunds\",\"params\":[\"hash\"]}";
            Assert.IsFalse(string.IsNullOrEmpty(txt));
            Assert.IsTrue(txt.Length > 4);
            Assert.IsTrue(txt.Contains("[\"hash\"]"));
            Assert.IsTrue(string.Compare(txt, expected) == 0);
        }

        [TestMethod]
        public void Test_SendRawTransactionRequestSerializeTest()
        {
            var req = new Bitcoin.Requests.SendRawTransactionRequest("hash");
            var txt = Newtonsoft.Json.JsonConvert.SerializeObject(req);
            var expected = "{\"jsonrpc\":\"1.0\",\"id\":\"" + req.Id + "\",\"method\":\"sendrawtransaction\",\"params\":[\"hash\"]}";
            Assert.IsFalse(string.IsNullOrEmpty(txt));
            Assert.IsTrue(txt.Length > 4);
            Assert.IsTrue(txt.Contains("[\"hash\"]"));
            Assert.IsTrue(string.Compare(txt, expected) == 0);
        }

        [TestMethod]
        public void Test_SignTransactionRequestSerializeTest()
        {
            var output = new[] { new Bitcoin.Models.TxOutput { Amount = 0.0M, RedeemScript = "a", ScriptPubKey = "b", Txid = "tx", Vout = 1 } };
            var req = new Bitcoin.Requests.SignTransactionRequest("hash", new[] { "key" }, output);
            var txt = Newtonsoft.Json.JsonConvert.SerializeObject(req);
            var expected = "{\"jsonrpc\":\"1.0\",\"id\":\""+req.Id+"\",\"method\":\"signrawtransactionwithkey\",\"params\":[\"hash\",[\"key\"],[{\"txid\":\"tx\",\"vout\":1,\"scriptPubKey\":\"b\",\"redeemScript\":\"a\",\"amount\":0.0}],\"ALL\"]}";
            Assert.IsFalse(string.IsNullOrEmpty(txt));
            Assert.IsTrue(txt.Length > 4);
            Assert.IsTrue(txt.Contains("[\"hash\",[\"key\"],[{\"txid\":\"tx\",\"vout\":1,\"scriptPubKey\":\"b\",\"redeemScript\":\"a\",\"amount\":0.0}]"));
            Assert.IsTrue(string.Compare(txt, expected) == 0);
        }

        [TestMethod]
        public void Test_WalletInfoRequestSerializeTest()
        {
            var req = new Bitcoin.Requests.WalletInfoRequest();
            var txt = Newtonsoft.Json.JsonConvert.SerializeObject(req);
            var expected = "{\"jsonrpc\":\"1.0\",\"id\":\"" + req.Id + "\",\"method\":\"getwalletinfo\",\"params\":[]}";
            Assert.IsFalse(string.IsNullOrEmpty(txt));
            Assert.IsTrue(txt.Length > 4);
            Assert.IsTrue(txt.Contains("[]"));
            Assert.IsTrue(string.Compare(txt, expected) == 0);
        }

        [TestMethod]
        public void Test_WalletPassphraseRequestSerializeTest()
        {
            var req = new Bitcoin.Requests.WalletPassphraseRequest("pass", 30);
            var txt = Newtonsoft.Json.JsonConvert.SerializeObject(req);
            var expected = "{\"jsonrpc\":\"1.0\",\"id\":\"" + req.Id + "\",\"method\":\"walletpassphrase\",\"params\":[\"pass\",30]}";
            Assert.IsFalse(string.IsNullOrEmpty(txt));
            Assert.IsTrue(txt.Length > 4);
            Assert.IsTrue(txt.Contains("[\"pass\",30]"));
            Assert.IsTrue(string.Compare(txt, expected) == 0);
        }

        [TestMethod]
        public void Test_WalletTransactionRequestSerializeTest()
        {
            var req = new Bitcoin.Requests.WalletTransactionRequest("hash", false);
            var txt = Newtonsoft.Json.JsonConvert.SerializeObject(req);
            var expected = "{\"jsonrpc\":\"1.0\",\"id\":\"" + req.Id + "\",\"method\":\"gettransaction\",\"params\":[\"hash\",false]}";
            Assert.IsFalse(string.IsNullOrEmpty(txt));
            Assert.IsTrue(txt.Length > 4);
            Assert.IsTrue(txt.Contains("[\"hash\",false]"));
            Assert.IsTrue(string.Compare(txt, expected) == 0);
        }
    }
}
