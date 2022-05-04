using AITLib.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace AITLib.Models
{
    [Table("Comments")]
    public class Comment : BaseObject
    {
        private uint id;
        private Note parent;
        private string text;
        private DateTime createdDate;
        private bool isDeleted;

        [Key, Column("cmnID", Order = 0), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public uint ID { get => id; }

        [ForeignKey("Note"), Column("cmnParentID", Order = 1)]
        public uint ParentID { get => parent.ID; }

        [Column("cmnText", Order = 2)]
        public string Text { get => text; set => text = value; }

        [Column("cmnCreatedDate", Order = 3)]
        public DateTime CreatedDate { get => createdDate; set => createdDate = value; }

        public virtual bool IsDeleted { get => isDeleted; }

        private Comment()
        {
        }

        public Comment(Note parent)
        {
            this.parent = parent;
            createdDate = DateTime.Now;
            isDeleted = false;
        }

        public Comment(SerializationInfo info, StreamingContext context)
        {
            id = (uint)info.GetValue("ID", typeof(uint));
            parent = (Note)info.GetValue("Parent", typeof(Note));
            text = (string)info.GetValue("Text", typeof(string));
            createdDate = (DateTime)info.GetValue("CreatedDate", typeof(DateTime));
            isDeleted = (bool)info.GetValue("IsDeleted", typeof(bool));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("ID", id, typeof(uint));
            info.AddValue("Parent", parent, typeof(Note));
            info.AddValue("Text", text, typeof(string));
            info.AddValue("CreatedDate", createdDate, typeof(DateTime));
            info.AddValue("IsDeleted", isDeleted, typeof(bool));
        }

        public object Clone()
        {
            var comment = new Comment();
            comment.id = id;
            comment.parent = parent;
            comment.text = text;
            comment.createdDate = createdDate;
            comment.isDeleted = isDeleted;
            return comment;
        }

        public void SetID(uint id)
        {
            this.id = id;
        }
    }
}
