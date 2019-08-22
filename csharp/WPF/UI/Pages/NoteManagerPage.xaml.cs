using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WPF.Databases.Contexts;
using WPF.Databases.Models;
using WPF.Managers;
using WPF.Managers.Helpers;
using WPF.Models;
using WPF.Models.Enums;
using WPF.Models.Extensions;
using WPF.Models.Extensions.Exceptions;
using WPF.Models.Interfaces;
using WPF.UI.Controls;

namespace WPF.UI.Pages
{
    /// <summary>
    /// Interaction logic for NoteManagerPage.xaml
    /// </summary>
    public partial class NoteManagerPage : Page, IDisposableExtended, IPropertizableControl
    {
        private AitAccountModel account;
        private bool? CorrectlyAssign;
        private bool IsCorrectFilled;
        private bool StopClock;
        private BackgroundWorker backgroundWorker;
        private FileTypesEnum? type;
        private AitFilesModel currentNote;

        public NoteManagerPage()
        {
            InitializeComponent();
            Init();
            Subscribe();
        }

        public bool IsDisposed { get; set; }

        public IProperties Properties { get; set; }

        public void Init()
        {
            InitListView();
            InitNoteTypeComboBox();

            NoteManagerTitle.Text = WPF.Properties.Resources.NOTEMANAGER_HEADER;
            DeleteSelectedItems.Content = WPF.Properties.Resources.DELETE_BTNCONTENT;
            DetachedSelectedItems.Content = WPF.Properties.Resources.DETACHED_BTNCONTENT;

            NoteContentTitle.Text = WPF.Properties.Resources.NOTE_CONTENT;
            EditContentBtn.Content = WPF.Properties.Resources.EDIT_HEADER;
            ClearContentBtn.Content = WPF.Properties.Resources.CLEAR;
            SaveContentBtn.Content = WPF.Properties.Resources.SAVE;
            // TODO Set note fields hits from WPF.Properties.Resources

            MessageTitle.Text = WPF.Properties.Resources.MESSAGE;
            DateTitle.Text = WPF.Properties.Resources.DATE_S;
            MessageContent.MaxLines = int.MaxValue;

            if (account != null && account.Permition.Equals(PermitionAccountEnum.ADMIN))
            {
                DeleteSelectedItems.Visibility = Visibility.Visible;
            }

            backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += StartTimeTicker;
            backgroundWorker.RunWorkerAsync();
        }

        public void Dispose()
        {
            NoteManagerListView.MouseDoubleClick -= NoteManagerListView_MouseDoubleClick;
            NoteManagerListView.SelectionChanged -= NoteManagerListView_SelectionChanged;

            SelectAllCheckBox.Click -= SelectAllCheckBox_Click;
            DeleteSelectedItems.Click -= DeleteSelectedItems_Click;
            DetachedSelectedItems.Click -= DetachedSelectedItems_Click;

            NoteTypeComboBox.SelectionChanged -= NoteTypeComboBox_SelectionChanged;
            NoteNameBox.TextChanged -= NoteTitleBox_TextChanged;
            NoteAssignedToBox.TextChanged -= NoteAssignedToBox_LostFocus;

            EditContentBtn.Click -= EditContentBtn_Click;
            ClearContentBtn.Click += ClearContentBtn_Click;
            SaveContentBtn.Click += SaveContentBtn_Click;

            MessageContent.LostFocus -= MessageContent_LostFocus;

            IsDisposed = true;
            GC.Collect();
        }

        public void Subscribe()
        {
            NoteManagerListView.MouseDoubleClick += NoteManagerListView_MouseDoubleClick;
            NoteManagerListView.SelectionChanged += NoteManagerListView_SelectionChanged;

            SelectAllCheckBox.Click += SelectAllCheckBox_Click;
            DeleteSelectedItems.Click += DeleteSelectedItems_Click;
            DetachedSelectedItems.Click += DetachedSelectedItems_Click;

            NoteTypeComboBox.SelectionChanged += NoteTypeComboBox_SelectionChanged;
            NoteNameBox.TextChanged += NoteTitleBox_TextChanged;
            NoteAssignedToBox.LostFocus += NoteAssignedToBox_LostFocus;

            EditContentBtn.Click += EditContentBtn_Click;
            ClearContentBtn.Click += ClearContentBtn_Click;
            SaveContentBtn.Click += SaveContentBtn_Click;

            MessageContent.LostFocus += MessageContent_LostFocus;
        }

        #region RIGHT PANEL METHODS

        #region RIGHT PANEL METHODS - NEW FILE

        private void EditContentBtn_Click(object sender, RoutedEventArgs e)
        {
            NoteNameBox.IsEnabled = true;
            NoteTypeComboBox.IsEnabled = true;
            NoteAssignedToBox.IsEnabled = true;
            MessageContent.IsEnabled = true;
        }

