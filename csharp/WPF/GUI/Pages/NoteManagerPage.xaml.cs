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
using System.Windows.Media;

namespace WPF.GUI.Pages
{
    /// <summary>
    /// Interaction logic for NoteManagerPage.xaml
    /// </summary>
    public partial class NoteManagerPage : Page, IDisposableExtended, IPropertizableControl
    {
        #region CONSTRUCT

        private AitAccountModel account;
        private AitFileModel currentNote;
        private BackgroundWorker backgroundWorker;

        private List<string> luckyNumbersToSave;

        private bool? CorrectlyAssign;
        private bool IsCorrectFilled;
        private bool StopClock;
        private FileTypeModel type;

        public bool IsDisposed { get; set; }

        public IProperties Properties { get; set; }

        public NoteManagerPage()
        {
            InitializeComponent();
            Init();
            Subscribe();
        }

        public void Init()
        {
            InitListView();
            InitNoteTypeComboBox();

            luckyNumbersToSave = new List<string>();

            NoteManagerTitle.Text = WPF.Properties.Resources.NOTEMANAGER_HEADER;
            DeleteSelectedItems.Content = WPF.Properties.Resources.DELETE_BTNCONTENT;
            DetachedSelectedItems.Content = WPF.Properties.Resources.DETACHED_BTNCONTENT;
            NoteManagerListViewEmpty.Text = WPF.Properties.Resources.LIST_EMPTY;

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

            CreateFilterPanel();

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

            MessageContentList.SelectionChanged += MessageContentList_SelectionChanged;

            IsDisposed = true;

            account.Dispose();
            currentNote?.Dispose();
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

            MessageContentList.SelectionChanged += MessageContentList_SelectionChanged;
        }

        #endregion

        #region RIGHT PANEL METHODS

        #region RIGHT PANEL METHODS - NEW FILE

        private void EditContentBtn_Click(object sender, RoutedEventArgs e)
        {
            NoteNameBox.IsEnabled = !NoteNameBox.IsEnabled;
            NoteTypeComboBox.IsEnabled = !NoteTypeComboBox.IsEnabled;
            NoteAssignedToBox.IsEnabled = !NoteAssignedToBox.IsEnabled;
            MessageContent.IsEnabled = !MessageContent.IsEnabled;

            foreach(ListViewItem item in MessageContentList.Items)
            {
                var textBox = item?.Content as TextBox;
                if (textBox != null)
                    textBox.IsEnabled = !textBox.IsEnabled;
            }
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
                        var name = value.Replace(" ", string.Empty);
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
                                newNote.Type = (NoteTypeComboBox.SelectedItem as FileTypeModel)?.EnumType ?? FileTypesEnum.UNDEFINED;
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
                            currentNote.Type = (NoteTypeComboBox.SelectedItem as FileTypeModel)?.EnumType ?? FileTypesEnum.UNDEFINED;
                            currentNote.Content = SerializableControl();
                            currentNote.Update();
                        }
                        else
                        {
                            var newNote = new AitFileModel(context);
                            newNote.ID = Generators.RecordIDGenerator(TableInerfixEnum.FLS);
                            newNote.Name = NoteNameBox.Text;
                            newNote.Type = (NoteTypeComboBox.SelectedItem as FileTypeModel)?.EnumType ?? FileTypesEnum.UNDEFINED;
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
                InitListView();
            }
        }

