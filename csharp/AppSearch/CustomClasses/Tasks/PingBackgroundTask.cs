using AppSearch.MVC.Helpers;
using AppSearch.MVC.Models;
using System.Collections.ObjectModel;
using System.Net.Http;

namespace AppSearch.CustomClasses.Tasks
{
    public class PingBackgroundTask : BackgroundTask
    {
        private readonly ConfigurationModel _config;
        private readonly ObservableCollection<EnviromentModel> _source;
        private int _activeTasks;
        public int ActiveTasks { get => _activeTasks; }

        public PingBackgroundTask(ObservableCollection<EnviromentModel> source, 
            ConfigurationModel config) : base()
        {
            _source = source;
            _config = config;
        }

        public override void CancelTask()
        {
            base.CancelTask();
        }

        public async Task StartAsync()
        {
            using var localToken = CancellationTokenSource.CreateLinkedTokenSource(_cancellationTokenSource.Token);
            _activeTasks = _source.Count;
            try
            {
                await Parallel.ForEachAsync(_source, localToken.Token, async (item, token) =>
                {
                    await Run(Ping, item);

                    int remainingTasks = Interlocked.Decrement(ref _activeTasks);
                    if (remainingTasks == 0)
                    {
                        OnTaskCompleted(EventArgs.Empty);
                    }
                });
            }
            catch (OperationCanceledException)
            {
                OnTaskCanceled(EventArgs.Empty);
            }
            catch (Exception ex)
            {
                if (_config.EnableLogging == LogginLevel.ERROR)
                    Console.WriteLine(ex);
            }
        }

        private async Task<bool> Ping(EnviromentModel item)
        {
            bool result = false;
            try
            {
                bool? isActive = await PingAsync(item.TargetUri, _cancellationTokenSource.Token);
                item.UpdateActive(isActive);
                if (isActive.HasValue)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLine(ex, _config, LogginLevel.ERROR);
            }
            return result;
        }

        private async Task<bool?> PingAsync(string? url, CancellationToken token)
        {
            if (string.IsNullOrEmpty(url)) return null;

            try
            {
                using var httpClient = new HttpClient();
                httpClient.Timeout = TimeSpan.FromSeconds(_config.Timeout);

                token.ThrowIfCancellationRequested();

                var response = await httpClient.GetAsync(url, token);
                return response.IsSuccessStatusCode;
            }
            catch (OperationCanceledException ex)
            {
                LogHelper.WriteLine(ex, _config, LogginLevel.ERROR);
                return false;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLine(ex, _config, LogginLevel.ERROR);
            }
            return false;
        }
    }
}
