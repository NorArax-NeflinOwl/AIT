using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using AIT.Constant;
using AIT.Converters;

namespace AIT.Models
{
    public class aitExtSimpleNoteModel : aitExtDataBaseModel, ICloneable, ISerializable
    {
        #region Properties

        private aitNotesTypesModel m_NoteType = aitNotesTypesModel.SIMPLE_NOTE_MODEL;
        public aitNotesTypesModel NoteType
        {
            get
            {
                return m_NoteType;
            }
        }

        private aitDateTimeType m_DateTimeType = aitDateTimeType.DATE;
        public aitDateTimeType DateTimeType
        {
            get
            {
                return m_DateTimeType;
            }
        }

        private DateTime? m_Date = null;
        public DateTime? Date
        {
            get
            {
                return m_Date;
            }
        }

        private string m_Title;
        public string Title
        {
            get
            {
                return m_Title;
            }
        }

        private string m_Note;
        public string Note
        {
            get
            {
                return m_Note;
            }
        }

        #endregion
        #region Constructor

        public aitExtSimpleNoteModel() : base()
        {
            SetTabName(aitTabsNames.aitTBCalNote);
        }

        public aitExtSimpleNoteModel(
            string title, 
            string note, 
            DateTime? noteDate, 
            aitDateTimeType dateType = aitDateTimeType.DATE,
            aitNotesTypesModel noteType = aitNotesTypesModel.SIMPLE_NOTE_MODEL) : base()
        {
            m_NoteType = noteType;
            m_DateTimeType = dateType;
            m_Date = noteDate;
            m_Title = title;
            m_Note = note;
            SetTabName(aitTabsNames.aitTBCalNote);
        }

        public aitExtSimpleNoteModel(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            m_NoteType = (aitNotesTypesModel)info.GetValue("NoteType", typeof(aitNotesTypesModel));
            m_DateTimeType = (aitDateTimeType)info.GetValue("DateTimeType", typeof(aitDateTimeType));
            m_Date = (DateTime?)info.GetValue("Date", typeof(DateTime?));
            m_Title = (string)info.GetValue("Title", typeof(string));
            m_Note = (string)info.GetValue("Note", typeof(string));
        }

        #endregion
        #region Implementation interface methods

        public object Clone()
        {
            base.Clone();
            var newObj = new aitExtSimpleNoteModel(m_Title, m_Note, m_Date, m_DateTimeType, m_NoteType);
            newObj.SetID(ID, true);
            newObj.SetTabName(TabName);
            newObj.m_CreateDate = CreateDate;
            return newObj;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("NoteType", m_NoteType, typeof(aitNotesTypesModel));
            info.AddValue("DateTimeType", m_DateTimeType, typeof(aitDateTimeType));
            info.AddValue("Date", m_Date, typeof(DateTime?));
            info.AddValue("Title", m_Title, typeof(string));
            info.AddValue("Note", m_Note, typeof(string));
        }

        #endregion
        #region Override

        public override string ToString()
        {
            return "[T]:" + aitType2StringConverter.NoteType2StringConverter(m_NoteType) + ";[D]" + aitType2StringConverter.DateType2StringConverter(m_DateTimeType) + " - " + m_Title;
        }

        #endregion
    }
}
