using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using WPF.Databases.Contexts;
using WPF.Databases.Models;
using WPF.Managers;
using WPF.Managers.Helpers;
using WPF.Models.Enums;
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
        private bool CorrectlyAssign;

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

            NoteContentControl.Dispose();
            NoteContentControl = new NoteContentControl();

            NoteManagerTitle.Text = WPF.Properties.Resources.NOTEMANAGER_HEADER;
            DeleteSelectedItems.Content = WPF.Properties.Resources.DELETE_BTNCONTENT;
            DetachedSelectedItems.Content = WPF.Properties.Resources.DETACHED_BTNCONTENT;

            NoteContentTitle.Text = WPF.Properties.Resources.NOTE_CONTENT;
            EditContentBtn.Content = WPF.Properties.Resources.EDIT_HEADER;
            ClearContentBtn.Content = WPF.Properties.Resources.CLEAR;
            SaveContentBtn.Content = WPF.Properties.Resources.SAVE;
            // TODO Set note fields hits from WPF.Properties.Resources

            if (account != null && account.Permition.Equals(PermitionAccountEnum.ADMIN))
            {
                DeleteSelectedItems.Visibility = Visibility.Visible;
            }
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
        }

        #region RIGHT PANEL METHODS

        #region RIGHT PANEL METHODS - NEW FILE

        private void EditContentBtn_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void SaveContentBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var names = NoteAssignedToBox.Text.Split(',', ';').ToList();
                foreach (var name in names)
                {
                    using (var context = PDBContext.Instance.Context)
                    {
                        var acc = context.Accounts.Where(q => q.Login.ToLower().Equals(name.ToLower())
                                                               || q.UserData.Nick.ToLower().Equals(name.ToLower())
                                                               || q.UserData.FullName.ToLower().Equals(name.ToLower())).ToList();

                        foreach (var a in acc)
                        {
                            var file = new AitFilesModel(context)
                            {
                                ID = Generators.RecordIDGenerator(TableInerfixEnum.FLS),
                                Creator = PDBContext.Instance.AccountID,
                                AssignedTo = a.ID,
                                Name = NoteNameBox.Text,
                                Type = (FileTypesEnum)NoteTypeComboBox.SelectedIndex,
                                Content = NoteContentControl.SerializableControl()
                            };
                            file.Insert();
                            file.Context.SaveChanges();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Instance.LogExceptionToFile(ex);
            }
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

            NoteContentControl.Dispose();
            NoteContentControl = new NoteContentControl(NoteTypeComboBox.SelectedIndex);
        }

        private void NoteTitleBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ValidateNotDefaultNote();
            ValidateRequiredFieldFillCorrectly();
        }

        private void NoteAssignedToBox_LostFocus(object sender, RoutedEventArgs e)
        {
            ValidateNotDefaultNote();
            ValidateRequiredFieldFillCorrectly();

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
                        var acc = context.Accounts.Where(q => q.Login.ToLower().Equals(name.ToLower())
                                                               || q.UserData.Nick.ToLower().Equals(name.ToLower())
                                                               || q.UserData.FullName.ToLower().Equals(name.ToLower())).ToList();
                        if (acc == null || !acc.Any())
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
                || NoteContentControl.IsNotDefault)
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
                && (string.IsNullOrEmpty(NoteNameBox.Text) || CorrectlyAssign)
                && NoteContentControl.IsCorrectFilled)
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
            foreach (NoteListViewItemControl item in NoteManagerListView.SelectedItems)
            {
                var copy = (AitFilesModel)item.Note.Clone();
                copy.AssignedTo = string.Empty;
                copy.Creator = string.Empty;
                copy.Context.SaveChanges();
            }

            NoteManagerListView.SelectedItems.Clear();
            DetachedSelectedItems.IsEnabled = false;
            DeleteSelectedItems.IsEnabled = false;

            NoteManagerListView.Items.Clear();
            ClearContentAction();
            InitListView();
        }

        private void DeleteSelectedItems_Click(object sender, RoutedEventArgs e)
        {
            foreach (NoteListViewItemControl item in NoteManagerListView.SelectedItems)
            {
                var copy = (AitFilesModel)item.Note.Clone();
                copy.Delete();
                copy.Context.SaveChanges();
            }

            NoteManagerListView.SelectedItems.Clear();
            DetachedSelectedItems.IsEnabled = false;
            DeleteSelectedItems.IsEnabled = false;

            NoteManagerListView.Items.Clear();
            ClearContentAction();
            InitListView();
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
            using (var context = PDBContext.Instance.Context)
            {
                account = context.Accounts.Where(q => q.ID.Equals(PDBContext.Instance.AccountID)).FirstOrDefault();
                if (account != null)
                {
                    var index = 1;
                    foreach (var note in account.Files.ToList())
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
            NoteContentControl = new NoteContentControl(NoteTypeComboBox.SelectedIndex);
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
                            names += ", ";
                        }
                        NoteAssignedToBox.Text = names;
                    }
                }
                NoteContentControl.Dispose();
                NoteContentControl = new NoteContentControl(item.Note.Content, item.Note.Type);
            }
        }
    }
}
