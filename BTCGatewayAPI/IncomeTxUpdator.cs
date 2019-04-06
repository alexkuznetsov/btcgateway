using BTCGatewayAPI.Infrastructure;
using BTCGatewayAPI.Services;
using System;
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

        public IncomeTxUpdator(Infrastructure.Container.ObjectFactory objectFactory)
        {
            HostingEnvironment.RegisterObject(this);
            _objectFactory = objectFactory;
            _conf = objectFactory.Create(typeof(Infrastructure.GlobalConf)) as Infrastructure.GlobalConf;
            _timer = new System.Threading.Timer(OnTimerCallback);
            _txDownloader = objectFactory.Create(typeof(Services.SyncBTCTransactinsService)) as Services.SyncBTCTransactinsService;

            StartTimer();
        }

        private async void OnTimerCallback(object state)
        {
            StopTimer();
            await _txDownloader.Download();
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
        }

        public void Stop(bool immediate)
        {
            StopTimer();
            HostingEnvironment.UnregisterObject(this);
        }

    }
}