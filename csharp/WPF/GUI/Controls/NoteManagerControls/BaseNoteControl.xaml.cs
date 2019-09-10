using System.Windows.Controls;
using WPF.Managers;
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
            MessageContent.MaxLines = int.MaxValue;
            MessageContentInfo.Text = WPF.Properties.Resources.NOTE_TREEVIEW_S;
        }

        public void Load()
        {
        }

        public string SerializableControl()
        {
            return CryptoJsonManager.Instance.Serialize(new MessageInfoModel(MessageContent.Text));
        }

        public void SetOneNoteContentAction(NoteListViewItemControl ctrl)
        {
        }

        public void Subscribe()
        {
        }

        public bool TypeAllowToEmptyContent()
        {
            return Type.AllowToEmptyContent;
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
