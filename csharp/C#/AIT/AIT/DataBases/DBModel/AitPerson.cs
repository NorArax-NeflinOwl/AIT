using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace AIT.DataBases.DBModel
{
    [Table("AitPersons")]
    public class AitPerson : ISerializable, ICloneable
    {
        [Key, Column("prnID", Order = 0), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Required, Column("prnLogin", Order = 1)]
        public string Login { get; set; }
        [Column("prnEmail", Order = 2)]
        public string Email { get; set; }
        [Required, Column("prnPassword", Order = 3)]
        public string Password { get; set; }

        public virtual AitPersonsDetail PersonalDetails { get; set; }
        public virtual ICollection<AitQuickNote> QuickNotes { get; set; }

        public AitPerson()
        {
        }

        public AitPerson(SerializationInfo info, StreamingContext context)
        {
            ID = (int)info.GetValue("ID", typeof(int));
            Login = (string)info.GetValue("Login", typeof(string));
            Email = (string)info.GetValue("Email", typeof(string));
            Password = (string)info.GetValue("Password", typeof(string));
            PersonalDetails = (AitPersonsDetail)info.GetValue("PersonalDetails", typeof(AitPersonsDetail));
            QuickNotes = (ICollection<AitQuickNote>)info.GetValue("QuickNotes", typeof(ICollection<AitQuickNote>));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("ID", ID, typeof(int));
            info.AddValue("Login", Login, typeof(string));
            info.AddValue("Email", Email, typeof(string));
            info.AddValue("Password", Password, typeof(string));
            info.AddValue("PersonalDetails", PersonalDetails, typeof(AitPersonsDetail));
            info.AddValue("QuickNotes", QuickNotes, typeof(ICollection<AitQuickNote>));
        }

        public object Clone()
        {
            var person = new AitPerson();
            person.ID = ID;
            person.Login = Login;
            person.Email = Email;
            person.Password = Password;
            person.QuickNotes = new List<AitQuickNote>();
            person.PersonalDetails = PersonalDetails;
            foreach (AitQuickNote note in person.QuickNotes)
            {
                person.QuickNotes.Add(note);
            }
            return person;
        }
    }
}
