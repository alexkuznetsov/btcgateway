﻿using BTCGatewayAPI.Infrastructure.Container;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BTCGatewayAPI.Tests
{
    [TestClass]
    public class HotWalletTests
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
        public async Task Test_CreateFakeHotWallet()
        {
            var dbContext = _container.Create<Infrastructure.DB.DBContext>();
            var hotWallet = await dbContext.FindAsync<Models.HotWallet>("select * from [hot_wallets] where address=@id",
                new KeyValuePair<string, object>("id", "fake"));

            if (hotWallet != null)
            {
                using (var tx = await dbContext.BeginTransactionAsync(System.Data.IsolationLevel.Serializable))
                {
                    try
                    {
                        await dbContext.DeleteAsync(hotWallet);
                        tx.Commit();
                    }
                    catch (Exception)
                    {
                        tx.Rollback();
                        throw;
                    }
                }
            }

            hotWallet = new Models.HotWallet
            {
                Address = "fake",
                Amount = 0.0M,
                CreatedAt = DateTime.Now,
                Passphrase = "fake",
                RPCAddress = "fake",
                Id = 0,
                RPCPassword = "fake",
                RPCUsername = "fake",
                UpdatedAt = null
            };

            Models.HotWallet retVal;

            using (var tx = await dbContext.BeginTransactionAsync(System.Data.IsolationLevel.Serializable))
            {
                try
                {
                    retVal = await dbContext.AddAsync(hotWallet);
                    tx.Commit();
                }
                catch (Exception)
                {
                    tx.Rollback();
                    throw;
                }
            }

            Assert.IsNotNull(retVal);
            Assert.IsTrue(retVal.Address == "fake");
            Assert.IsTrue(retVal.Amount - 0.11111M < 0.0000001M);

            int affected;

            using (var tx = await dbContext.BeginTransactionAsync(System.Data.IsolationLevel.Serializable))
            {
                try
                {
                    affected = await dbContext.DeleteAsync(hotWallet);
                    tx.Commit();
                }
                catch (Exception)
                {
                    tx.Rollback();
                    throw;
                }
            }

            Assert.IsTrue(affected == 1);
        }
    }
}
