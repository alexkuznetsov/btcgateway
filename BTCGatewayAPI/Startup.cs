using BTCGatewayAPI.Services;
using Owin;
using System.Threading;
using System.Web.Hosting;
using System.Web.Http;

namespace BTCGatewayAPI
{
    public partial class Startup : IRegisteredObject
    {
        IncomeTxUpdatorService _updator;

        public Startup()
        {
            HostingEnvironment.RegisterObject(this);
        }

        public void Configuration(IAppBuilder app)
        {
            var factory = ObjectRegistryConfig.Configure();
            var configuration = new HttpConfiguration();
            _updator = factory.Create<IncomeTxUpdatorService>();

            ConfigureWebApi(configuration, factory);
            ConfigureOauth(configuration, factory, app);

            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.UseWebApi(configuration);
        }

        public void Stop(bool immediate)
        {
            Thread.Sleep(60);
            _updator?.Dispose();
            ObjectRegistryConfig.Shutdown();
            HostingEnvironment.UnregisterObject(this);
        }
    }
}