using System;
using System.Data.Common;
using System.Threading.Tasks;

namespace BTCGatewayAPI.Services
{
    public abstract class BaseService : IDisposable
    {
        private bool _disposed;

        protected DbConnection DbCon { get; }

        protected BaseService(DbConnection dbConnection)
        {
            DbCon = dbConnection;
        }

        ~BaseService()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                DbCon.Dispose();
                _disposed = true;
            }
        }

        protected async Task<bool> TryToPerformAsync(Func<Task> action, Action<Exception> onError, int triesCount)
        {
            var tryNumber = 0;
            var isSuccess = false;

            while (tryNumber < triesCount - 1)
            {
                try
                {
                    await action();
                    isSuccess = true;
                    break;
                }
                catch (Exception ex)
                {
                    onError?.Invoke(ex);
                    tryNumber++;
                }
            }

            return isSuccess;
        }

        protected async Task<(bool, TResult)> TryToPerformAsync<TResult>(Func<Task<TResult>> action, Action<Exception> onError, int triesCount)
        {
            var tryNumber = 0;
            var isSuccess = false;
            var result = default(TResult);

            while (tryNumber < triesCount - 1)
            {
                try
                {
                    result = await action();
                    isSuccess = true;
                    break;
                }
                catch (Exception ex)
                {
                    onError?.Invoke(ex);
                    tryNumber++;
                }
            }

            return (isSuccess, result);
        }
    }
}