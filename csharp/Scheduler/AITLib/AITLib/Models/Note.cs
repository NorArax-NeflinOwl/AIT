using AITLib.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace AITLib.Models
{
    [Table("Notes")]
    public class Note : BaseObject
    {
        private uint id;
        private User parent;
        private string title;
        private string description;
        private DateTime createdDate;
        private DateTime? updatedDate;
        private ICollection<Comment> comments;

        [Key, Column("nteID", Order = 0), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public uint ID { get => id; }

        [ForeignKey("Note"), Column("nteParentID", Order = 1)]
        public uint ParentID { get => parent.ID; }

        [Column("nteTitle", Order = 2)]
        public string Title { get => title; }

        [Column("nteDesc", Order = 3)]
        public string Description { get => description; }

        [Column("nteCreateDate", Order = 4)]
        public DateTime CreatedDate { get => createdDate; }

        [Column("nteUpdateDate", Order = 5)]
        public DateTime UpdatedDate { get => updatedDate ?? CreatedDate; }
        public virtual ICollection<Comment> Comments { get => comments; }

        private Note()
        {
        }

        public Note(User parent)
        {
            this.parent = parent;
            createdDate = DateTime.Now;
            comments = new List<Comment>();
        }

        public Note(User user, string title, string description) :this(user)
        {
            this.title = title;
            this.description = description;
        }

        public Note(SerializationInfo info, StreamingContext context)
        {
            id = (uint)info.GetValue("ID", typeof(uint));
            parent = (User)info.GetValue("Parent", typeof(User));
            title = (string)info.GetValue("Title", typeof(string));
            description = (string)info.GetValue("Description", typeof(string));
            createdDate = (DateTime)info.GetValue("CreatedDate", typeof(DateTime));
            updatedDate = (DateTime?)info.GetValue("UpdatedDate", typeof(DateTime?));
            comments = (ICollection<Comment>)info.GetValue("Comments", typeof(ICollection<Comment>));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("ID", id, typeof(uint));
            info.AddValue("Parent", parent, typeof(User));
            info.AddValue("Title", title, typeof(string));
            info.AddValue("Description", description, typeof(string));
            info.AddValue("CreatedDate", createdDate, typeof(DateTime));
            info.AddValue("UpdatedDate", updatedDate, typeof(DateTime?));
            info.AddValue("Comments", comments, typeof(ICollection<Comment>));
        }

        public object Clone()
        {
            var note = new Note();
            note.id = id;
            note.parent = parent;
            note.title = title;
            note.description = description;
            note.createdDate = createdDate;
            note.updatedDate = updatedDate;
            foreach(Comment comment in comments)
            {
                note.comments.Add((Comment)comment.Clone());
            }
            return note;
        }

        public void UpdateComment(uint commentID, Comment newComment)
        {
            Comment item = comments.Where(c => c.ID == commentID).FirstOrDefault();
            item = newComment;
            item.SetID(commentID);
        }

        public void Update(string title, string description)
        {
            this.title = title;
            this.description = description;
            updatedDate = DateTime.Now;
        }

        public void Update(string title, string description, uint? commentID, Comment newComment)
        {
            if(null != commentID)
            {
                UpdateComment((uint)commentID, newComment);
            }
            Update(title, description);
        }

        public void AddComment(Comment comment)
        {
            comments.Add(comment);
            updatedDate = DateTime.Now;
        }

        public void RemoveComment(Comment comment)
        {
            updatedDate = DateTime.Now;
            comments.Remove(comment);
        }
    }
}
