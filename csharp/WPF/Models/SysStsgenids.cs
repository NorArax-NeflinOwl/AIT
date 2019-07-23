using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WPF.Models
{
    [Table("sys_stsgenids")]
    public class SysStsgenids
    {
        [Key, Column("sgi_id")]
        public string ID { get; set; }
        [Column("sgi_create")]
        public DateTime Create { get; set; }
        [Column("sgi_delete")]
        public DateTime? Delete { get; set; }

        public SysStsgenids()
        {
            Create = DateTime.Now;
        }
    }
}
