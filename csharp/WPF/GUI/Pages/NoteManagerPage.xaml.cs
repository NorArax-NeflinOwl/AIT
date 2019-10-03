using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WPF.Databases.Contexts;
using WPF.Databases.Models;
using WPF.Managers;
using WPF.Models;
using WPF.Models.Enums;
using WPF.Models.Interfaces;
using WPF.GUI.Controls;
using System.Configuration;
using System.Windows.Input;
using WPF.GUI.Windows.Properties;
using WPF.GUI.Controls.NoteManagerControls;
using System.Collections.Generic;
using System.Windows.Navigation;

namespace WPF.GUI.Pages
{
    /// <summary>
    /// Interaction logic for NoteManagerPage.xaml
    /// </summary>
    public partial class NoteManagerPage : Page, IDisposableExtended, IPropertizableControl, ISerializableForSession
    {
        #region Fields

        public bool IsDisposed { get; set; }
        public IProperties Properties { get; }
        public NoteFiltersManager FilterManager { get; set; }
        public RightPanelNoteControl RightPanel { get; set; }

        #endregion

        #region Constructor & Interface Implementation

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

            IsDisposed = true;

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
        }

        public void Init()
        {
            RightPanel = new RightPanelNoteControl(this);
            FilterManager = new NoteFiltersManager(this, RightPanel.Control);
            InitListView();
            FilterManager.CreateFilterPanel(RightPanel.Account);

            NoteManagerTitle.Text = WPF.Properties.Resources.NOTEMANAGER_HEADER;
            DeleteSelectedItems.Content = WPF.Properties.Resources.DELETE_BTNCONTENT;
            DetachedSelectedItems.Content = WPF.Properties.Resources.DETACHED_BTNCONTENT;
            NoteManagerListViewEmpty.Text = WPF.Properties.Resources.LIST_EMPTY;
            
            // TODO Set note fields hits from WPF.Properties.Resources

            if (RightPanel.Account != null && RightPanel.Account.Permition.Equals(PermitionAccountEnum.ADMIN))
            {
                DeleteSelectedItems.Visibility = Visibility.Visible;
            }


            if (RightPanelFrame == null)
                RightPanelFrame = new Frame();

            RightPanelFrame.Content = RightPanel;
            RightPanelFrame.DataContext = RightPanel;
            RightPanelFrame.Visibility = Visibility.Visible;
        }

        public void SerializeSession()
        {
            // TODO save in dictionary mark filter, focused file and unsaved temp file
            if(!string.IsNullOrEmpty(SearchNoteItemTextBox.Text))
            {
                PDBContext.Instance.SessionDictionary[nameof(SearchNoteItemTextBox)] = SearchNoteItemTextBox.Text;
            }

            if(NoteManagerListView.SelectedItems.Any())
            {
                var selectedFiles = new List<string>();
                foreach(NoteListViewItemControl file in NoteManagerListView.SelectedItems)
                {
                    selectedFiles.Add(file.Note.ID);
                }
                PDBContext.Instance.SessionDictionary[nameof(NoteListViewItemControl)] = CryptoJsonManager.Instance.Serialize(selectedFiles);

                if (PDBContext.Instance.SessionDictionary.Count == 1)
                {
                    RightPanel.SerializeSession();
                }
            }
        }

        public void DeserializaSession()
        {
            // TODO
        }

        #region Public Methods

