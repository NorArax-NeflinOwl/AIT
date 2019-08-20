using System.Windows.Controls;
using WPF.Databases.Models;

namespace WPF.UI.Controls
{
    /// <summary>
    /// Interaction logic for NoteListViewItemControl.xaml
    /// </summary>
    public partial class NoteListViewItemControl : UserControl
    {
        public AitFilesModel Note {get;set;}

        public NoteListViewItemControl(int index, AitFilesModel note)
        {
            Note = note;
            InitializeComponent();
            Title.Text = index + " " + Note.Name;
            CreateDate.Text = Properties.Resources.CREATE_S + " " + Note.Create.ToString("dd/MM/yyyy hh:mm:ss");
            Type.Text = Properties.Resources.TYPE_S + " " + Note.Type.ToString();
        }
    }
}
