using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using BTCGatewayAPI.Common.Authentication;
using BTCGatewayAPI.Services;
using BTCGatewayAPI.Common;
using BTCGatewayAPI.Bitcoin;
using System.Net.Http;
using BTCGatewayAPI.Common.DbProviderFactories;
using System.Data.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using IBackgroundService = Microsoft.Extensions.Hosting.IHostedService;
using BTCGatewayAPI.Services.Accounts;
using BTCGatewayAPI.Services.Payments;
using BTCGatewayAPI.Services.Background;

namespace BTCGatewayAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();

            services
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                //.AddMvcOptions(options => options.Filters.Add(typeof(AuthorizationFilter)))
                ;

            services.AddJwt();

            #region Config

            services.AddSingleton(Configuration.GetOptions<GlobalConf>(nameof(GlobalConf)));

            #endregion

            #region DbConnection

            services.AddDbConnection();

            #endregion

            #region Bitcond

            services.AddSingleton((r) => new LoggingHandler(new HttpClientHandler()));

            services.AddSingleton((r) =>
            {
                var handler = r.GetRequiredService<LoggingHandler>();
                var conf = r.GetRequiredService<GlobalConf>();

                return new BitcoinClientFactory(handler, new BitcoinClientOptions
                {
                    ConfTargetForEstimateSmartFee = conf.ConfTargetForEstimateSmartFee,
                    DefaultWalletUnlockTime = conf.DefaultWalletUnlockTime
                });
            });

            #endregion

            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();

            #region Services

            services.AddTransient<CheckClientService>();
            services.AddTransient<HotWalletsService>();
            services.AddTransient<SyncBTCTransactinsService>();
            services.AddTransient<TransactionsService>();

            services.AddSingleton<IPasswordHasher, PasswordHasher>();
            services.AddTransient<IPaymentService, PaymentServiceV2>();
            services.AddSingleton<IBackgroundService, UpdatorService>();

            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseAccessTokenValidator(/*"/auth/login"*/);
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
