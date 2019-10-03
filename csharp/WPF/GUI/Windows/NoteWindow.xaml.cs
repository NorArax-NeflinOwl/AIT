using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using WPF.GUI.Controls.NoteManagerControls;
using WPF.GUI.Windows.Properties;
using WPF.Managers.Helpers;
using WPF.Models.Interfaces;

namespace WPF.GUI.Windows
{
    /// <summary>
    /// Interaction logic for NoteWindow.xaml
    /// </summary>
    public partial class NoteWindow : Window, IDisposableExtended, IPropertizableControl
    {
        private readonly string path;
        private BackgroundWorker bw;

        public bool IsDisposed { get; set; }

        public IProperties Properties { get; }
        public RightPanelNoteControl RightPanel { get; set; }

        public NoteWindow()
        {
            Properties = new NoteProperties(this);
            InitializeComponent();
            Init();
            Subscribe();
        }

        public NoteWindow(string path)
        {
            Properties = new NoteProperties(this, path);
            this.path = path;

            InitializeComponent();
            Init();
            Subscribe();
        }

        public void Dispose()
        {
            IsDisposed = true;
            GC.Collect();
        }

        public void Init()
        {
            WindowCentralizer.CenterWindowOnScreen(this);
            Title = (Properties as NoteProperties).Title;

            if (string.IsNullOrEmpty(path))
            {
                RightPanel = new RightPanelNoteControl(null);

                if (MainNoteFrame == null)
                    MainNoteFrame = new Frame();

                MainNoteFrame.Content = RightPanel;
                MainNoteFrame.DataContext = RightPanel;
                MainNoteFrame.Visibility = Visibility.Visible;
            }
            else
            {
                MainNoteFrame.Visibility = Visibility.Collapsed;
                ProgressControl.Visibility = Visibility.Visible;

                bw = new BackgroundWorker();
                bw.DoWork += BackgroundWorker_DoWork;
                bw.RunWorkerAsync();
            }
        }
        private void HandleNavigating(object sender, NavigatingCancelEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.Forward)
            {
                e.Cancel = true;
            }
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            bw.DoWork -= BackgroundWorker_DoWork;

            var result = DirectoryInfoProvider.GetString2PrintDirectorySize(path);
            Dispatcher.Invoke(() => StopInit(result));
        }

        private void StopInit(string result)
        {
            ResultTextControl.Text = result;
            ResultText.Visibility = Visibility.Visible;
            ProgressControl.Visibility = Visibility.Collapsed;
        }

        public void Subscribe()
        {
        }
    }
}
