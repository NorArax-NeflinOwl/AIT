using AITLib.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace AITLib.Models
{
    public class User : BaseObject
    {
        private uint id;
        private string login;
        private string passwordHash;
        private DateTime createdDate;
        private DateTime? updatedDate;
        private UserInfo userInfo;
        private ICollection<Note> notes;
        private ICollection<Mission> missions;
        private Callendar callendar;

        [Key, Column("usrID", Order = 0), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public uint ID { get => id; }
        public virtual uint ParentID => throw new NotImplementedException();
        [Required, Column("usrLogin", Order = 1)]
        public string Login { get => login; }
        [Column("usrPasswordHash", Order = 2)]
        public string PasswordHash { get => passwordHash; }
        [Column("usrCreatedDate", Order = 3)]
        public DateTime CreatedDate { get => createdDate; }
        [Column("usrUpdatedDate", Order = 4)]
        public DateTime UpdatedDate { get => updatedDate ?? createdDate; }
        public virtual UserInfo UserInfo { get => userInfo; }
        public virtual ICollection<Note> Notes { get => notes; }
        public virtual ICollection<Mission> Missions { get => missions; }
        public virtual Callendar Callendar { get => callendar; }

        public User()
        {
            createdDate = DateTime.Now;
            userInfo = new UserInfo(this);
            notes = new List<Note>();
            missions = new List<Mission>();
            callendar = new Callendar(this);
        }

        public User(string login, string passwordHash) :base()
        {
            this.login = login;
            this.passwordHash = passwordHash;
        }

        public User(SerializationInfo info, StreamingContext context)
        {
            id = (uint)info.GetValue("ID", typeof(uint));
            login = (string)info.GetValue("Login", typeof(string));
            passwordHash = (string)info.GetValue("Password", typeof(string));
            createdDate = (DateTime)info.GetValue("CreatedDate", typeof (DateTime));
            updatedDate = (DateTime)info.GetValue("UpdatedDate", typeof(DateTime));
            userInfo = (UserInfo)info.GetValue("UserInfo", typeof(UserInfo));
            notes = (ICollection<Note>)info.GetValue("Notes", typeof(ICollection<Note>));
            missions = (ICollection<Mission>)info.GetValue("Missions", typeof(ICollection<Mission>));
            callendar = (Callendar)info.GetValue("Callendar", typeof(Callendar));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("ID", id, typeof(uint));
            info.AddValue("Login", login, typeof(string));
            info.AddValue("PasswordHash", passwordHash, typeof(string));
            info.AddValue("CreatedDate", createdDate, typeof(DateTime));
            info.AddValue("UpdatedDate", updatedDate, typeof(DateTime));
            info.AddValue("UserInfo", userInfo, typeof(UserInfo));
            info.AddValue("Notes", notes, typeof(ICollection<Note>));
            info.AddValue("Missions", missions, typeof(ICollection<Mission>));
            info.AddValue("Callendar", callendar, typeof(Callendar));
        }

        public object Clone()
        {
            var user = new User();
            user.id = id;
            user.login = login;
            user.passwordHash = passwordHash;
            user.createdDate = createdDate;
            user.updatedDate = updatedDate;
            user.userInfo = (UserInfo)userInfo.Clone();
            foreach (Note note in notes)
            {
                user.notes.Add((Note)note.Clone());
            }
            foreach (Mission mission in missions)
            {
                user.missions.Add((Mission)mission.Clone());
            }
            user.callendar = (Callendar)callendar.Clone();
            return user;
        }

        public void SetID(uint id)
        {
            this.id = id;
        }

        public void SetUserInfo(UserInfo userInfo)
        {
            this.userInfo = userInfo;
        }

        public void AddNote(Note note)
        {
            notes.Add(note);
        }

        public void AddCommentToNote(Note note, Comment comment)
        {
            note.AddComment(comment);
        }

        public void RemoveCommentFromNote(Note note, Comment comment)
        {
            note.RemoveComment(comment);
        }

        public void UpdateNote(Note note, string title, string description)
        {
            Note item = notes.Where(q => q.ID == note.ID).FirstOrDefault();
            item.Update(title, description);
        }

        public void UpdateNote(Note note, string title, string description, uint? commentID, Comment newComment)
        {
            Note item = notes.Where(q => q.ID == note.ID).FirstOrDefault();
            item.Update(title, description, commentID, newComment);
        }

        public void RemoveNote(Note note)
        {
            notes.Remove(note);
        }
    }
}
