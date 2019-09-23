using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using WPF.Databases.Contexts;
using WPF.Databases.Models;
using WPF.GUI.Pages;
using WPF.Managers;
using WPF.Managers.Bilders;
using WPF.Models;
using WPF.Models.Enums;
using WPF.Models.Extensions.Exceptions;
using WPF.Models.Interfaces;

namespace WPF.GUI.Controls.NoteManagerControls
{
    /// <summary>
    /// Interaction logic for RightPanelNoteControl.xaml
    /// </summary>
    public partial class RightPanelNoteControl : UserControl, IDisposableExtended, IPropertizableControl, ISerializableForSession
    {
        private bool? IsCorrectlyAssign;

        private FileTypeModel type;
        private NoteManagerPage page;
        private BackgroundWorker backgroundWorker;

        public bool StopClock { get; set; }
        public IBaseNoteManagerControl Control { get; set; }
        public AitFileModel CurrentNote { get; set; }
        public AitAccountModel Account { get; set; }

        public FileTypeModel Type
        {
            get
            {
                return type;
            }
            set
            {
                if (type == null && value != null || value != null && type?.EnumType.Equals(value.EnumType) == false)
                {
                    Control = NoteManagerControlBilder.Build(value);
                    Control.Load();

                    if (NoteManagerContent == null)
                        NoteManagerContent = new Frame();

                    NoteManagerContent.Content = Control;
                    NoteManagerContent.DataContext = Control;
                    NoteManagerContent.Visibility = Visibility.Visible;

                    page.FilterManager = new NoteFiltersManager(page, Control);
                    page.FilterManager.CreateFilterPanel(Account);
                }

                if (value == null)
                {
                    NoteManagerContent.Visibility = Visibility.Collapsed;
                }

                if (NoteManagerContent != null)
                    NoteManagerContent.NavigationUIVisibility = NavigationUIVisibility.Hidden;

                type = value;
            }
        }

        public bool IsDisposed { get; set; }

        public IProperties Properties { get; }

        public RightPanelNoteControl()
        {
            InitializeComponent();
        }

        public RightPanelNoteControl(NoteManagerPage page)
        {
            InitializeComponent();
            this.page = page;
            Init();
            Subscribe();
        }

        public void Dispose()
        {
            NoteTypeComboBox.SelectionChanged -= NoteTypeComboBox_SelectionChanged;
            NoteNameBox.TextChanged -= NoteTitleBox_TextChanged;

            EditContentBtn.Click -= EditContentBtn_Click;
            ClearContentBtn.Click += ClearContentBtn_Click;
            SaveContentBtn.Click += SaveContentBtn_Click;

            NoteNameBox.LostFocus -= MessageContent_LostFocus;
            NoteTypeComboBox.LostFocus -= MessageContent_LostFocus;
            NoteAssignedToBox.LostFocus -= NoteAssignedToBox_LostFocus;

            IsDisposed = true;
            Account.Dispose();

            CurrentNote?.Dispose();
            backgroundWorker.Dispose();

            GC.Collect();
        }

        public void Init()
        {
            Control = NoteManagerControlBilder.Build(FileTypesManager.GetType(FileTypesEnum.UNDEFINED));

            using(var context = PDBContext.Instance.Context)
            {
                Account = context.Accounts.Where(q => q.ID.Equals(PDBContext.Instance.AccountID)).FirstOrDefault();
            }

            InitNoteTypeComboBox();

            NoteContentTitle.Text = WPF.Properties.Resources.NOTE_CONTENT;
            EditContentBtn.Content = WPF.Properties.Resources.EDIT_HEADER;
            ClearContentBtn.Content = WPF.Properties.Resources.CLEAR;
            SaveContentBtn.Content = WPF.Properties.Resources.SAVE;

            MessageTitle.Text = WPF.Properties.Resources.MESSAGE;
            DateTitle.Text = WPF.Properties.Resources.DATE_S;
            
            backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += StartTimeTicker;
            backgroundWorker.RunWorkerAsync();
        }

        public void Subscribe()
        {
            NoteTypeComboBox.SelectionChanged += NoteTypeComboBox_SelectionChanged;
            NoteNameBox.TextChanged += NoteTitleBox_TextChanged;

            EditContentBtn.Click += EditContentBtn_Click;
            ClearContentBtn.Click += ClearContentBtn_Click;
            SaveContentBtn.Click += SaveContentBtn_Click;

            NoteNameBox.LostFocus += MessageContent_LostFocus;
            NoteTypeComboBox.LostFocus += MessageContent_LostFocus;
            NoteAssignedToBox.LostFocus += NoteAssignedToBox_LostFocus;
        }

        public void SerializeSession()
        {
            var session = PDBContext.Instance.SessionDictionary;

            // TODO save in dictionary mark filter, focused file and unsaved temp file
        }

