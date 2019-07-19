using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WPF.Enums;
using WPF.Validators;

namespace WPF.Models
{
    [Table("ait_accounts")]
    public class AccountModel
    {
        private string id, login, password, email;

        [Key, Column("ait_id")]
        public string ID
        {
            get { return id; }
            set
            {
                if (AitAccountPropertiesValidator.ValidateID(value))
                    id = value;
            }
        }

        [Column("ait_login")]
        public string Login
        {
            get { return login; }
            set
            {
                if (AitAccountPropertiesValidator.ValidateLogin(value))
                    login = value;
            }
        }

        [Column("ait_password")]
        public string Password
        {
            get { return password; }
            set
            {
                if (AitAccountPropertiesValidator.ValidatePassword(value))
                    password = value;
            }
        }

        [Column("ait_email")]
        public string Email
        {
            get { return email; }
            set
            {
                if (AitAccountPropertiesValidator.ValidateEmail(value))
                    email = value;
            }
        }

        [Column("ait_active")]
        public bool Active { get; set; }
        [Column("ait_permition")]
        public PermitionAccount Permition { get; set; }
        [Column("ait_create")]
        public DateTime Create { get; set; }
        [Column("ait_lastupdate")]
        public DateTime? LastUpdate { get; set; }

        public AccountModel()
        {
            Active = false;
            Permition = PermitionAccount.SIMPLE;
            Create = DateTime.Now;
        }
    }
}
