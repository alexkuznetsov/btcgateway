using BTCGatewayAPI.Common;
using BTCGatewayAPI.Common.Logging;
using Microsoft.Extensions.Hosting;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace BTCGatewayAPI.Services.Background
{
    public class UpdatorService : IHostedService, IDisposable
    {
        private bool _disposed;
        private readonly GlobalConf _conf;
        private readonly Timer _timer;
        private readonly SyncBTCTransactinsService _txDownloader;
        private readonly HotWalletsService _hotWalletInfoSync;

        private static readonly Lazy<ILogger> LoggerLazy = new Lazy<ILogger>(LoggerFactory.GetLogger);

        private static ILogger Logger => LoggerLazy.Value;

        public UpdatorService(GlobalConf conf,
            SyncBTCTransactinsService syncBTCTransactinsService,
            HotWalletsService hotWalletsService)
        {
            _conf = conf;
            _txDownloader = syncBTCTransactinsService;
            _hotWalletInfoSync = hotWalletsService;

            _timer = new Timer(OnTimerCallbackAsync);
        }

        #region descruct and idisposable

        ~UpdatorService()
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
            StopTimer();

            _timer.Dispose();
            _txDownloader.Dispose();
            _hotWalletInfoSync.Dispose();
        }

        private async void OnTimerCallbackAsync(object state)
        {
            StopTimer();

            try
            {
                await _hotWalletInfoSync.SyncWalletsInformationAsync();
                await _txDownloader.DownloadAsync();
            }
            catch (WebException ex)
            {
                Logger.Error(ex, Messages.ErrNetworkingExceptionOccurs);
            }
            catch (Exception ex)
            {
                Logger.Fatal(ex, Messages.ErrUnhandledExceptionOccurs);
            }

            StartTimer();
        }

        private void StopTimer()
        {
            _timer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        private void StartTimer()
        {
            _timer.Change(_conf.TXUpdateTimerInterval, _conf.TXUpdateTimerInterval);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            StartTimer();

            return Task.FromResult(0);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            StopTimer();

            await Task.Delay(60);
        }
    }
}
