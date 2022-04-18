using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace AIT.DataBases.DBModel
{
    [Table("AitQuickNote")]
    public class AitQuickNote : ISerializable, IEnumerable, ICloneable
    {
        [Key, Column("qknID", Order = 0), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Key, ForeignKey("Person"), Column("qknPersonID", Order = 1)]
        public int PersonID { get; set; }
        [Required, Column("qknTitle", Order = 2)]
        public string Title { get; set; }
        [Required, Column("qknNote", Order = 3)]
        public string Note { get; set; }
        [Required, Column("qknCDate", Order = 4)]
        public DateTime CDate { get; set; }
        [Column("qknPassword", Order = 5)]
        public string Password { get; set; }
        public virtual AitPerson Person { get; set; }
        [NotMapped]
        public bool IsCrypted { get { return !string.IsNullOrEmpty(Password); } }

        private List<AitQuickNote> m_QuickNotes;

        public AitQuickNote() { }

        public AitQuickNote(AitQuickNote[] pArray)
        {
            if (pArray == null)
                m_QuickNotes = new List<AitQuickNote>();

            for (int i = 0; i < pArray.Length; i++)
            {
                m_QuickNotes[i] = pArray[i];
            }
        }

        public AitQuickNote(SerializationInfo info, StreamingContext context)
        {
            ID = (int)info.GetValue("ID", typeof(int));
            PersonID = (int)info.GetValue("PersonID", typeof(int));
            Title = (string)info.GetValue("Title", typeof(string));
            Note = (string)info.GetValue("Note", typeof(string));
            CDate = (DateTime)info.GetValue("CDate", typeof(DateTime));
            Password = (string)info.GetValue("Password", typeof(string));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("ID", ID, typeof(int));
            info.AddValue("PersonID", PersonID, typeof(int));
            info.AddValue("Title", Title, typeof(string));
            info.AddValue("Note", Note, typeof(string));
            info.AddValue("CDate", CDate, typeof(DateTime));
            info.AddValue("Password", Password, typeof(string));
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)GetEnumerator();
        }

        public AitQuickNoteEnum GetEnumerator()
        {
            return new AitQuickNoteEnum(m_QuickNotes?.ToArray());
        }

        public object Clone()
        {
            var note = new AitQuickNote();
            note.ID = ID;
            note.PersonID = PersonID;
            note.Title = Title;
            note.Note = Note;
            note.Password = Password;
            note.CDate = CDate;
            note.Person = Person;
            return note;
        }
    }

    public class AitQuickNoteEnum : IEnumerator
    {
        public AitQuickNote[] m_QuickNotes;

        // Enumerators are positioned before the first element
        // until the first MoveNext() call.
        int position = -1;

        public AitQuickNoteEnum(AitQuickNote[] list)
        {
            m_QuickNotes = list;
        }

        public bool MoveNext()
        {
            position++;
            return (m_QuickNotes != null && position < m_QuickNotes.Length);
        }

        public void Reset()
        {
            position = -1;
        }

        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }

        public AitQuickNote Current
        {
            get
            {
                try
                {
                    return m_QuickNotes[position];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }
    }
}
