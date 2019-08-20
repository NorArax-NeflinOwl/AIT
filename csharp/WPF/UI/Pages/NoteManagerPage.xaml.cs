using System;
using System.Linq;
using System.Windows.Controls;
using WPF.Databases.Contexts;
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
        }

        public bool IsDisposed { get; set; }

        public IProperties Properties { get; set; }

        public void Dispose()
        {
            NoteManagerListView.MouseDoubleClick -= NoteManagerListView_MouseDoubleClick;
            NoteManagerListView.SelectionChanged -= NoteManagerListView_SelectionChanged;

            SelectAllCheckBox.Click -= SelectAllCheckBox_Click;

            IsDisposed = true;
            GC.Collect();
        }

        public void Init()
        {
            NoteManagerTitle.Text = WPF.Properties.Resources.NOTEMANAGER_HEADER;

            using(var context = PDBContext.Instance.Context)
            {
                var account = context.Accounts.Where(q => q.ID.Equals(PDBContext.Instance.AccountID)).FirstOrDefault();
                if(account != null)
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

        public void Subscribe()
        {
            NoteManagerListView.MouseDoubleClick += NoteManagerListView_MouseDoubleClick;
            NoteManagerListView.SelectionChanged += NoteManagerListView_SelectionChanged;
            SelectAllCheckBox.Click += SelectAllCheckBox_Click;
        }

        private void SelectAllCheckBox_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if(SelectAllCheckBox.IsChecked == true)
            {
                NoteManagerListView.SelectAll();
            }
            if(SelectAllCheckBox.IsChecked == false)
            {
                NoteManagerListView.SelectedItems.Clear();
            }
        }

        private void NoteManagerListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //TODO
        }

        private void NoteManagerListView_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
        }
    }
}
