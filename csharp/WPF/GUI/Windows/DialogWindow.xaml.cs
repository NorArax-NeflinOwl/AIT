using System;
using System.Text;
using System.Windows;
using WPF.Databases.Contexts;
using WPF.Databases.Models;
using WPF.GUI.Controls;
using WPF.GUI.Pages;
using WPF.GUI.Windows.Properties;
using WPF.Managers.Helpers;
using WPF.Models.Enums;
using WPF.Models.Interfaces;

namespace WPF.GUI.Windows
{
    /// <summary>
    /// Interaction logic for DialogWindow.xaml
    /// </summary>
    public partial class DialogWindow : Window, IDisposableExtended, IPropertizableControl
    {
        public IProperties Properties { get; set; }
        public bool IsDisposed { get; set; }

        public DialogWindow()
        {
            Properties = new DialogProperties(this);
            InitializeComponent();
            Init();
            Subscribe();
        }

        public DialogWindow(IWindowsProperties properties)
        {
            Properties = properties;
            InitializeComponent();
            Init();
            Subscribe();
        }

        public void Dispose()
        {
            KeyUp -= App.MainWindow_KeyUp;
            DialogCancelBtn.Click -= DialogCancelBtn_Click;
            DialogSaveBtn.Click -= DialogSaveBtn_Click;

            IsDisposed = true;
            GC.Collect();
        }

        public void Init()
        {
            WindowCentralizer.CenterWindowOnScreen(this);
            DialogCancelBtn.Content = WPF.Properties.Resources.CANCEL;
            DialogSaveBtn.Content = WPF.Properties.Resources.SAVE;

            if (Properties is DialogProperties dialogProp)
            {
                if (dialogProp.Properties.ContainsKey(nameof(DialogTypeEnum)))
                {
                    var dialogType = (DialogTypeEnum)dialogProp.Properties[nameof(DialogTypeEnum)];

                    if (DialogTypeEnum.NOTE_DELETE_FOR_REASON.Equals(dialogType) || DialogTypeEnum.NOTE_DETACHED_FOR_REASON.Equals(dialogType))
                    {
                        var page = dialogProp.Properties[nameof(NoteManagerPage)] as NoteManagerPage;

                        if (page != null)
                        {
                            var names = new StringBuilder();
                            foreach (NoteListViewItemControl item in page.NoteManagerListView.SelectedItems)
                            {
                                names.Append(item.Note.Name).Append(", ");
                            }

                            if (DialogTypeEnum.NOTE_DELETE_FOR_REASON.Equals(dialogType))
                                DialogTitle.Text = string.Format(WPF.Properties.Resources.DIALOG_DELETE_TITLE, names.ToString().Substring(0, names.Length - 2), Environment.NewLine);
                            else
                                DialogTitle.Text = string.Format(WPF.Properties.Resources.DIALOG_DETACHED_TITLE, names.ToString().Substring(0, names.Length - 2), Environment.NewLine);

                            Title = DialogTitle.Text;
                        }
                    }
                }
            }
        }

        public void Subscribe()
        {
            KeyUp += App.MainWindow_KeyUp;
            DialogCancelBtn.Click += DialogCancelBtn_Click;
            DialogSaveBtn.Click += DialogSaveBtn_Click;
        }

        private void DialogSaveBtn_Click(object sender, RoutedEventArgs e)
        {
            var content = DialogUsedContent.Text;
            if (string.IsNullOrEmpty(content))
            {
                MessageBox.Show(WPF.Properties.Resources.CONTENT_EMPTY, WPF.Properties.Resources.ERROR, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (MessageBox.Show(WPF.Properties.Resources.SAVE_QUESTION, WPF.Properties.Resources.QUESTION, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                if (Properties is DialogProperties dialogProp)
                {
                    if (dialogProp.Properties.ContainsKey(nameof(DialogTypeEnum)))
                    {
                        if (DialogTypeEnum.NOTE_DELETE_FOR_REASON.Equals((DialogTypeEnum)dialogProp.Properties[nameof(DialogTypeEnum)]))
                        {
                            var page = dialogProp.Properties[nameof(NoteManagerPage)] as NoteManagerPage;
                            if (page != null)
                            {
                                var count = 0;
                                foreach (NoteListViewItemControl item in page.NoteManagerListView.SelectedItems)
                                {
                                    item.Note.DeleteForReason(content);
                                    count++;
                                }

                                page.NoteManagerListView.SelectedItems.Clear();
                                page.DetachedSelectedItems.IsEnabled = false;
                                page.DeleteSelectedItems.IsEnabled = false;

                                page.NoteManagerListView.Items.Clear();
                                page.ClearContentAction();
                                page.InitListView();

                                MessageBox.Show(string.Format(WPF.Properties.Resources.DELETE_SUCC, count), WPF.Properties.Resources.INFORMATION, MessageBoxButton.OK, MessageBoxImage.Information);
                                MainContext.Instance.Windows.Close(WindowsNameEnum.DIALOG);
                            }
                        }
                        else if (DialogTypeEnum.NOTE_DETACHED_FOR_REASON.Equals((DialogTypeEnum)dialogProp.Properties[nameof(DialogTypeEnum)]))
                        {
                            var page = dialogProp.Properties[nameof(NoteManagerPage)] as NoteManagerPage;
                            if (page != null)
                            {
                                var count = 0;
                                foreach (NoteListViewItemControl item in page.NoteManagerListView.SelectedItems)
                                {
                                    item.DetachNote();
                                    item.Note.Update();
                                    count++;
                                }

                                page.NoteManagerListView.SelectedItems.Clear();
                                page.DetachedSelectedItems.IsEnabled = false;
                                page.DeleteSelectedItems.IsEnabled = false;

                                page.NoteManagerListView.Items.Clear();
                                page.ClearContentAction();
                                page.InitListView();

                                MessageBox.Show(string.Format(WPF.Properties.Resources.DETACHED_SUCC, count), WPF.Properties.Resources.INFORMATION, MessageBoxButton.OK, MessageBoxImage.Information);
                                MainContext.Instance.Windows.Close(WindowsNameEnum.DIALOG);
                            }
                        }
                    }
                }
            }
        }

        private void DialogCancelBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show(WPF.Properties.Resources.CANCEL_QUESTION, WPF.Properties.Resources.QUESTION, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                MainContext.Instance.Windows.Close(WindowsNameEnum.DIALOG);
            }
        }
    }
}
