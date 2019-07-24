using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WPF.Validators;

namespace WPF.Databases.Models
{
    [Table("sys_stsgenids")]
    public class SysStsgenids : ICloneable
    {
        private string id;

        [Key, Column("sgi_id")]
        public string ID
        {
            get { return id; }
            set
            {
                if (BasePropertiesValidator.ValidateID(value))
                    id = value;
            }
        }
        [Column("sgi_create")]
        public DateTime Create { get; set; }
        [Column("sgi_delete")]
        public DateTime? Delete { get; set; }

        public SysStsgenids()
        {
            Create = DateTime.Now;
        }

        public object Clone()
        {
            var clone = new SysStsgenids
            {
                ID = ID,
                Create = Create,
                Delete = Delete
            };
            return clone;
        }
    }
}
