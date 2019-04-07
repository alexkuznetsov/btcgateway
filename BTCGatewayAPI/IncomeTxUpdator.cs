using BTCGatewayAPI.Infrastructure;
using BTCGatewayAPI.Services;
using System;
using System.Net;
using System.Threading;
using System.Web.Hosting;

namespace BTCGatewayAPI
{
    public class IncomeTxUpdator : IRegisteredObject, IDisposable
    {
        private bool _disposed;
        private readonly Infrastructure.Container.ObjectFactory _objectFactory;
        private readonly GlobalConf _conf;
        private readonly Timer _timer;
        private readonly SyncBTCTransactinsService _txDownloader;
        private readonly HotWalletsService _hotWalletInfoSync;

        private static readonly Lazy<Infrastructure.Logging.ILogger> LoggerLazy = new Lazy<Infrastructure.Logging.ILogger>(Infrastructure.Logging.LoggerFactory.GetLogger);
        private static Infrastructure.Logging.ILogger Logger => LoggerLazy.Value;

        public IncomeTxUpdator(Infrastructure.Container.ObjectFactory objectFactory)
        {
            HostingEnvironment.RegisterObject(this);
            _objectFactory = objectFactory;
            _conf = objectFactory.Create<GlobalConf>();
            _timer = new Timer(OnTimerCallback);
            _txDownloader = objectFactory.Create<SyncBTCTransactinsService>();
            _hotWalletInfoSync = objectFactory.Create<HotWalletsService>();

            StartTimer();
        }

        private async void OnTimerCallback(object state)
        {
            StopTimer();

            try
            {
                await _hotWalletInfoSync.SyncWalletsInformationAsync();
                await _txDownloader.DownloadAsync();
            }
            catch (WebException ex)
            {
                Logger.Error(ex, "Networking error occurs");
            }
            catch (Exception ex)
            {
                Logger.Fatal(ex, "Unhandled error occurs");
            }

            StartTimer();
        }

        private void StopTimer()
        {
            _timer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        private void StartTimer()
        {
            _timer.Change(0, _conf.TXUpdateTimerInterval);
        }

        #region descruct and idisposable

        ~IncomeTxUpdator()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                OnDispose();

                _disposed = true;
            }

        }

        #endregion

        private void OnDispose()
        {
            _timer.Dispose();
            _txDownloader.Dispose();
            _hotWalletInfoSync.Dispose();
        }

        public void Stop(bool immediate)
        {
            StopTimer();
            HostingEnvironment.UnregisterObject(this);
        }

    }
}