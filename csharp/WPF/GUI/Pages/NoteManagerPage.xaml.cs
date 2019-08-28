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
using WPF.GUI.Controls;
using System.Collections.Generic;

namespace WPF.GUI.Pages
{
    /// <summary>
    /// Interaction logic for NoteManagerPage.xaml
    /// </summary>
    public partial class NoteManagerPage : Page, IDisposableExtended, IPropertizableControl
    {
        private AitAccountModel account;
        private AitFileModel currentNote;
        private BackgroundWorker backgroundWorker;

        private bool? CorrectlyAssign;
        private bool IsCorrectFilled;
        private bool StopClock;
        private bool LottoListIsDefault;
        private FileTypesEnum? type;

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

            AddNewLottoTextBox();
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

            EditContentBtn.Click -= EditContentBtn_Click;
            ClearContentBtn.Click += ClearContentBtn_Click;
            SaveContentBtn.Click += SaveContentBtn_Click;

            NoteNameBox.LostFocus -= MessageContent_LostFocus;
            NoteTypeComboBox.LostFocus -= MessageContent_LostFocus;
            NoteAssignedToBox.LostFocus -= NoteAssignedToBox_LostFocus;
            MessageContent.LostFocus -= MessageContent_LostFocus;

            IsDisposed = true;

            account.Dispose();
            currentNote.Dispose();
            backgroundWorker.Dispose();

            ClearMessageContentListView(true);

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

            EditContentBtn.Click += EditContentBtn_Click;
            ClearContentBtn.Click += ClearContentBtn_Click;
            SaveContentBtn.Click += SaveContentBtn_Click;

            NoteNameBox.LostFocus += MessageContent_LostFocus;
            NoteTypeComboBox.LostFocus += MessageContent_LostFocus;
            NoteAssignedToBox.LostFocus += NoteAssignedToBox_LostFocus;
            MessageContent.LostFocus += MessageContent_LostFocus;
        }

        #region RIGHT PANEL METHODS

        #region RIGHT PANEL METHODS - NEW FILE

