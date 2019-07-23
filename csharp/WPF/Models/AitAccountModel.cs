using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WPF.Enums;
using WPF.Validators;

namespace WPF.Models
{
    [Table("ait_accounts")]
    public class AitAccountModel
    {
        private string id, login, password, email;

        [Key, Column("acc_id")]
        public string ID
        {
            get { return id; }
            set
            {
                if (BasePropertiesValidator.ValidateID(value))
                    id = value;
            }
        }

        [Column("acc_login")]
        public string Login
        {
            get { return login; }
            set
            {
                if (AitAccountPropertiesValidator.ValidateLogin(value))
                    login = value;
            }
        }

        [Column("acc_password")]
        public string Password
        {
            get { return password; }
            set
            {
                if (AitAccountPropertiesValidator.ValidatePassword(value))
                    password = value;
            }
        }

        [Column("acc_email")]
        public string Email
        {
            get { return email; }
            set
            {
                if (AitAccountPropertiesValidator.ValidateEmail(value))
                    email = value;
            }
        }

        [Column("acc_active")]
        public bool IsActive { get; set; }
        [Column("acc_permition")]
        public PermitionAccount Permition { get; set; }
        [Column("acc_create")]
        public DateTime Create { get; set; }
        [Column("acc_lastupdate")]
        public DateTime? LastUpdate { get; set; }

        public AitAccountModel()
        {
            IsActive = false;
            Permition = PermitionAccount.SIMPLE;
            Create = DateTime.Now;
        }
    }
}
