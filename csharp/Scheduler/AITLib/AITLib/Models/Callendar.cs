using AITLib.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace AITLib.Models
{
    public class Callendar : BaseObject
    {
        private uint id;
        private User parent;

        public uint ID { get => id; }
        [Key, ForeignKey("User"), Column("cldParentID", Order = 0)]
        public uint ParentID { get => parent.ID; }
        public IEnumerable<Mission> Missions { get => parent.Missions; }

        private Callendar()
        {
        }

        public Callendar(User parent)
        {
            this.parent = parent;
        }

        public Callendar(SerializationInfo info, StreamingContext context)
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
            var callendar = new Callendar();
            callendar.id = id;
            callendar.parent = parent;
            return callendar;
        }

    }
}
