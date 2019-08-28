using System.Windows.Controls;
using WPF.Databases.Models;

namespace WPF.GUI.Controls
{
    /// <summary>
    /// Interaction logic for NoteListViewItemControl.xaml
    /// </summary>
    public partial class NoteListViewItemControl : UserControl
    {
        public AitFileModel Note {get;set;}

        public NoteListViewItemControl(int index, AitFileModel note)
        {
            Note = note;
            InitializeComponent();
            Title.Text = index + " " + Note.Name;
            CreateDate.Text = Properties.Resources.CREATE_S + " " + Note.Create.ToString("dd/MM/yyyy HH:mm:ss");
            Type.Text = Properties.Resources.TYPE_S + " " + Note.Type.ToString();

            if(note.IsDetached)
                AdditionalData.Text = Properties.Resources.DETACHED;
        }
    }
}