        private string SerializableControl()
        {
            return CryptoJsonManager.Instance.Serialize(new LogInfoModel
            {
                Type = type != null ? type.EnumType : FileTypesEnum.UNDEFINED,
                MessageInfo = new MessageInfoModel(luckyNumbersToSave)
                {
                    Message = MessageContent.Text
                },
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

            type = NoteTypeComboBox.SelectedItem as FileTypeModel;

            if(type != null)
            {
                if(FileTypesEnum.LOTTO_NOTE.Equals(type.EnumType))
                {
                    MessageContentList.Visibility = Visibility.Visible;
                }
                else
                {
                    MessageContentList.Visibility = Visibility.Collapsed;
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
                            var name = value.Replace(" ", string.Empty);
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
            foreach (var item in FileTypesManager.Types)
            {
                if((int)account.Permition >= (int)item.PermitionLevel)
                {
                    NoteTypeComboBox.Items.Add(item);
                }
            }
        }

        private void ValidateNotDefaultNote()
        {
            if (NoteTypeComboBox.SelectedIndex >= 0
                || !string.IsNullOrEmpty(NoteNameBox.Text)
                || !string.IsNullOrEmpty(NoteAssignedToBox.Text)
                || !string.IsNullOrEmpty(MessageContent.Text) && TypeAllowToEmptyContent()
                || (luckyNumbersToSave.Any() && type != null && FileTypesEnum.LOTTO_NOTE.Equals(type.EnumType)))
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
            return NoteTypeComboBox.SelectedItem is FileTypeModel model && model.AllowToEmptyContent;
        }

        private void ValidateRequiredFieldFillCorrectly()
        {
            if ((string.IsNullOrEmpty(NoteNameBox.Text) || CorrectlyAssign != false)
                && (IsCorrectFilled || TypeAllowToEmptyContent()) 
                && (!luckyNumbersToSave.Any() || (luckyNumbersToSave.Any() && type != null && FileTypesEnum.LOTTO_NOTE.Equals(type.EnumType))))
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
            try
            {
                var textBox = sender as TextBox;
                if (!string.IsNullOrEmpty(textBox?.Text))
                {
                    if (textBox.Text.Contains(",") || textBox.Text.Contains(" "))
                    {
                        var spaceTab = textBox.Text.Split(' ').ToList();
                        var dotTab = textBox.Text.Split(',').ToList();
                        var spaceDetected = ValidateLottoNewLuckyNumber(spaceTab);
                        if (ValidateLottoNewLuckyNumber(dotTab) || spaceDetected)
                        {
                            if (spaceDetected)
                                dotTab = spaceTab;

                            luckyNumbersToSave.Add(ConvertTab2String(dotTab));

                            AddNewLottoTextBox();
                        }
                        else
                        {
                            throw new AitAccountExceptions.InvalidValueException(string.Format(WPF.Properties.Resources.INVALID_LOTTO_NUMBER, textBox.Text));
                        }
                    }
                    else
                    {
                        throw new AitAccountExceptions.InvalidValueException(string.Format(WPF.Properties.Resources.INVALID_LOTTO_NUMBER, textBox.Text));
                    }
                }

                if (string.IsNullOrEmpty(textBox.Text) && MessageContentList.Items.Count != 1)
                {
                    MessageContentList.Items.Remove(sender);
                }

                ValidateNotDefaultNote();
                ValidateRequiredFieldFillCorrectly();
            }
            catch (Exception ex)
            {
                LogManager.Instance.LogExceptionToFileAndDB(ex);
            }
        }
        
        private void MessageContentList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = MessageContentList.SelectedItem as ListViewItem;
            var textBox = item?.Content as TextBox;
            if (textBox != null)
                textBox.Focus();
        }

        private bool ValidateLottoNewLuckyNumber(List<string> tab)
        {
            tab.ForEach(q => q = q.Replace(" ", string.Empty));
            tab = tab.Distinct().ToList();

            if (tab == null || tab.Count != 6)
                return false;

            foreach(var input in tab)
            {
                var number = -1;
                if (!int.TryParse(input, out number) || number < 0 || number > 50)
                {
                    return false;
                }
            }

            return true;
        }

        private string ConvertTab2String(List<string> tab)
        {
            var result = string.Empty;
            var index = 0;
            foreach(var value in tab)
            {
                result += index > 0 ? ", " + value : "" + value;
                index++;
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

        #region Filter Menu

        private FileTypesEnum? CheckIfFilterIsSelected()
        {
            foreach (MenuItem item in NoteManagerFilter.Items)
            {
                var index = -1;
                foreach (MenuItem subItem in item.Items)
                {
                    if (subItem.IsEnabled)
                    {
                        if (!subItem.Header.Equals("All"))
                        {
                            var model = FileTypesManager.SetType(index);
                            return model.EnumType;
                        }
                        else
                            return null;
                    }
                    index++;
                }
            }
            return null;
        }

        private void ALL_Click(object sender, RoutedEventArgs e)
        {
            ClearContentAction();
            InitListView();
            SetEditableMenuItem(sender);
        }

        private void ACTIVATION_CODE_Click(object sender, RoutedEventArgs e)
        {
            ClearContentAction();
            InitListView(FileTypesEnum.ACTIVATION_CODE);
            SetEditableMenuItem(sender);
        }

        private void EXCEPTION_Click(object sender, RoutedEventArgs e)
        {
            ClearContentAction();
            InitListView(FileTypesEnum.EXCEPTION);
            SetEditableMenuItem(sender);
        }

        private void INFORMATION_Click(object sender, RoutedEventArgs e)
        {
            ClearContentAction();
            InitListView(FileTypesEnum.INFORMATION);
            SetEditableMenuItem(sender);
        }

        private void KEYLOGGER_Click(object sender, RoutedEventArgs e)
        {
            ClearContentAction();
            InitListView(FileTypesEnum.KEYLOGGER);
            SetEditableMenuItem(sender);
        }

        private void LOTTO_NOTE_Click(object sender, RoutedEventArgs e)
        {
            ClearContentAction();
            InitListView(FileTypesEnum.LOTTO_NOTE);
            SetEditableMenuItem(sender);
        }

        private void NOTE_Click(object sender, RoutedEventArgs e)
        {
            ClearContentAction();
            InitListView(FileTypesEnum.NOTE);
            SetEditableMenuItem(sender);
        }

        private void QUERY_Click(object sender, RoutedEventArgs e)
        {
            ClearContentAction();
            InitListView(FileTypesEnum.QUERY);
            SetEditableMenuItem(sender);
        }

        private void TASK_Click(object sender, RoutedEventArgs e)
        {
            ClearContentAction();
            InitListView(FileTypesEnum.TASK);
            SetEditableMenuItem(sender);
        }

        private void TRACE_Click(object sender, RoutedEventArgs e)
        {
            ClearContentAction();
            InitListView(FileTypesEnum.TRACE);
            SetEditableMenuItem(sender);
        }

        private void UNDEFINED_Click(object sender, RoutedEventArgs e)
        {
            ClearContentAction();
            InitListView(FileTypesEnum.UNDEFINED);
            SetEditableMenuItem(sender);
        }

        private void DETACHED_Click(object sender, RoutedEventArgs e)
        {
            ClearContentAction();
            InitListView(FileTypesEnum.DETACHED);
            SetEditableMenuItem(sender);
        }

        private void SetEditableMenuItem(object sender)
        {
            foreach (MenuItem item in NoteManagerFilter.Items)
            {
                foreach (MenuItem subItem in item.Items)
                {
                    if (subItem.Header.Equals((sender as MenuItem)?.Header))
                        subItem.IsEnabled = false;
                    else
                        subItem.IsEnabled = true;
                }
            }
        }

        private void CreateFilterPanel()
        {
            NoteManagerFilter.Items.Clear();
            var noteFilter = new MenuItem();
            noteFilter.Header = "Filters";
            noteFilter.Background = Brushes.Transparent;
            noteFilter.Foreground = Brushes.White;

            var obj = new MenuItem();
            obj.Header = "All";
            obj.Background = Brushes.LightGray;
            obj.Foreground = Brushes.Black;
            obj.IsEnabled = false;
            obj.Click += ALL_Click;
            noteFilter.Items.Add(obj);

            if (account != null)
            {
                if (FileTypesManager.AccountHasPermitionToFile(account.Permition, FileTypesEnum.UNDEFINED))
                {
                    obj = new MenuItem();
                    obj.Header = "Undefined";
                    obj.Background = Brushes.LightGray;
                    obj.Foreground = Brushes.Black;
                    obj.Click += UNDEFINED_Click;
                    noteFilter.Items.Add(obj);
                }
                if (FileTypesManager.AccountHasPermitionToFile(account.Permition, FileTypesEnum.EXCEPTION))
                {
                    obj = new MenuItem();
                    obj.Header = "Exceptions";
                    obj.Background = Brushes.LightGray;
                    obj.Foreground = Brushes.Black;
                    obj.Click += EXCEPTION_Click;
                    noteFilter.Items.Add(obj);
                }
                if (FileTypesManager.AccountHasPermitionToFile(account.Permition, FileTypesEnum.INFORMATION))
                {
                    obj = new MenuItem();
                    obj.Header = "Informations";
                    obj.Background = Brushes.LightGray;
                    obj.Foreground = Brushes.Black;
                    obj.Click += INFORMATION_Click;
                    noteFilter.Items.Add(obj);
                }
                if (FileTypesManager.AccountHasPermitionToFile(account.Permition, FileTypesEnum.NOTE))
                {
                    obj = new MenuItem();
                    obj.Header = "Notes";
                    obj.Background = Brushes.LightGray;
                    obj.Foreground = Brushes.Black;
                    obj.Click += NOTE_Click;
                    noteFilter.Items.Add(obj);
                }
                if (FileTypesManager.AccountHasPermitionToFile(account.Permition, FileTypesEnum.TRACE))
                {
                    obj = new MenuItem();
                    obj.Header = "Traces";
                    obj.Background = Brushes.LightGray;
                    obj.Foreground = Brushes.Black;
                    obj.Click += TRACE_Click;
                    noteFilter.Items.Add(obj);
                }
                if (FileTypesManager.AccountHasPermitionToFile(account.Permition, FileTypesEnum.QUERY))
                {
                    obj = new MenuItem();
                    obj.Header = "Queries";
                    obj.Background = Brushes.LightGray;
                    obj.Foreground = Brushes.Black;
                    obj.Click += QUERY_Click;
                    noteFilter.Items.Add(obj);
                }
                if (FileTypesManager.AccountHasPermitionToFile(account.Permition, FileTypesEnum.TASK))
                {
                    obj = new MenuItem();
                    obj.Header = "Tasks";
                    obj.Background = Brushes.LightGray;
                    obj.Foreground = Brushes.Black;
                    obj.Click += TASK_Click;
                    noteFilter.Items.Add(obj);
                }
                if (FileTypesManager.AccountHasPermitionToFile(account.Permition, FileTypesEnum.KEYLOGGER))
                {
                    obj = new MenuItem();
                    obj.Header = "Keys Logger";
                    obj.Background = Brushes.LightGray;
                    obj.Foreground = Brushes.Black;
                    obj.Click += KEYLOGGER_Click;
                    noteFilter.Items.Add(obj);
                }
                if (FileTypesManager.AccountHasPermitionToFile(account.Permition, FileTypesEnum.ACTIVATION_CODE))
                {
                    obj = new MenuItem();
                    obj.Header = "Activation codes";
                    obj.Background = Brushes.LightGray;
                    obj.Foreground = Brushes.Black;
                    obj.Click += ACTIVATION_CODE_Click;
                    noteFilter.Items.Add(obj);
                }
                if (FileTypesManager.AccountHasPermitionToFile(account.Permition, FileTypesEnum.LOTTO_NOTE))
                {
                    obj = new MenuItem();
                    obj.Header = "Lotto Notes";
                    obj.Background = Brushes.LightGray;
                    obj.Foreground = Brushes.Black;
                    obj.Click += LOTTO_NOTE_Click;
                    noteFilter.Items.Add(obj);
                }
                if (FileTypesManager.AccountHasPermitionToFile(account.Permition, FileTypesEnum.DETACHED))
                {
                    obj = new MenuItem();
                    obj.Header = "Detached";
                    obj.Background = Brushes.LightGray;
                    obj.Foreground = Brushes.Black;
                    obj.Click += DETACHED_Click;
                    noteFilter.Items.Add(obj);
                }
            }

            NoteManagerFilter.Items.Add(noteFilter);
        }

        #endregion

        private void DetachedSelectedItems_Click(object sender, RoutedEventArgs e)
        {
            DispatcherExtension.Invoke(() =>
            {
                foreach (NoteListViewItemControl item in NoteManagerListView.SelectedItems)
                {
                    item.DetachNote();
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
            ClearContentAction();
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

        private void InitListView(FileTypesEnum? optionalType = null)
        {
            NoteManagerListView.Items.Clear();

            if(optionalType == null)
                optionalType = CheckIfFilterIsSelected();

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
                        if(ValidateNoteFilters(note, optionalType))
                        {
                            NoteManagerListView.Items.Add(new NoteListViewItemControl(index, note));
                            index++;
                        }
                    }
                }
            }

            if(NoteManagerListView.Items.Count == 1)
            {
                NoteManagerListView.SelectedIndex = 0;
                SetOneNoteContentAction();
            }

            if(NoteManagerListView.Items.Count == 0)
            {
                NoteManagerListView.Visibility = Visibility.Collapsed;
                NoteManagerListViewEmpty.Visibility = Visibility.Visible;
            }
        }

        private bool ValidateNoteFilters(AitFileModel note, FileTypesEnum? optionalType)
        {
            if(FileTypesManager.Types.Where(q => (int)account.Permition >= (int)q.PermitionLevel && q.EnumType.Equals(note.Type)).Any())
            {
                if(FileTypesEnum.DETACHED.Equals(optionalType) && note.IsDetached)
                    return true;

                if (optionalType == null || note.Type.Equals(optionalType))
                    return true;

            }

            return false;
        }

        #endregion

        #region Private Methods

        private void ClearContentAction()
        {
            NoteNameBox.Text = string.Empty;
            NoteAssignedToBox.Text = string.Empty;
            NoteTypeComboBox.SelectedIndex = -1;
            type = NoteTypeComboBox.SelectedItem as FileTypeModel;
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
            if (item != null)
            {
                NoteNameBox.Text = item.Note.Name;
                var assignId = item.Note.AssignedTo;
                using (var context = PDBContext.Instance.Context)
                {
                    var accs = context.Accounts.Where(q => q.ID.Equals(assignId)).ToList();
                    if (accs != null)
                    {
                        var index = 0;
                        var names = string.Empty;
                        foreach (var acc in accs)
                        {
                            acc.FillObject();
                            if (!string.IsNullOrEmpty(acc.UserData?.Nick))
                            {
                                names += acc.UserData.Nick;
                            }
                            else if (!string.IsNullOrEmpty(acc.UserData?.FullName))
                            {
                                names += acc.UserData.FullName;
                            }
                            else
                            {
                                names += acc.Login;
                            }

                            if (accs.Count - 1 != index)
                                names += ", ";

                            index++;
                        }
                        NoteAssignedToBox.Text = names;
                    }
                }

                StopClock = true;
                type = FileTypesManager.SetType((int)item.Note.Type);
                NoteTypeComboBox.SelectedItem = type;
                EditContentBtn.IsEnabled = true;

                try
                {
                    if (item.Note.Content.StartsWith("[{"))
                    {
                        MessageContent.Text = string.Empty;
                        var objs = CryptoJsonManager.Instance.Deserialize<List<LogInfoModel>>(item.Note.Content, false);
                        if (objs != null)
                        {
                            foreach (var obj in objs)
                            {
                                FillNoteFieldsFromNote(item.Note.Type, obj);
                            }
                        }
                    }
                    else
                    {
                        var obj = CryptoJsonManager.Instance.Deserialize<LogInfoModel>(item.Note.Content, false);
                        if (obj != null)
                        {
                            FillNoteFieldsFromNote(item.Note.Type, obj);
                        }
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

                foreach (ListViewItem element in MessageContentList.Items)
                {
                    var textbox = element?.Content as TextBox;
                    if (textbox != null)
                        textbox.IsEnabled = false;
                }
            }
        }

        private void FillNoteFieldsFromNote(FileTypesEnum type, LogInfoModel obj)
        {
            if (luckyNumbersToSave == null)
                throw new Exception();

            luckyNumbersToSave.Clear();

            if (FileTypesEnum.EXCEPTION.Equals(type))
            {
                foreach (var exception in obj.MessageInfo.ExceptionInfo.ToList())
                    MessageContent.Text += exception;
            }
            else if (FileTypesEnum.TRACE.Equals(type))
            {
                foreach (var element in obj.MessageInfo.Array.ToList())
                    MessageContent.Text += element;
            }
            else if (FileTypesEnum.LOTTO_NOTE.Equals(type))
            {
                MessageContent.Text = obj.MessageInfo.Message;

                if (obj.MessageInfo.Array != null)
                {
                    foreach (var line in obj.MessageInfo.Array.ToList())
                    {
                        luckyNumbersToSave.Add(line);
                        AddNewLottoTextBox(line);
                    }
                }
            }
            else
            {
                MessageContent.Text = obj.MessageInfo.Message;
            }
        }

        private void AddNewLottoTextBox(string text = "")
        {
            var lottoTextbox = new TextBox();
            lottoTextbox.LostFocus += LottoTextbox_LostFocus;
            lottoTextbox.HorizontalAlignment = HorizontalAlignment.Center;
            lottoTextbox.MinWidth = 150;
            lottoTextbox.MaxLines = 1;
            lottoTextbox.Background = Brushes.AliceBlue;


            var toAdd = true;
            if (!string.IsNullOrEmpty(text))
            {
                lottoTextbox.Text = text;
                lottoTextbox.IsEnabled = false;

                var anyFilled = false;
                foreach (ListViewItem item in MessageContentList.Items)
                {
                    var textBox = item?.Content as TextBox;
                    if (!string.IsNullOrEmpty(textBox.Text))
                        anyFilled = true;
                }

                if (!anyFilled)
                    MessageContentList.Items.Clear();
            }
            else
            {
                foreach (ListViewItem item in MessageContentList.Items)
                {
                    var textBox = item?.Content as TextBox;
                    if (string.IsNullOrEmpty(textBox.Text))
                        toAdd = false;
                }
            }

            if (toAdd)
            {
                MessageContentList.Items.Add(new ListViewItem { Content = lottoTextbox });
            }
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
            luckyNumbersToSave.Clear();
            MessageContentList.Items.Clear();

            if (!fullClear)
                AddNewLottoTextBox();
        }

        #endregion

        #region Public Methods

        public void RefreshList()
        {
            InitListView();
        }

        #endregion
    }
}
