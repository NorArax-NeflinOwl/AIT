using System.Windows.Controls;
using WPF.Models;
using WPF.Models.Interfaces;

namespace WPF.GUI.Controls.NoteManagerControls
{
    /// <summary>
    /// Interaction logic for BaseNoteControl.xaml
    /// </summary>
    public partial class BaseNoteControl : UserControl, IBaseNoteManagerControl
    {
        public BaseNoteControl()
        {
            InitializeComponent();
        }

        public bool IsCorrectlyFilled { get; set; }
        public FileTypeModel Type { get; set; }
        public bool IsDisposed { get; set; }

        public IProperties Properties { get; }

        public void ClearContentAction()
        {
        }

        public void Dispose()
        {
        }

        public void EditContentBtn_Click()
        {
        }

        public void Init()
        {
        }

        public void Load()
        {
        }

        public string SerializableControl()
        {
            return string.Empty;
        }

        public void SetOneNoteContentAction(NoteListViewItemControl ctrl)
        {
        }

        public void Subscribe()
        {
        }

        public bool TypeAllowToEmptyContent()
        {
            return true;
        }

        public bool ValidateNotDefaultNote()
        {
            return true;
        }

        public bool ValidateRequiredFieldFillCorrectly()
        {
            return true;
        }
    }
}
