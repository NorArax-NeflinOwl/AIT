using AITLib.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace AITLib.Models
{
    public enum Status
    {
        NEW,
        PENDING,
        COMPLETED,
        CANCEL,
        FAILED,
        EXPIRED,
    }

    public class Mission : BaseObject
    {
        private uint id;
        private User parent;
        private string name;
        private DateTime createdDate;
        private DateTime? updatedDate;
        private Status status;

        public uint ID { get => id; }
        [Key, ForeignKey("User"), Column("msnParentID", Order = 0)]
        public uint ParentID { get => parent.ID; }
        [Column("usiName", Order = 1)]
        public string Name { get => name; }
        [Column("usiCreatedDate", Order = 2)]
        public DateTime CreatedDate { get => createdDate; }
        [Column("usiUpdatedDate", Order = 3)]
        public DateTime UpdatedDate { get => updatedDate ?? createdDate; }
        [Column("usiStatus", Order = 4)]
        public Status Status { get => status; }

        private Mission()
        {
        }

        public Mission(User parent)
        {
            this.parent = parent;
            createdDate = DateTime.Now;
            status = Status.NEW;
        }

        public Mission(SerializationInfo info, StreamingContext context)
        {
            id = (uint)info.GetValue("ID", typeof(uint));
            parent = (User)info.GetValue("Parent", typeof(User));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("ID", id, typeof(uint));
            info.AddValue("Parent", parent, typeof(User));
        }

        public object Clone()
        {
            var mission = new Mission();
            mission.id = id;
            mission.parent = parent;
            return mission;
        }
    }
}
