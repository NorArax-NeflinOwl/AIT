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
using WPF.Models;
using WPF.Models.Enums;
using WPF.Models.Extensions;
using WPF.Models.Extensions.Exceptions;
using WPF.Models.Interfaces;
using WPF.GUI.Controls;
using System.Configuration;
using System.Windows.Input;
using WPF.Managers.Bilders;
using System.Windows.Navigation;

namespace WPF.GUI.Pages
{
    /// <summary>
    /// Interaction logic for NoteManagerPage.xaml
    /// </summary>
    public partial class NoteManagerPage : Page, IDisposableExtended, IPropertizableControl
    {
        #region Fields

        private AitAccountModel account;
        private AitFileModel currentNote;
        private BackgroundWorker backgroundWorker;
        private NoteFiltersManager filterManager;
        private FileTypeModel type;

        private IBaseNoteManagerControl ctrl;

        private bool? CorrectlyAssign;
        private bool IsCorrectlyFilled;
        private bool StopClock;

        public bool IsDisposed { get; set; }

        public IProperties Properties { get; }

        public FileTypeModel Type
        {
            get
            {
                return type;
            }
            set
            {
                ctrl = NoteManagerControlBilder.Build(value);
                NoteManagerContent = new Frame
                {
                    NavigationUIVisibility = NavigationUIVisibility.Hidden,
                    Content = ctrl
                };

                filterManager = new NoteFiltersManager(this, ctrl);
                filterManager.CreateFilterPanel(account);
                type = value;
            }
        }

        #endregion

        #region Constructor

        public NoteManagerPage()
        {
            InitializeComponent();
            Init();
            Subscribe();
        }
        
        public void Dispose()
        {
            NoteManagerListView.MouseDoubleClick -= NoteManagerListView_MouseDoubleClick;
            NoteManagerListView.SelectionChanged -= NoteManagerListView_SelectionChanged;

            SearchNoteItemTextBox.KeyUp -= SearchNoteItemTextBox_KeyUp;

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

            IsDisposed = true;

            account.Dispose();
            currentNote?.Dispose();
            backgroundWorker.Dispose();

            GC.Collect();
        }

        public void Subscribe()
        {
            NoteManagerListView.MouseDoubleClick += NoteManagerListView_MouseDoubleClick;
            NoteManagerListView.SelectionChanged += NoteManagerListView_SelectionChanged;

            SearchNoteItemTextBox.KeyUp += SearchNoteItemTextBox_KeyUp;

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
        }

        public void Init()
        {
            ctrl = NoteManagerControlBilder.Build(FileTypesManager.GetType(FileTypesEnum.UNDEFINED));
            filterManager = new NoteFiltersManager(this, ctrl);

            InitListView();
            InitNoteTypeComboBox();

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

            if (account != null && account.Permition.Equals(PermitionAccountEnum.ADMIN))
            {
                DeleteSelectedItems.Visibility = Visibility.Visible;
            }
            filterManager.CreateFilterPanel(account);

            backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += StartTimeTicker;
            backgroundWorker.RunWorkerAsync();
        }

        #region Private Methods

        private void InitNoteTypeComboBox()
        {
            foreach (var item in FileTypesManager.Types)
            {
                if ((int)account.Permition >= (int)item.PermitionLevel)
                {
                    NoteTypeComboBox.Items.Add(item);
                }
            }
        }

        #endregion

        #endregion

        #region Right Panel

        #region Events

        private void EditContentBtn_Click(object sender, RoutedEventArgs e)
        {
            NoteNameBox.IsEnabled = !NoteNameBox.IsEnabled;
            NoteTypeComboBox.IsEnabled = !NoteTypeComboBox.IsEnabled;
            NoteAssignedToBox.IsEnabled = !NoteAssignedToBox.IsEnabled;
            ctrl.EditContentBtn_Click();
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
                            var acc = ValidateAssignedToAccount(context, name.ToLower());
                            if(acc != null)
                            {
                                var file = context.Files.Where(q => acc.ID.Equals(q.Creator) || acc.ID.Equals(q.AssignedTo)).FirstOrDefault();
                                if (file == null)
                                {
                                    var creator = context.Accounts.Where(q => q.ID.Equals(PDBContext.Instance.AccountID)).FirstOrDefault();
                                    var newNote = new AitFileModel(context);

                                    if (clone != null)
                                    {
                                        newNote.FileCreator = clone.FileCreator;
                                        newNote.Create = clone.Create;
                                    }

                                    newNote.FileOwner = acc;
                                    newNote.Name = NoteNameBox.Text;
                                    newNote.Type = (NoteTypeComboBox.SelectedItem as FileTypeModel)?.EnumType ?? FileTypesEnum.UNDEFINED;
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
                            currentNote.Name = NoteNameBox.Text;
                            currentNote.Type = (NoteTypeComboBox.SelectedItem as FileTypeModel)?.EnumType ?? FileTypesEnum.UNDEFINED;
                            currentNote.Content = SerializableControl();
                            currentNote.Update();
                        }
                        else
                        {
                            var newNote = new AitFileModel(context)
                            {
                                Name = NoteNameBox.Text,
                                Type = (NoteTypeComboBox.SelectedItem as FileTypeModel)?.EnumType ?? FileTypesEnum.UNDEFINED,
                                Content = SerializableControl()
                            };
                            newNote.Insert();
                        }
                    }
                }

