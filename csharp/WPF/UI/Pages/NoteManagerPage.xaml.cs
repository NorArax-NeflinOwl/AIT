using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Linq;
using System.Windows.Controls;
using WPF.Databases.Contexts;
using WPF.Databases.Models;
using WPF.Models.Interfaces;
using WPF.UI.Controls;

namespace WPF.UI.Pages
{
    /// <summary>
    /// Interaction logic for NoteManagerPage.xaml
    /// </summary>
    public partial class NoteManagerPage : Page, IDisposableExtended, IPropertizableControl
    {
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
            NoteManagerTitle.Text = WPF.Properties.Resources.NOTEMANAGER_HEADER;
            DeleteSelectedItems.Content = WPF.Properties.Resources.DELETE_BTNCONTENT;

            InitListView();
        }

        public void Subscribe()
        {
            NoteManagerListView.MouseDoubleClick += NoteManagerListView_MouseDoubleClick;
            NoteManagerListView.SelectionChanged += NoteManagerListView_SelectionChanged;
            SelectAllCheckBox.Click += SelectAllCheckBox_Click;
            DeleteSelectedItems.Click += DeleteSelectedItems_Click;
        }

        public void Dispose()
        {
            NoteManagerListView.MouseDoubleClick -= NoteManagerListView_MouseDoubleClick;
            NoteManagerListView.SelectionChanged -= NoteManagerListView_SelectionChanged;

            SelectAllCheckBox.Click -= SelectAllCheckBox_Click;
            DeleteSelectedItems.Click += DeleteSelectedItems_Click;

            IsDisposed = true;
            GC.Collect();
        }

        private void DeleteSelectedItems_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            foreach (NoteListViewItemControl item in NoteManagerListView.SelectedItems)
            {
                var copy = (AitFilesModel)item.Note.Clone();
                copy.Delete();
                copy.Context.SaveChanges();
            }
            NoteManagerListView.SelectedItems.Clear();
            DeleteSelectedItems.IsEnabled = false;

            NoteManagerListView.Items.Clear();
            InitListView();
        }

        private void SelectAllCheckBox_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if(SelectAllCheckBox.IsChecked == true)
            {
                foreach (var item in NoteManagerListView.Items)
                    NoteManagerListView.SelectedItems.Add(item);
            }
            if(SelectAllCheckBox.IsChecked == false)
            {
                NoteManagerListView.SelectedItems.Clear();
                DeleteSelectedItems.IsEnabled = false;
            }
        }

        private void NoteManagerListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(NoteManagerListView.SelectedItems.Any())
                DeleteSelectedItems.IsEnabled = true;

            //TODO
        }

        private void NoteManagerListView_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            NoteManagerListView.SelectedItem = null;
            if (NoteManagerListView.SelectedItems.Any())
                DeleteSelectedItems.IsEnabled = true;
        }

        private void InitListView()
        {
            using (var context = PDBContext.Instance.Context)
            {
                var account = context.Accounts.Where(q => q.ID.Equals(PDBContext.Instance.AccountID)).FirstOrDefault();
                if (account != null)
                {
                    var index = 1;
                    foreach (var note in account.Files)
                    {
                        NoteManagerListView.Items.Add(new NoteListViewItemControl(index, note));
                        index++;
                    }
                }
            }
        }
    }
}
