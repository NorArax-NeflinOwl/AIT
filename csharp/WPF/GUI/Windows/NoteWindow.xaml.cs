using System;
using System.Windows;
using System.Windows.Controls;
using WPF.GUI.Controls.NoteManagerControls;
using WPF.GUI.Windows.Properties;
using WPF.Models.Interfaces;

namespace WPF.GUI.Windows
{
    /// <summary>
    /// Interaction logic for NoteWindow.xaml
    /// </summary>
    public partial class NoteWindow : Window, IDisposableExtended, IPropertizableControl
    {
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

        public void Dispose()
        {
            IsDisposed = true;
            GC.Collect();
        }

        public void Init()
        {
            RightPanel = new RightPanelNoteControl(null);

            if (MainNoteFrame == null)
                MainNoteFrame = new Frame();

            MainNoteFrame.Content = RightPanel;
            MainNoteFrame.DataContext = RightPanel;
            MainNoteFrame.Visibility = Visibility.Visible;
        }

        public void Subscribe()
        {
        }
    }
}