                InitListView();
                ClearContentAction();
            }
            catch (Exception ex)
            {
                LogManager.Instance.LogExceptionToFileAndDB(ex);
                InitListView();
            }
        }
        
        private void ClearContentBtn_Click(object sender, RoutedEventArgs e)
        {
            ClearContentAction();
            NoteManagerListView.SelectedIndex = -1;
        }

        private void NoteTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ValidateRequiredFieldFillCorrectly(ValidateNotDefaultNote());

            if (NoteTypeComboBox.SelectedIndex >= 0)
                NoteAssignedToBox.IsEnabled = true;
            else
                NoteAssignedToBox.IsEnabled = false;

            Type = NoteTypeComboBox.SelectedItem as FileTypeModel;

            if(Type != null)
            {
                ctrl = NoteManagerControlBilder.Build(Type);
                filterManager = new NoteFiltersManager(this, ctrl);
                filterManager.CreateFilterPanel(account);
                NoteManagerContent = new Frame
                {
                    NavigationUIVisibility = NavigationUIVisibility.Hidden,
                    Content = ctrl
                };
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
                            var accs = ValidateAssignedToAccount(context, name.ToLower());

                            if (accs == null)
                                exceptionName.Append(string.Format(WPF.Properties.Resources.INVALID_ACCOUNT_NAME, name) + Environment.NewLine);
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
            if (!string.IsNullOrEmpty(NoteNameBox.Text) && NoteTypeComboBox.SelectedIndex != -1)
                IsCorrectlyFilled = true;
            else
                IsCorrectlyFilled = false;

            ValidateRequiredFieldFillCorrectly(ValidateNotDefaultNote());
        }

        #endregion

        #region Private Methods

        private string SerializableControl()
        {
            return ctrl.SerializableControl();
        }

        private AitAccountModel ValidateAssignedToAccount(DBContext context, string name)
        {
            var accounts = context.Accounts.ToList();

            foreach (var account in accounts)
            {
                account.FillObject();
                if (name.Equals(account.Login.ToLower()))
                {
                    return account;
                }
                if (account.UserData != null)
                {
                    if (!string.IsNullOrEmpty(account.UserData.Nick) && name.Equals(account.UserData.Nick.ToLower()))
                    {
                        return account;
                    }
                    if (!string.IsNullOrEmpty(account.UserData.FullName) && name.Equals(account.UserData.FullName.ToLower().Replace(" ", string.Empty)))
                    {
                        return account;
                    }
                }
            }

            return null;
        }

        private bool ValidateNotDefaultNote()
        {
            if (NoteTypeComboBox.SelectedIndex >= 0
                || !string.IsNullOrEmpty(NoteNameBox.Text)
                || !string.IsNullOrEmpty(NoteAssignedToBox.Text)
                || ctrl.ValidateNotDefaultNote())
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

        private void ValidateRequiredFieldFillCorrectly(bool editButtonWasEnabled)
        {
            if ((string.IsNullOrEmpty(NoteNameBox.Text) || CorrectlyAssign != false)
                && ctrl.ValidateRequiredFieldFillCorrectly()
                && editButtonWasEnabled)
            {
                SaveContentBtn.IsEnabled = true;
            }
            else
            {
                SaveContentBtn.IsEnabled = false;
            }
        }

        #endregion

        #endregion

        #region Left Panel

        #region Public Methods

        public void InitListView(FileTypesEnum? optionalType = null)
        {
            NoteManagerListView.Items.Clear();

            if (optionalType == null)
                optionalType = filterManager.CheckIfFilterIsSelected();

            var searchItem = SearchNoteItemTextBox.Text ?? string.Empty;

            using (var context = PDBContext.Instance.Context)
            {
                account = context.Accounts.Where(q => q.ID.Equals(PDBContext.Instance.AccountID)).FirstOrDefault();
                if (account != null)
                {
                    var index = 1;
                    account.FillObject();
                    var list = account.Files.GroupBy(q => q.Name).Select(q => q.First()).OrderBy(q => q.Name).ToList();
                    if (!string.IsNullOrEmpty(searchItem))
                    {
                        list = list.Where(q => q.Name.ToLower().Contains(searchItem.ToLower()) || q.Content.ToLower().Contains(searchItem.ToLower())).ToList();
                    }

                    foreach (var note in list)
                    {
                        if (ValidateNoteFilters(note, optionalType))
                        {
                            NoteManagerListView.Items.Add(new NoteListViewItemControl(index, note));
                            index++;
                        }
                    }
                }
            }

            if (NoteManagerListView.Items.Count == 1)
            {
                NoteManagerListView.SelectedIndex = 0;
                SetOneNoteContentAction();
            }
            ChangeNoteManagerListViewVisibility();
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

        private bool ValidateNoteFilters(AitFileModel note, FileTypesEnum? optionalType)
        {
            if (FileTypesManager.Types.Where(q => (int)account.Permition >= (int)q.PermitionLevel && q.EnumType.Equals(note.Type)).Any())
            {
                if (FileTypesEnum.DETACHED.Equals(optionalType) && note.IsDetached)
                    return true;

                if (optionalType == null || note.Type.Equals(optionalType))
                    return true;

            }

            return false;
        }

        private void SetOneNoteContentAction()
        {
            if (NoteManagerListView.SelectedItem is NoteListViewItemControl item)
            {
                Type = FileTypesManager.SetType((int)item.Note.Type);
                NoteNameBox.Text = item.Note.Name;
                using (var context = PDBContext.Instance.Context)
                {
                    var names = string.Empty;
                    var files = context.Files.Where(q => q.Name.Equals(item.Note.Name)).ToList();
                    foreach (var file in files)
                    {
                        var accs = context.Accounts.Where(q => q.ID.Equals(file.Creator) || q.ID.Equals(file.AssignedTo)).ToList();
                        if (accs != null)
                        {
                            var index = 0;
                            foreach (var acc in accs)
                            {
                                if((int)account.Permition >= (int)acc.Permition)
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
                            }
                        }
                    }
                    NoteAssignedToBox.Text = names;
                }

                StopClock = true;
                NoteTypeComboBox.SelectedItem = Type;
                EditContentBtn.IsEnabled = true;

                ctrl.SetOneNoteContentAction(item);

                var sysManager = ConfigurationManager.AppSettings["TasksManager"].ToString();
                if (string.IsNullOrEmpty(item.Note.AssignedTo) || !string.IsNullOrEmpty(item.Note.AssignedTo) && !item.Note.AssignedTo.Equals(sysManager))
                {
                    EditContentBtn.Visibility = Visibility.Visible;
                }

                Date.Text = item.Note.Create.ToString("dd/MM/yyyy HH:mm:ss");
                currentNote = item.Note;

                NoteNameBox.IsEnabled = false;
                NoteTypeComboBox.IsEnabled = false;
                NoteAssignedToBox.IsEnabled = false;
            }
        }

        #endregion

        #region Events

        private void SearchNoteItemTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Enter))
            {
                InitListView();
            }
        }

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
                //SetMulitModePanel();
            }
        }

        private void NoteManagerListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ClearContentAction();
            if (ChangeButtonsEditabilityAndCheckMultiMode())
            {
                //TODO SetMulitModePanel();
            }
        }

        private void NoteManagerListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            NoteManagerListView.SelectedItem = null;
            if (ChangeButtonsEditabilityAndCheckMultiMode())
            {
                //TODO SetMulitModePanel();
            }
        }

        #endregion

        #endregion

        #region Private Methods

        private void ClearContentAction()
        {
            NoteNameBox.Text = string.Empty;
            NoteAssignedToBox.Text = string.Empty;
            NoteTypeComboBox.SelectedIndex = -1;
            StopClock = false;

            EditContentBtn.Visibility = Visibility.Collapsed;
            EditContentBtn.IsEnabled = false;
            ClearContentBtn.IsEnabled = false;
            SaveContentBtn.IsEnabled = false;

            NoteNameBox.IsEnabled = true;
            NoteTypeComboBox.IsEnabled = true;
            NoteAssignedToBox.IsEnabled = true;

            currentNote = null;
            Type = NoteTypeComboBox.SelectedItem as FileTypeModel;
            ChangeNoteManagerListViewVisibility();

            backgroundWorker.DoWork -= StartTimeTicker;
            backgroundWorker.Dispose();

            backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += StartTimeTicker;
            backgroundWorker.RunWorkerAsync();
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
