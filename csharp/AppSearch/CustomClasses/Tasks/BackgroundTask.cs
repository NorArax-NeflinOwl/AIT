namespace AppSearch.CustomClasses.Tasks
{
    public class BackgroundTask
    {
        protected CancellationTokenSource _cancellationTokenSource;

        public delegate void TaskCompletedEventHandler(object sender, object arg);
        public event TaskCompletedEventHandler? TaskCompleted;

        public delegate void TaskCanceledEventHandler(object sender, object arg);
        public event TaskCanceledEventHandler? TaskCanceled;

        public Exception? Exception { get; private set; }

        public BackgroundTask()
        {
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public async Task Run<T>(Func<T, Task<bool>> function, T item)
        {
            await Task.Run(() =>
            {
                try
                {
                    if(item != null)
                    {
                        bool value = function(item).Result;

                        if (!value || _cancellationTokenSource.Token.IsCancellationRequested)
                        {
                            OnTaskCanceled(item);
                        }
                        else
                        {
                            OnTaskCompleted(item);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Exception = ex;
                    OnTaskCanceled(EventArgs.Empty);
                }
            }, _cancellationTokenSource.Token);
        }

        public virtual void CancelTask()
        {
            _cancellationTokenSource.Cancel();
        }

        protected virtual async void OnTaskCompleted(object arg)
        {
            await Task.Delay(500);
            TaskCompleted?.Invoke(this, arg);
        }

        protected void OnTaskCanceled(object arg)
        {
            TaskCanceled?.Invoke(this, arg);
        }
    }
}
