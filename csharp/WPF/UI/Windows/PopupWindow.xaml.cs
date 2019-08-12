using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using WPF.Models.Interfaces;
using WPF.UI.Windows.Properties;

namespace WPF.UI.Windows
{
    /// <summary>
    /// Interaction logic for PopupWindow.xaml
    /// </summary>
    public partial class PopupWindow : Window, IDisposable
    {
        public IWindowsProperties Properties { get; }
        public int VisibilityTimeInSecond { get; set; }

        public PopupWindow()
        {
            Properties = new PopupProperties(this);
            InitializeComponent();
            ToastTitleTextBox.Text = "Popup";
            ToastMessageTextBox.Text = "Test";
            VisibilityTimeInSecond = 5;
            Init();
        }

        public PopupWindow(string title, string content, int visibilityTimeInSecond)
        {
            Properties = new PopupProperties(this);
            InitializeComponent();
            ToastTitleTextBox.Text = title;
            ToastMessageTextBox.Text = content;
            VisibilityTimeInSecond = visibilityTimeInSecond;
            Init();
        }

        private void Init()
        {
            Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new Action(() =>
            {
                try
                {
                    var workingArea = SystemParameters.WorkArea;
                    var transform = PresentationSource.FromVisual(this).CompositionTarget.TransformFromDevice;
                    var corner = transform.Transform(new Point(workingArea.Right, workingArea.Bottom));

                    this.Left = corner.X - this.ActualWidth - 10;
                    this.Top = corner.Y - this.ActualHeight;
                }
                catch (Exception) { }
            }));

            Dispatcher.Invoke(async () =>
            {
                await Task.Delay(VisibilityTimeInSecond * 1500);
                Close();
            });
        }

        public void Dispose()
        {
            Properties.Dispose();
            GC.Collect();
        }
    }
}
