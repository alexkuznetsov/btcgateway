#define USE_TESTAPI
#if USE_TESTAPI
using BTCGatewayAPI.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Http;

namespace BTCGatewayAPI.Controllers
{
    [Authorize]
    public class HotWalletsController : ApiController
    {
        private readonly HotWalletsService _hotWalletsService;

        public HotWalletsController(HotWalletsService hotWalletsService)
        {
            this._hotWalletsService = hotWalletsService;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _hotWalletsService.Dispose();
            }

            base.Dispose(disposing);
        }

        [HttpGet]
        [Route("getwallets")]
        public async Task<IHttpActionResult> GetWallets()
        {
            var wallets = await _hotWalletsService.GetAllWalletsAsync();

            return Json(wallets);
        }

        [HttpGet]
        [Route("simple/getwallets")]
        public async Task<IHttpActionResult> GetWalletsDirect()
        {
            var cs = ConfigurationManager.ConnectionStrings["DefaultSQL"].ConnectionString;
            var wallets = new List<Models.HotWallet>();

            using (var dbCon = new System.Data.SqlClient.SqlConnection(cs))
            {
                await dbCon.OpenAsync();

                using (var cmd = dbCon.CreateCommand())
                {
                    cmd.CommandText = "select * from [hot_wallets]";
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var w = new Models.HotWallet
                            {
                                Id = reader.GetInt32(0),
                                CreatedAt = reader.GetDateTime(1),
                                UpdatedAt = reader.GetValue(2) == DBNull.Value ? new DateTime?() : reader.GetDateTime(2),
                                Address = reader.GetString(3),
                                Amount = reader.GetDecimal(4),
                            };

                            wallets.Add(w);
                        }
                    }
                }
            }

            return Json(wallets);
        }
    }
}
#endif