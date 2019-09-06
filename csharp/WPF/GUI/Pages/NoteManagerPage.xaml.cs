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
using System.Configuration;

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
                if (item?.Content is TextBox textBox)
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
                            var newNote = new AitFileModel(context)
                            {
                                ID = Generators.RecordIDGenerator(TableInerfixEnum.FLS),
                                Name = NoteNameBox.Text,
                                Type = (NoteTypeComboBox.SelectedItem as FileTypeModel)?.EnumType ?? FileTypesEnum.UNDEFINED,
                                Content = SerializableControl()
                            };
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
            List<string> array = new List<string>();
            if(type != null)
            {
                switch(type.EnumType)
                {
                    case FileTypesEnum.LOTTO_NOTE:
                        foreach(ListViewItem item in MessageContentList.Items)
                        {
                            if(item.Content is TextBox textbox && !string.IsNullOrEmpty(textbox.Text))
                            {
                                array.Add(textbox.Text);
                            }
                        }
                        break;
                }
            }

            return CryptoJsonManager.Instance.Serialize(new MessageInfoModel(luckyNumbersToSave)
            {
                Message = MessageContent.Text,
                Array = array.ToArray()
            });
        }

        private void ClearContentBtn_Click(object sender, RoutedEventArgs e)
        {
            ClearContentAction();
        }

        private void NoteTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ValidateRequiredFieldFillCorrectly(ValidateNotDefaultNote());

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
            ValidateRequiredFieldFillCorrectly(ValidateNotDefaultNote());
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

            ValidateRequiredFieldFillCorrectly(ValidateNotDefaultNote());
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

        private bool ValidateNotDefaultNote()
        {
            if (NoteTypeComboBox.SelectedIndex >= 0
                || !string.IsNullOrEmpty(NoteNameBox.Text)
                || !string.IsNullOrEmpty(NoteAssignedToBox.Text)
                || !string.IsNullOrEmpty(MessageContent.Text) && TypeAllowToEmptyContent()
                || (luckyNumbersToSave.Any() && type != null && FileTypesEnum.LOTTO_NOTE.Equals(type.EnumType)))
            {
                ClearContentBtn.IsEnabled = true;
                return true;
            }
            else
            {
                ClearContentBtn.IsEnabled = false;
                return false;
            }
        }

        private bool TypeAllowToEmptyContent()
        {
            return NoteTypeComboBox.SelectedItem is FileTypeModel model && model.AllowToEmptyContent;
        }

        private void ValidateRequiredFieldFillCorrectly(bool editButtonWasEnabled)
        {
            if ((string.IsNullOrEmpty(NoteNameBox.Text) || CorrectlyAssign != false)
                && (IsCorrectFilled || TypeAllowToEmptyContent()) 
                && (!luckyNumbersToSave.Any() || (luckyNumbersToSave.Any() && type != null && FileTypesEnum.LOTTO_NOTE.Equals(type.EnumType)))
                && editButtonWasEnabled)
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

                ValidateRequiredFieldFillCorrectly(ValidateNotDefaultNote());
            }
            catch (Exception ex)
            {
                LogManager.Instance.LogExceptionToFileAndDB(ex);
            }
        }
        
        private void MessageContentList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = MessageContentList.SelectedItem as ListViewItem;
            if (item?.Content is TextBox textBox)
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
                    if (!subItem.IsEnabled && !subItem.Header.Equals("All"))
                    {
                        var model = FileTypesManager.SetType(index);
                        return model.EnumType;
                    }
                    if (subItem.IsEnabled && subItem.Header.Equals("All"))
                        return null;

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
            var noteFilter = new MenuItem
            {
                Header = "Filters",
                Background = Brushes.Transparent,
                Foreground = Brushes.White
            };

            var obj = new MenuItem
            {
                Header = "All",
                Background = Brushes.LightGray,
                Foreground = Brushes.Black,
                IsEnabled = false
            };
            obj.Click += ALL_Click;
            noteFilter.Items.Add(obj);

            if (account != null)
            {
                if (FileTypesManager.AccountHasPermitionToFile(account.Permition, FileTypesEnum.UNDEFINED))
                {
                    obj = new MenuItem
                    {
                        Header = "Undefined",
                        Background = Brushes.LightGray,
                        Foreground = Brushes.Black
                    };
                    obj.Click += UNDEFINED_Click;
                    noteFilter.Items.Add(obj);
                }
                if (FileTypesManager.AccountHasPermitionToFile(account.Permition, FileTypesEnum.EXCEPTION))
                {
                    obj = new MenuItem
                    {
                        Header = "Exceptions",
                        Background = Brushes.LightGray,
                        Foreground = Brushes.Black
                    };
                    obj.Click += EXCEPTION_Click;
                    noteFilter.Items.Add(obj);
                }
                if (FileTypesManager.AccountHasPermitionToFile(account.Permition, FileTypesEnum.INFORMATION))
                {
                    obj = new MenuItem
                    {
                        Header = "Informations",
                        Background = Brushes.LightGray,
                        Foreground = Brushes.Black
                    };
                    obj.Click += INFORMATION_Click;
                    noteFilter.Items.Add(obj);
                }
                if (FileTypesManager.AccountHasPermitionToFile(account.Permition, FileTypesEnum.NOTE))
                {
                    obj = new MenuItem
                    {
                        Header = "Notes",
                        Background = Brushes.LightGray,
                        Foreground = Brushes.Black
                    };
                    obj.Click += NOTE_Click;
                    noteFilter.Items.Add(obj);
                }
                if (FileTypesManager.AccountHasPermitionToFile(account.Permition, FileTypesEnum.TRACE))
                {
                    obj = new MenuItem
                    {
                        Header = "Traces",
                        Background = Brushes.LightGray,
                        Foreground = Brushes.Black
                    };
                    obj.Click += TRACE_Click;
                    noteFilter.Items.Add(obj);
                }
                if (FileTypesManager.AccountHasPermitionToFile(account.Permition, FileTypesEnum.QUERY))
                {
                    obj = new MenuItem
                    {
                        Header = "Queries",
                        Background = Brushes.LightGray,
                        Foreground = Brushes.Black
                    };
                    obj.Click += QUERY_Click;
                    noteFilter.Items.Add(obj);
                }
                if (FileTypesManager.AccountHasPermitionToFile(account.Permition, FileTypesEnum.TASK))
                {
                    obj = new MenuItem
                    {
                        Header = "Tasks",
                        Background = Brushes.LightGray,
                        Foreground = Brushes.Black
                    };
                    obj.Click += TASK_Click;
                    noteFilter.Items.Add(obj);
                }
                if (FileTypesManager.AccountHasPermitionToFile(account.Permition, FileTypesEnum.KEYLOGGER))
                {
                    obj = new MenuItem
                    {
                        Header = "Keys Logger",
                        Background = Brushes.LightGray,
                        Foreground = Brushes.Black
                    };
                    obj.Click += KEYLOGGER_Click;
                    noteFilter.Items.Add(obj);
                }
                if (FileTypesManager.AccountHasPermitionToFile(account.Permition, FileTypesEnum.ACTIVATION_CODE))
                {
                    obj = new MenuItem
                    {
                        Header = "Activation codes",
                        Background = Brushes.LightGray,
                        Foreground = Brushes.Black
                    };
                    obj.Click += ACTIVATION_CODE_Click;
                    noteFilter.Items.Add(obj);
                }
                if (FileTypesManager.AccountHasPermitionToFile(account.Permition, FileTypesEnum.LOTTO_NOTE))
                {
                    obj = new MenuItem
                    {
                        Header = "Lotto Notes",
                        Background = Brushes.LightGray,
                        Foreground = Brushes.Black
                    };
                    obj.Click += LOTTO_NOTE_Click;
                    noteFilter.Items.Add(obj);
                }
                if (FileTypesManager.AccountHasPermitionToFile(account.Permition, FileTypesEnum.DETACHED))
                {
                    obj = new MenuItem
                    {
                        Header = "Detached",
                        Background = Brushes.LightGray,
                        Foreground = Brushes.Black
                    };
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
            ClearContentAction();
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
            ChangeNoteManagerListViewVisibility();
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

        private void ChangeNoteManagerListViewVisibility()
        {
            if (NoteManagerListView.Items.Count == 0)
            {
                NoteManagerListView.Visibility = Visibility.Collapsed;
                NoteManagerListViewEmpty.Visibility = Visibility.Visible;
            }
            else
            {
                NoteManagerListView.Visibility = Visibility.Visible;
                NoteManagerListViewEmpty.Visibility = Visibility.Collapsed;
            }
        }

        private void ClearContentAction()
        {
            NoteNameBox.Text = string.Empty;
            NoteAssignedToBox.Text = string.Empty;
            NoteTypeComboBox.SelectedIndex = -1;
            type = NoteTypeComboBox.SelectedItem as FileTypeModel;
            MessageContent.Text = string.Empty;
            StopClock = false;
            luckyNumbersToSave.Clear();

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
            ChangeNoteManagerListViewVisibility();
        }

        private void SetOneNoteContentAction()
        {
            if (NoteManagerListView.SelectedItem is NoteListViewItemControl item)
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
                        var objs = CryptoJsonManager.Instance.Deserialize<List<MessageInfoModel>>(item.Note.Content, false);
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
                        var obj = CryptoJsonManager.Instance.Deserialize<MessageInfoModel>(item.Note.Content, false);
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

                var sysManager = ConfigurationManager.AppSettings["TasksManager"].ToString();
                if (!string.IsNullOrEmpty(item.Note.AssignedTo) || !string.IsNullOrEmpty(item.Note.AssignedTo) && !item.Note.AssignedTo.Equals(sysManager))
                {
                    EditContentBtn.Visibility = Visibility.Visible;
                }

                Date.Text = item.Note.Create.ToString("dd/MM/yyyy HH:mm:ss");
                currentNote = item.Note;

                NoteNameBox.IsEnabled = false;
                NoteTypeComboBox.IsEnabled = false;
                NoteAssignedToBox.IsEnabled = false;
                MessageContent.IsEnabled = false;

                foreach (ListViewItem element in MessageContentList.Items)
                {
                    if (element?.Content is TextBox textbox)
                        textbox.IsEnabled = false;
                }
            }
        }

        private void FillNoteFieldsFromNote(FileTypesEnum type, MessageInfoModel obj)
        {
            if (luckyNumbersToSave == null)
                throw new Exception();

            luckyNumbersToSave.Clear();

            if (FileTypesEnum.EXCEPTION.Equals(type))
            {
                foreach (var exception in obj.ExceptionInfo.ToList())
                    MessageContent.Text += exception;
            }
            else if (FileTypesEnum.TRACE.Equals(type))
            {
                foreach (var element in obj.Array.ToList())
                    MessageContent.Text += element;
            }
            else if (FileTypesEnum.LOTTO_NOTE.Equals(type))
            {
                MessageContent.Text = obj.Message;

                if (obj.Array != null)
                {
                    foreach (var line in obj.Array.ToList())
                    {
                        luckyNumbersToSave.Add(line);
                        AddNewLottoTextBox(line);
                    }
                }
            }
            else
            {
                MessageContent.Text = obj.Message;
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
                if (item?.Content is TextBox textbox)
                {
                    textbox.LostFocus -= LottoTextbox_LostFocus;
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
