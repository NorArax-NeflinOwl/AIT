using System;
using System.Windows;
using System.Windows.Controls;
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

                ResultTextControl.Text = DirectoryInfoProvider.GetString2PrintDirectorySize(path);

                ResultText.Visibility = Visibility.Visible;
                ProgressControl.Visibility = Visibility.Collapsed;
            }
        }

        public void Subscribe()
        {
        }
    }
}