        private void EditContentBtn_Click(object sender, RoutedEventArgs e)
        {
            NoteNameBox.IsEnabled = !NoteNameBox.IsEnabled;
            NoteTypeComboBox.IsEnabled = !NoteTypeComboBox.IsEnabled;
            NoteAssignedToBox.IsEnabled = !NoteAssignedToBox.IsEnabled;
            MessageContent.IsEnabled = !MessageContent.IsEnabled;
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
                if (!string.IsNullOrEmpty(NoteAssignedToBox.Text))
                {
                    AitFileModel clone = null;
                    var names = NoteAssignedToBox.Text.Split(',', ';').ToList();
                    if(currentNote != null)
                    {
                        clone = (AitFileModel)currentNote.Clone();
                        currentNote.Delete();
                    }
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
                                var creator = context.Accounts.Where(q => q.ID.Equals(PDBContext.Instance.AccountID)).FirstOrDefault();
                                var newNote = new AitFileModel(context);

                                if(clone != null)
                                {
                                    newNote.FileCreator = clone.FileCreator;
                                    newNote.Create = clone.Create;
                                }

                                newNote.ID = Generators.RecordIDGenerator(TableInerfixEnum.FLS);
                                newNote.FileOwner = a;
                                newNote.Name = NoteNameBox.Text;
                                newNote.Type = (FileTypesEnum)NoteTypeComboBox.SelectedIndex;
                                newNote.Content = SerializableControl();
                                newNote.Insert();
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
                            currentNote.Name = NoteNameBox.Text;
                            currentNote.Type = (FileTypesEnum)NoteTypeComboBox.SelectedIndex;
                            currentNote.Content = SerializableControl();
                            currentNote.Update();
                        }
                        else
                        {
                            var newNote = new AitFileModel(context);
                            newNote.ID = Generators.RecordIDGenerator(TableInerfixEnum.FLS);
                            newNote.Name = NoteNameBox.Text;
                            newNote.Type = (FileTypesEnum)NoteTypeComboBox.SelectedIndex;
                            newNote.Content = SerializableControl();
                            newNote.Insert();
                        }
                    }
                }
                ClearContentAction();
                InitListView();
            }
            catch (Exception ex)
            {
                LogManager.Instance.LogExceptionToFileAndDB(ex);
            }
        }

        public string SerializableControl()
        {
            return CryptoJsonManager.Instance.Serialize(new LogInfoModel
            {
                Type = type != null ? (FileTypesEnum)type : FileTypesEnum.UNDEFINED,
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

            if(type != null)
            {
                if(FileTypesEnum.LOTTO_NOTE.Equals(type))
                {
                    MessageContentList.Visibility = Visibility.Visible;
                }
            }
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
                if(!string.IsNullOrEmpty(NoteAssignedToBox.Text))
                {
                    var exceptionName = new StringBuilder();
                    var names = NoteAssignedToBox.Text.Split(',', ';').ToList();
                    using (var context = PDBContext.Instance.Context)
                    {
                        foreach (var value in names)
                        {
                            var name = value.Replace(" ", "");
                            var accs = context.Accounts.Where(q => q.Login.ToLower().Equals(name.ToLower())
                                                                   || q.UserData != null && !string.IsNullOrEmpty(q.UserData.Nick) && q.UserData.Nick.ToLower().Equals(name.ToLower())
                                                                   || q.UserData != null && !string.IsNullOrEmpty(q.UserData.FullName) && q.UserData.FullName.ToLower().Equals(name.ToLower())).ToList();
                            if (accs == null || !accs.Any())
                            {
                                exceptionName.Append(string.Format(WPF.Properties.Resources.INVALID_ACCOUNT_NAME, name) + Environment.NewLine);
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(exceptionName.ToString()))
                        throw new AitAccountExceptions.InvalidValueException(exceptionName.ToString());
                }

                CorrectlyAssign = true;
            }
            catch (Exception ex)
            {
                LogManager.Instance.LogExceptionToFileAndDB(ex);
            }

            MessageContent_LostFocus(sender, e);
        }

        private void MessageContent_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(NoteNameBox.Text) && NoteTypeComboBox.SelectedIndex != -1 && !string.IsNullOrEmpty(MessageContent.Text))
                IsCorrectFilled = true;
            else
                IsCorrectFilled = false;

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
                || !string.IsNullOrEmpty(MessageContent.Text) && TypeAllowToEmptyContent())
            {
                ClearContentBtn.IsEnabled = true;
            }
            else
            {
                ClearContentBtn.IsEnabled = false;
            }
        }

        private bool TypeAllowToEmptyContent()
        {
            var typeInt = NoteTypeComboBox.SelectedIndex;
            var type = FileTypesManager.SetType(typeInt);
            return FileTypesManager.AllowToEmptyContent(type);
        }

        private void ValidateRequiredFieldFillCorrectly()
        {
            if ((string.IsNullOrEmpty(NoteNameBox.Text) || CorrectlyAssign != false)
                && (IsCorrectFilled || TypeAllowToEmptyContent()))
            {
                SaveContentBtn.IsEnabled = true;
            }
            else
            {
                SaveContentBtn.IsEnabled = false;
            }
        }

        private void LottoTextbox_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (!string.IsNullOrEmpty(textBox?.Text) && textBox.Text.Contains(",") || textBox.Text.Contains(" "))
            {
                var spaceTab = textBox.Text.Split(' ').ToList();
                var dotTab = textBox.Text.Split(',').ToList();
                if(ValidateLottoNewLuckyNumber(dotTab) || ValidateLottoNewLuckyNumber(spaceTab))
                {
                    using(var context = PDBContext.Instance.Context)
                    {
                        var nick = string.Empty;
                        var data = context.UsersDatas.Where(q => !string.IsNullOrEmpty(q.AssignedTo) && q.AssignedTo.Equals(PDBContext.Instance.AccountID) == true).FirstOrDefault();
                        if (data != null)
                        {
                            if (!string.IsNullOrEmpty(data.Nick))
                                nick = data.Nick;
                            else
                                nick = data.FullName;
                        }
                        else
                            nick = context.Accounts.Find(PDBContext.Instance.AccountID)?.Login;

                        var luckyNumberToSave = new AitFileModel(context)
                        {
                            ID = Generators.RecordIDGenerator(TableInerfixEnum.FLS),
                            FileCreator = context.Accounts.Where(q => q.ID.Equals(PDBContext.Instance.AccountID)).FirstOrDefault(),
                            Name = string.Format(WPF.Properties.Resources.USER_LOTTONUMBER, nick),
                            Type = FileTypesEnum.LOTTO_NOTE,
                            Content = ConvertTab2String(dotTab)
                        };

                        luckyNumberToSave.Insert();
                    }

                    AddNewLottoTextBox();
                }
                else
                {
                    throw new AitAccountExceptions.InvalidValueException(string.Format(WPF.Properties.Resources.INVALID_LOTTO_NUMBER, textBox.Text));
                }
            }

            if(string.IsNullOrEmpty(textBox.Text) && MessageContentList.Items.Count != 1)
            {
                MessageContentList.Items.Remove(sender);
            }
        }

        private bool ValidateLottoNewLuckyNumber(List<string> tab)
        {
            if (tab == null || tab.Count != 6)
                return false;

            foreach(var value in tab)
            {
                var input = value.Replace(" ", "");
                var number = -1;
                if (!int.TryParse(input, out number))
                {
                    return false;
                }
            }

            return true;
        }

        public string ConvertTab2String(List<string> tab)
        {
            var result = string.Empty;

            foreach(var value in tab)
            {
                result += ", " + value;
            }

            return result;
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
            DispatcherExtension.Invoke(() =>
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
                    account.FillObject();
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

            EditContentBtn.Visibility = Visibility.Collapsed;
            EditContentBtn.IsEnabled = false;
            ClearContentBtn.IsEnabled = false;
            SaveContentBtn.IsEnabled = false;

            NoteNameBox.IsEnabled = true;
            NoteTypeComboBox.IsEnabled = true;
            NoteAssignedToBox.IsEnabled = true;
            MessageContent.IsEnabled = true;

            currentNote = null;
            MessageContentList.Visibility = Visibility.Collapsed;
            ClearMessageContentListView();
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
                            acc.FillObject();
                            if(!string.IsNullOrEmpty(acc.UserData?.Nick))
                            {
                                names += acc.UserData.Nick;
                            }
                            else if(!string.IsNullOrEmpty(acc.UserData?.FullName))
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
                    var obj = CryptoJsonManager.Instance.Deserialize<LogInfoModel>(item.Note.Content, false);
                    if (obj != null)
                    {
                        MessageContent.Text = obj.Message.Message;
                    }
                }
                catch (Exception)
                {
                    MessageContent.Text = item.Note.Content;
                }

                Date.Text = item.Note.Create.ToString("dd/MM/yyyy HH:mm:ss");
                currentNote = item.Note;

                NoteNameBox.IsEnabled = false;
                NoteTypeComboBox.IsEnabled = false;
                NoteAssignedToBox.IsEnabled = false;
                MessageContent.IsEnabled = false;
            }
        }

        private void AddNewLottoTextBox()
        {
            var lottoTextbox = new TextBox();
            lottoTextbox.LostFocus += LottoTextbox_LostFocus;
            MessageContentList.Items.Add(new ListViewItem { Content = lottoTextbox });

            if (MessageContentList.Items.Count == 1)
                LottoListIsDefault = true;
        }

        private void ClearMessageContentListView(bool fullClear = false)
        {
            foreach (ListViewItem item in MessageContentList.Items)
            {
                var textBox = item?.Content as TextBox;
                if (textBox != null)
                {
                    textBox.LostFocus -= LottoTextbox_LostFocus;
                }
            }
            MessageContentList.Items.Clear();

            if(!fullClear)
                AddNewLottoTextBox();
        }
    }
}
