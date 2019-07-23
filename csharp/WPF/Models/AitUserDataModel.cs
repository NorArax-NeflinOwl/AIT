using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WPF.Validators;

namespace WPF.Models
{
    [Table("ait_usersdata")]
    public class AitUserDataModel
    {
        private string id, assignedTo;

        [Key, Column("usd_id")]
        public string ID
        {
            get { return id; }
            set
            {
                if (BasePropertiesValidator.ValidateID(value))
                    id = value;
            }
        }
        [ForeignKey("AccountData"), Column("usd_accid")]
        public string AssignedTo
        {
            get { return assignedTo; }
            set
            {
                if (BasePropertiesValidator.ValidateID(value))
                    assignedTo = value;
            }
        }
        [Column("usd_nick")]
        public string Nick { get; set; }
        [Column("usd_firstname")]
        public string FirstName { get; set; }
        [Column("usd_middlename")]
        public string MiddleName { get; set; }
        [Column("usd_lastname")]
        public string LastName { get; set; }
        [Column("usd_birthday")]
        public DateTime Birthday { get; set; }
        [Column("usd_lastupdate")]
        public DateTime? LastUpdate { get; set; } 

        [NotMapped]
        public string FullName
        {
            get
            {
                string fullname = string.Empty;

                if (!string.IsNullOrEmpty(FirstName))
                    fullname += FirstName + " ";
                if (!string.IsNullOrEmpty(MiddleName))
                    fullname += MiddleName + " ";
                if (!string.IsNullOrEmpty(LastName))
                    fullname += LastName;

                return fullname;
            }
        }

        public AitAccountModel AccountData { get; set; }
    }
}
