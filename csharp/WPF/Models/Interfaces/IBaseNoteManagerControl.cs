using WPF.GUI.Controls;

namespace WPF.Models.Interfaces
{
    public interface IBaseNoteManagerControl : IDisposableExtended, IPropertizableControl
    {
        bool IsCorrectlyFilled { get; set; }
        FileTypeModel Type { get; set; }

        string SerializableControl();
        bool TypeAllowToEmptyContent();
        bool ValidateNotDefaultNote();
        bool ValidateRequiredFieldFillCorrectly();
        void SetOneNoteContentAction(NoteListViewItemControl ctrl);
        void EditContentBtn_Click();
        void ClearContentAction();
    }
}
