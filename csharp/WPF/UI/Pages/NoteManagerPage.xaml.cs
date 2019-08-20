using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WPF.Databases.Contexts;
using WPF.Databases.Models;
using WPF.Models.Enums;
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
            NoteManagerTitle.Text = WPF.Properties.Resources.NOTEMANAGER_HEADER;
            DeleteSelectedItems.Content = WPF.Properties.Resources.DELETE_BTNCONTENT;
            DetachedSelectedItems.Content = WPF.Properties.Resources.DETACHED_BTNCONTENT;

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
        }

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
            InitListView();
        }

        private void SelectAllCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if(SelectAllCheckBox.IsChecked == true)
            {
                foreach (var item in NoteManagerListView.Items)
                    NoteManagerListView.SelectedItems.Add(item);
            }
            if(SelectAllCheckBox.IsChecked == false)
            {
                NoteManagerListView.SelectedItems.Clear();
            }
            ChangeButtonsEditability();
        }

        private void NoteManagerListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ChangeButtonsEditability();

            //TODO
        }

        private void NoteManagerListView_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            NoteManagerListView.SelectedItem = null;
            ChangeButtonsEditability();
        }

        private void ChangeButtonsEditability()
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
    }
}
