using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WPF.Validators;

namespace WPF.Models
{
    [Table("ait_usershosts")]
    public class AitUserHostModel
    {
        private string id, assignedTo;

        [Key, Column("ush_id")]
        public string ID
        {
            get { return id; }
            set
            {
                if (BasePropertiesValidator.ValidateID(value))
                    id = value;
            }
        }
        [ForeignKey("AccountData"), Column("ush_accid")]
        public string AssignedTo
        {
            get { return assignedTo; }
            set
            {
                if (BasePropertiesValidator.ValidateID(value))
                    assignedTo = value;
            }
        }
        [Column("ush_hostname")]
        public string HostName { get; set; }
        [Column("ush_active")]
        public bool IsActive { get; set; }
        [Column("ush_loggedin")]
        public bool IsLoggedIn { get; set; }
        [Column("ush_create")]
        public DateTime Create { get; set; }
        [Column("ush_lastupdate")]
        public DateTime? LastUpdate { get; set; }

        public AitAccountModel AccountData { get; set; }

        public AitUserHostModel()
        {
            IsActive = false;
            IsLoggedIn = false;
            Create = DateTime.Now;
        }
    }
}
