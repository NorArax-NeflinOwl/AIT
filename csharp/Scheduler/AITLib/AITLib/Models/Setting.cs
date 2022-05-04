using AITLib.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace AITLib.Models
{
    [Table("Settings")]
    public class Setting : BaseObject
    {
        private uint id;
        private User parent;

        [Key, Column("setID", Order = 0), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public uint ID { get => id; set => id = value; }

        [ForeignKey("User"), Column("setParentID", Order = 1)]
        public uint ParentID { get => parent.ID;}

        public IEnumerable<Mission> Missions { get => parent.Missions; }

        private Setting()
        {
        }

        public Setting(User parent)
        {
            this.parent = parent;
        }

        public Setting(SerializationInfo info, StreamingContext context)
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
            var callendar = new Setting();
            callendar.id = id;
            callendar.parent = parent;
            return callendar;
        }
    }
}
