using System;
using System.Windows.Threading;
using WPF.Managers;

namespace WPF.Models.Extensions
{
    public class DispatcherExtension
    {
        public static void Invoke(Action method)
        {
            Dispatcher.CurrentDispatcher.Invoke(() =>
            {
                try
                {
                    method.Invoke();
                }
                catch(Exception e)
                {
                    LogManager.Instance.LogExceptionToFile(e);
                }
            });
        }

        public static void InvokeAsync(Action action, DispatcherPriority priority)
        {
            Dispatcher.CurrentDispatcher.InvokeAsync(() =>
            {
                try
                {
                    action.Invoke();
                }
                catch(Exception e)
                {
                    LogManager.Instance.LogExceptionToFile(e);
                }
            }, priority);
        }
    }
}