        public void DeserializaSession()
        {
            // TODO
        }

        private void InitNoteTypeComboBox()
        {
            foreach (var item in FileTypesManager.Types)
            {
                if ((int)Account.Permition >= (int)item.PermitionLevel)
                {
                    NoteTypeComboBox.Items.Add(item);
                }
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

        private void EditContentBtn_Click(object sender, RoutedEventArgs e)
        {
            NoteNameBox.IsEnabled = !NoteNameBox.IsEnabled;
            NoteTypeComboBox.IsEnabled = !NoteTypeComboBox.IsEnabled;
            NoteAssignedToBox.IsEnabled = !NoteAssignedToBox.IsEnabled;
            Control.EditContentBtn_Click();
        }

        private void SaveContentBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(NoteAssignedToBox.Text))
                {
                    AitFileModel clone = null;
                    var names = NoteAssignedToBox.Text.Split(',', ';').ToList();
                    if (CurrentNote != null)
                    {
                        clone = (AitFileModel)CurrentNote.Clone();
                        CurrentNote.Delete();
                    }
                    foreach (var value in names)
                    {
                        var name = value.Replace(" ", string.Empty);
                        using (var context = PDBContext.Instance.Context)
                        {
                            var acc = ValidateAssignedToAccount(context, name.ToLower());
                            if (acc != null)
                            {
                                var file = context.Files.Where(q => (acc.ID.Equals(q.Creator) || acc.ID.Equals(q.AssignedTo)) && q.Name.Equals(NoteNameBox?.Text)).ToList();
                                if (file == null || !file.Any())
                                {
                                    var creator = context.Accounts.Where(q => q.ID.Equals(PDBContext.Instance.AccountID)).FirstOrDefault();
                                    var newNote = new AitFileModel(context);

                                    if (clone != null)
                                    {
                                        newNote.FileCreator = clone.FileCreator;
                                        newNote.Create = clone.Create;
                                        newNote.LastUpdate = DateTime.Now;
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
                        if (CurrentNote != null)
                        {
                            CurrentNote.Name = NoteNameBox.Text;
                            CurrentNote.Type = (NoteTypeComboBox.SelectedItem as FileTypeModel)?.EnumType ?? FileTypesEnum.UNDEFINED;
                            CurrentNote.Content = SerializableControl();
                            CurrentNote.Update();
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

                page.InitListView();
                ClearContentAction();
            }
            catch (Exception ex)
            {
                LogManager.Instance.LogExceptionToFileAndDB(ex);
                page.InitListView();
            }
        }

        private void ClearContentBtn_Click(object sender, RoutedEventArgs e)
        {
            ClearContentAction();
            page.NoteManagerListView.SelectedIndex = -1;
        }

        private void NoteTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ValidateRequiredFieldFillCorrectly(ValidateNotDefaultNote());

            if (NoteTypeComboBox.SelectedIndex >= 0)
                NoteAssignedToBox.IsEnabled = true;
            else
                NoteAssignedToBox.IsEnabled = false;

            Type = NoteTypeComboBox.SelectedItem as FileTypeModel;
        }

        private void NoteTitleBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ValidateRequiredFieldFillCorrectly(ValidateNotDefaultNote());
        }

        private void NoteAssignedToBox_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                IsCorrectlyAssign = false;
                if (!string.IsNullOrEmpty(NoteAssignedToBox.Text))
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

                IsCorrectlyAssign = true;
            }
            catch (Exception ex)
            {
                LogManager.Instance.LogExceptionToFileAndDB(ex);
            }

            MessageContent_LostFocus(sender, e);
        }

        private void MessageContent_LostFocus(object sender, RoutedEventArgs e)
        {
            ValidateRequiredFieldFillCorrectly(ValidateNotDefaultNote());
        }

        private string SerializableControl()
        {
            return Control.SerializableControl();
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
                || Control.ValidateNotDefaultNote())
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
            if ((string.IsNullOrEmpty(NoteNameBox.Text) || IsCorrectlyAssign != false)
                && Control.ValidateRequiredFieldFillCorrectly()
                && editButtonWasEnabled)
            {
                SaveContentBtn.IsEnabled = true;
            }
            else
            {
                SaveContentBtn.IsEnabled = false;
            }
        }

        public void RefreshLayout()
        {
            ValidateRequiredFieldFillCorrectly(ValidateNotDefaultNote());
        }

        public void ClearContentAction()
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

            CurrentNote = null;
            Type = NoteTypeComboBox.SelectedItem as FileTypeModel;

            backgroundWorker.DoWork -= StartTimeTicker;
            backgroundWorker.Dispose();

            backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += StartTimeTicker;
            backgroundWorker.RunWorkerAsync();
        }
    }
}
