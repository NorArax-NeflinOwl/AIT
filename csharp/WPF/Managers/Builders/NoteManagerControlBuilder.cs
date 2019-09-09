using WPF.GUI.Controls.NoteManagerControls;
using WPF.Models;
using WPF.Models.Enums;
using WPF.Models.Interfaces;

namespace WPF.Managers.Bilders
{
    public class NoteManagerControlBilder
    {
        public static IBaseNoteManagerControl Build(FileTypeModel type)
        {
            if(type != null)
            {
                switch (type.EnumType)
                {
                    case FileTypesEnum.LOTTO_NOTE:
                        return new LottoNoteControl(type);
                }
            }
            return new BaseNoteControl();
        }
    }
}