        public void InitListView(FileTypesEnum? optionalType = null)
        {
            NoteManagerListView.Items.Clear();

            if (optionalType == null)
                optionalType = FilterManager.CheckIfFilterIsSelected();

            var searchItem = SearchNoteItemTextBox.Text ?? string.Empty;

            using (var context = PDBContext.Instance.Context)
            {
                if (RightPanel.Account != null)
                {
                    var index = 1;
                    RightPanel.Account.FillObject();
                    var list = RightPanel.Account.Files.ToList();
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

            RightPanel.EditContentBtn.Visibility = Visibility.Collapsed;
            if (!NoteManagerListView.SelectedItems.Any())
            {
                RightPanel.CreateNewNoteGrid.Visibility = Visibility.Visible;
                RightPanel.OpenMultiNoteGrid.Visibility = Visibility.Collapsed;
                ClearContentAction();
            }
            else if (NoteManagerListView.SelectedItems.Count == 1)
            {
                RightPanel.CreateNewNoteGrid.Visibility = Visibility.Visible;
                RightPanel.OpenMultiNoteGrid.Visibility = Visibility.Collapsed;
                SetOneNoteContentAction();
            }
            else
            {
                RightPanel.CreateNewNoteGrid.Visibility = Visibility.Collapsed;
                RightPanel.OpenMultiNoteGrid.Visibility = Visibility.Visible;

                return true;
            }
            return false;
        }

        private bool ValidateNoteFilters(AitFileModel note, FileTypesEnum? optionalType)
        {
            if (FileTypesManager.Types.Where(q => (int)RightPanel.Account.Permition >= (int)q.PermitionLevel && q.EnumType.Equals(note.Type)).Any())
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
                RightPanel.Type = FileTypesManager.SetType((int)item.Note.Type);
                RightPanel.NoteNameBox.Text = item.Note.Name;
                using (var context = PDBContext.Instance.Context)
                {
                    var names = string.Empty;
                    var files = context.Files.Where(q => q.Name.Equals(item.Note.Name)).ToList();
                    foreach (var file in files)
                    {
                        var accs = context.Accounts.Where(q => q.ID.Equals(file.Creator) || q.ID.Equals(file.AssignedTo)).ToList();
                        if (accs != null && string.IsNullOrEmpty(names))
                        {
                            var index = 0;
                            foreach (var acc in accs)
                            {
                                if((int)RightPanel.Account.Permition >= (int)acc.Permition)
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
                    RightPanel.NoteAssignedToBox.Text = names;
                }

                RightPanel.StopClock = true;
                RightPanel.NoteTypeComboBox.SelectedItem = RightPanel.Type;
                RightPanel.EditContentBtn.IsEnabled = true;

                RightPanel.Control.SetOneNoteContentAction(item);

                var sysManager = ConfigurationManager.AppSettings["TasksManager"].ToString();
                if (string.IsNullOrEmpty(item.Note.AssignedTo) || !string.IsNullOrEmpty(item.Note.AssignedTo) && !item.Note.AssignedTo.Equals(sysManager))
                {
                    RightPanel.EditContentBtn.Visibility = Visibility.Visible;
                }

                RightPanel.Date.Text = item.Note.Create.ToString("dd/MM/yyyy HH:mm:ss");
                RightPanel.CurrentNote = item.Note;

                RightPanel.NoteNameBox.IsEnabled = false;
                RightPanel.NoteTypeComboBox.IsEnabled = false;
                RightPanel.NoteAssignedToBox.IsEnabled = false;
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
            MainContext.Instance.Windows.Open(new DialogProperties(DialogTypeEnum.NOTE_DETACHED_FOR_REASON, nameof(NoteManagerPage), this));
        }

        private void DeleteSelectedItems_Click(object sender, RoutedEventArgs e)
        {
            MainContext.Instance.Windows.Open(new DialogProperties(DialogTypeEnum.NOTE_DELETE_FOR_REASON, nameof(NoteManagerPage), this));
        }

        private void SelectAllCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (SelectAllCheckBox.IsChecked == true)
            {
                RightPanel.CreateNewNoteGrid.Visibility = Visibility.Collapsed;
                RightPanel.OpenMultiNoteGrid.Visibility = Visibility.Visible;

                foreach (var item in NoteManagerListView.Items)
                    NoteManagerListView.SelectedItems.Add(item);
            }
            if (SelectAllCheckBox.IsChecked == false)
            {
                NoteManagerListView.SelectedItems.Clear();
                ClearContentAction();

                RightPanel.CreateNewNoteGrid.Visibility = Visibility.Visible;
                RightPanel.OpenMultiNoteGrid.Visibility = Visibility.Collapsed;
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

        private void HandleNavigating(object sender, NavigatingCancelEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.Forward)
            {
                e.Cancel = true;
            }
        }

        #endregion

        #endregion

        #region Public Methods

        public void RefreshList()
        {
            InitListView();
        }

        public void RefreshLayout()
        {
            RightPanel.RefreshLayout();
        }

        public void ClearContentAction()
        {
            RightPanel.ClearContentAction();
            ChangeNoteManagerListViewVisibility();
        }

        #endregion
    }
}