        private void MessageContent_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(MessageContent.Text))
                IsCorrectFilled = true;
            else
                IsCorrectFilled = false;

            ValidateNotDefaultNote();
            ValidateRequiredFieldFillCorrectly();
        }

        private void StartTimeTicker(object sender, DoWorkEventArgs e)
        {
            Dispatcher.Invoke(async () =>
            {
                while (!StopClock)
                {
                    Date.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                    await Task.Delay(1000);
                }
            });
        }

        private void SaveContentBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DispatcherExtension.Invoke(() =>
                {
                    if (!string.IsNullOrEmpty(NoteAssignedToBox.Text))
                    {
                        var names = NoteAssignedToBox.Text.Split(',', ';').ToList();
                        foreach (var value in names)
                        {
                            var name = value.Replace(" ", "");
                            using (var context = PDBContext.Instance.Context)
                            {
                                var acc = context.Accounts.Where(q => q.Login.ToLower().Equals(name.ToLower())
                                                                       || q.UserData != null && !string.IsNullOrEmpty(q.UserData.Nick) && q.UserData.Nick.ToLower().Equals(name.ToLower())
                                                                       || q.UserData != null && !string.IsNullOrEmpty(q.UserData.FullName) && q.UserData.FullName.ToLower().Equals(name.ToLower())).ToList();

                                foreach (var a in acc)
                                {
                                    if (currentNote != null)
                                    {
                                        currentNote.Creator = PDBContext.Instance.AccountID;
                                        currentNote.AssignedTo = a.ID;
                                        currentNote.Name = NoteNameBox.Text;
                                        currentNote.Type = (FileTypesEnum)NoteTypeComboBox.SelectedIndex;
                                        currentNote.Content = SerializableControl();
                                        currentNote.Update();
                                    }
                                    else
                                    {
                                        var newNote = new AitFilesModel(context);
                                        newNote.ID = Generators.RecordIDGenerator(TableInerfixEnum.FLS);
                                        newNote.Creator = PDBContext.Instance.AccountID;
                                        newNote.AssignedTo = a.ID;
                                        newNote.Name = NoteNameBox.Text;
                                        newNote.Type = (FileTypesEnum)NoteTypeComboBox.SelectedIndex;
                                        newNote.Content = SerializableControl();
                                        newNote.Insert();
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        using (var context = PDBContext.Instance.Context)
                        {
                            if (currentNote != null)
                            {
                                currentNote.Creator = PDBContext.Instance.AccountID;
                                currentNote.Name = NoteNameBox.Text;
                                currentNote.Type = (FileTypesEnum)NoteTypeComboBox.SelectedIndex;
                                currentNote.Content = SerializableControl();
                                currentNote.Update();
                            }
                            else
                            {
                                var newNote = new AitFilesModel(context);
                                newNote.ID = Generators.RecordIDGenerator(TableInerfixEnum.FLS);
                                newNote.Creator = PDBContext.Instance.AccountID;
                                newNote.Name = NoteNameBox.Text;
                                newNote.Type = (FileTypesEnum)NoteTypeComboBox.SelectedIndex;
                                newNote.Content = SerializableControl();
                                newNote.Insert();
                            }
                        }
                    }
                    ClearContentAction();
                    InitListView();
                });
            }
            catch (Exception ex)
            {
                LogManager.Instance.LogExceptionToFile(ex);
            }
        }

        public string SerializableControl()
        {
            return CryptoJsonManager.Instance.Serialize(new LogInfoModel
            {
                Type = type != null ? (FileTypesEnum)type : FileTypesEnum.NOTE,
                Message = new SimpleMessageInfoModel(MessageContent.Text)
            });
        }

        private void ClearContentBtn_Click(object sender, RoutedEventArgs e)
        {
            ClearContentAction();
        }

        private void NoteTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ValidateNotDefaultNote();
            ValidateRequiredFieldFillCorrectly();

            if (NoteTypeComboBox.SelectedIndex >= 0)
                NoteAssignedToBox.IsEnabled = true;
            else
                NoteAssignedToBox.IsEnabled = false;

            type = FileTypesManager.SetType(NoteTypeComboBox.SelectedIndex);
        }

        private void NoteTitleBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ValidateNotDefaultNote();
            ValidateRequiredFieldFillCorrectly();
        }

        private void NoteAssignedToBox_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                CorrectlyAssign = false;
                var exceptionName = new StringBuilder();
                var names = NoteAssignedToBox.Text.Split(',', ';').ToList();
                using (var context = PDBContext.Instance.Context)
                {
                    foreach (var name in names)
                    {
                        // FIX ME
                        var accs = context.Accounts.Where(q => q.Login.ToLower().Equals(name.ToLower())
                                                               || q.UserData != null && !string.IsNullOrEmpty(q.UserData.Nick) && q.UserData.Nick.ToLower().Equals(name.ToLower())
                                                               || q.UserData != null && !string.IsNullOrEmpty(q.UserData.FullName) &&  q.UserData.FullName.ToLower().Equals(name.ToLower())).ToList();
                        if (accs == null || !accs.Any())
                        {
                            exceptionName.Append(string.Format(WPF.Properties.Resources.INVALID_ACCOUNT_NAME, name) + Environment.NewLine);
                        }
                    }
                }

                if (!string.IsNullOrEmpty(exceptionName.ToString()))
                    throw new AitAccountExceptions.InvalidNameException(exceptionName.ToString());

                CorrectlyAssign = true;
            }
            catch (Exception ex)
            {
                LogManager.Instance.LogExceptionToFile(ex);
            }

            ValidateNotDefaultNote();
            ValidateRequiredFieldFillCorrectly();
        }

        private void InitNoteTypeComboBox()
        {
            foreach (var item in FileTypesManager.Content)
            {
                NoteTypeComboBox.Items.Add(item);
            }
        }

        private void ValidateNotDefaultNote()
        {
            if (NoteTypeComboBox.SelectedIndex >= 0
                || !string.IsNullOrEmpty(NoteNameBox.Text)
                || !string.IsNullOrEmpty(NoteAssignedToBox.Text)
                || !string.IsNullOrEmpty(MessageContent.Text))
            {
                ClearContentBtn.IsEnabled = true;
            }
            else
            {
                ClearContentBtn.IsEnabled = false;
            }
        }

        public void ValidateRequiredFieldFillCorrectly()
        {
            if (NoteTypeComboBox.SelectedIndex >= 0
                && (string.IsNullOrEmpty(NoteNameBox.Text) || CorrectlyAssign != false)
                && IsCorrectFilled)
            {
                SaveContentBtn.IsEnabled = true;
            }
            else
            {
                SaveContentBtn.IsEnabled = false;
            }
        }

        #endregion

        #region RIGHT PANEL METHODS - MULTI MODE

        private void SetMulitModePanel()
        {

        }

        #endregion

        #endregion

        #region LEFT PANEL METHODS

        private void DetachedSelectedItems_Click(object sender, RoutedEventArgs e)
        {
            DispatcherExtension.Invoke(() =>
            {
                foreach (NoteListViewItemControl item in NoteManagerListView.SelectedItems)
                {
                    item.Note.AssignedTo = string.Empty;
                    item.Note.Creator = string.Empty;
                    item.Note.Update();
                }

                NoteManagerListView.SelectedItems.Clear();
                DetachedSelectedItems.IsEnabled = false;
                DeleteSelectedItems.IsEnabled = false;

                NoteManagerListView.Items.Clear();
                ClearContentAction();
                InitListView();
            });
        }

        private void DeleteSelectedItems_Click(object sender, RoutedEventArgs e)
        {
            DispatcherExtension.Invoke(async () =>
            {
                foreach (NoteListViewItemControl item in NoteManagerListView.SelectedItems)
                {
                    item.Note.Delete();
                }

                NoteManagerListView.SelectedItems.Clear();
                DetachedSelectedItems.IsEnabled = false;
                DeleteSelectedItems.IsEnabled = false;

                NoteManagerListView.Items.Clear();
                ClearContentAction();
                InitListView();
            });
        }

        private void SelectAllCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (SelectAllCheckBox.IsChecked == true)
            {
                CreateNewNoteGrid.Visibility = Visibility.Collapsed;
                OpenMultiNoteGrid.Visibility = Visibility.Visible;

                foreach (var item in NoteManagerListView.Items)
                    NoteManagerListView.SelectedItems.Add(item);
            }
            if (SelectAllCheckBox.IsChecked == false)
            {
                NoteManagerListView.SelectedItems.Clear();
                ClearContentAction();

                CreateNewNoteGrid.Visibility = Visibility.Visible;
                OpenMultiNoteGrid.Visibility = Visibility.Collapsed;
            }

            if (ChangeButtonsEditabilityAndCheckMultiMode())
            {
                SetMulitModePanel();
            }
        }

        private void NoteManagerListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ChangeButtonsEditabilityAndCheckMultiMode())
            {
                SetMulitModePanel();
            }
        }

        private void NoteManagerListView_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            NoteManagerListView.SelectedItem = null;
            if(ChangeButtonsEditabilityAndCheckMultiMode())
            {
                SetMulitModePanel();
            }
        }

        private bool ChangeButtonsEditabilityAndCheckMultiMode()
        {
            if (NoteManagerListView.SelectedItems.Any())
            {
                DeleteSelectedItems.IsEnabled = true;

                var IsAnyDetached = false;
                foreach (NoteListViewItemControl item in NoteManagerListView.SelectedItems)
                {
                    if (item.Note.IsDetached)
                        IsAnyDetached = true;
                }

                if (!IsAnyDetached)
                    DetachedSelectedItems.IsEnabled = true;
                else
                    DetachedSelectedItems.IsEnabled = false;
            }
            else
            {
                DetachedSelectedItems.IsEnabled = false;
                DeleteSelectedItems.IsEnabled = false;
            }

            EditContentBtn.Visibility = Visibility.Collapsed;
            if (!NoteManagerListView.SelectedItems.Any())
            {
                CreateNewNoteGrid.Visibility = Visibility.Visible;
                OpenMultiNoteGrid.Visibility = Visibility.Collapsed;
                ClearContentAction();
            }
            else if (NoteManagerListView.SelectedItems.Count == 1)
            {
                CreateNewNoteGrid.Visibility = Visibility.Visible;
                OpenMultiNoteGrid.Visibility = Visibility.Collapsed;
                SetOneNoteContentAction();
                EditContentBtn.Visibility = Visibility.Visible;
            }
            else
            {
                CreateNewNoteGrid.Visibility = Visibility.Collapsed;
                OpenMultiNoteGrid.Visibility = Visibility.Visible;

                return true;
            }
            return false;
        }

        private void InitListView()
        {
            NoteManagerListView.Items.Clear();
            using (var context = PDBContext.Instance.Context)
            {
                account = context.Accounts.Where(q => q.ID.Equals(PDBContext.Instance.AccountID)).FirstOrDefault();
                if (account != null)
                {
                    var index = 1;
                    var list = account.Files.ToList();
                    foreach (var note in list)
                    {
                        NoteManagerListView.Items.Add(new NoteListViewItemControl(index, note));
                        index++;
                    }
                }
            }
        }

        #endregion

        private void ClearContentAction()
        {
            NoteNameBox.Text = string.Empty;
            NoteAssignedToBox.Text = string.Empty;
            NoteTypeComboBox.SelectedIndex = -1;
            type = FileTypesManager.SetType(NoteTypeComboBox.SelectedIndex);
            MessageContent.Text = string.Empty;
            StopClock = false;

            backgroundWorker.DoWork -= StartTimeTicker;
            backgroundWorker.Dispose();

            backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += StartTimeTicker;
            backgroundWorker.RunWorkerAsync();

            EditContentBtn.IsEnabled = false;
            ClearContentBtn.IsEnabled = false;
            SaveContentBtn.IsEnabled = false;

            NoteNameBox.IsEnabled = true;
            NoteTypeComboBox.IsEnabled = true;
            NoteAssignedToBox.IsEnabled = true;
            MessageContent.IsEnabled = true;

            currentNote = null;
        }

        private void SetOneNoteContentAction()
        {
            var item = NoteManagerListView.SelectedItem as NoteListViewItemControl;
            if(item != null)
            {
                NoteNameBox.Text = item.Note.Name;
                NoteTypeComboBox.SelectedIndex = (int)item.Note.Type;
                var assignId = item.Note.AssignedTo;
                using(var context = PDBContext.Instance.Context)
                {
                    var accs = context.Accounts.Where(q => q.ID.Equals(assignId)).ToList();
                    if(accs != null)
                    {
                        var index = 0;
                        var names = string.Empty;
                        foreach(var acc in accs)
                        {
                            if(!string.IsNullOrEmpty(acc.UserData.Nick))
                            {
                                names += acc.UserData.Nick;
                            }
                            else if(!string.IsNullOrEmpty(acc.UserData.FullName))
                            {
                                names += acc.UserData.FullName;
                            }
                            else
                            {
                                names += acc.Login;
                            }

                            if(accs.Count - 1 != index)
                                names += ", ";

                            index++;
                        }
                        NoteAssignedToBox.Text = names;
                    }
                }

                StopClock = true;
                type = item.Note.Type;
                EditContentBtn.IsEnabled = true;

                try
                {
                    var obj = CryptoJsonManager.Instance.Deserialize<LogInfoModel>(item.Note.Content, null, false);
                    if (obj != null)
                    {
                        Date.Text = obj.Date.ToString();
                        MessageContent.Text = CryptoJsonManager.Instance.Deserialize<SimpleMessageInfoModel>(obj.Message.ToString(), null, false)?.Message;
                    }
                }
                catch (Exception)
                {
                    Date.Text = WPF.Properties.Resources.UNKNOWN;
                    MessageContent.Text = item.Note.Content;
                }
                currentNote = item.Note;

                NoteNameBox.IsEnabled = false;
                NoteTypeComboBox.IsEnabled = false;
                NoteAssignedToBox.IsEnabled = false;
                MessageContent.IsEnabled = false;
            }
        }
    }
}
