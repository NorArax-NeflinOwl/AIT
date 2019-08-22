using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using WPF.Models.Extensions;
using WPF.Models.Interfaces;
using WPF.UI.Windows.Properties;

namespace WPF.UI.Windows
{
    /// <summary>
    /// Interaction logic for PopupWindow.xaml
    /// </summary>
    public partial class PopupWindow : Window, IDisposableExtended
    {
        public IWindowsProperties Properties { get; }
        public int VisibilityTimeInSecond { get; set; }
        public bool IsDisposed { get; set; }

        public PopupWindow()
        {
            Properties = new PopupProperties(this);
            InitializeComponent();

            ProcessToastTitle.Text = WPF.Properties.Resources.PROCESS;
            ProcessToastGrid.Visibility = Visibility.Visible;
            ToastWindowBorder.Visibility = Visibility.Collapsed;
            CenterWindowOnScreen();
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
            DispatcherExtension.Invoke(() =>
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
            });

            DispatcherExtension.Invoke(async () =>
            {
                await Task.Delay(VisibilityTimeInSecond * 1500);
                Close();
            });
        }
        private void CenterWindowOnScreen()
        {
            double screenWidth = SystemParameters.PrimaryScreenWidth;
            double screenHeight = SystemParameters.PrimaryScreenHeight;
            double windowWidth = this.Width;
            double windowHeight = this.Height;
            Left = (screenWidth / 2) - (windowWidth / 2);
            Top = (screenHeight / 2) - (windowHeight / 2);
        }

        public void Dispose()
        {
            Properties.Dispose();
            IsDisposed = true;
            GC.Collect();
        }
    }
}
