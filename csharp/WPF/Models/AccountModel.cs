using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
                if (AccountPropertiesValidator.ValidateID(value))
                    id = value;
            }
        }

        [Column("ait_login")]
        public string Login
        {
            get { return login; }
            set
            {
                if (AccountPropertiesValidator.ValidateLogin(value))
                    login = value;
            }
        }

        [Column("ait_password")]
        public string Password
        {
            get { return password; }
            set
            {
                if (AccountPropertiesValidator.ValidatePassword(value))
                    password = value;
            }
        }

        [Column("ait_email")]
        public string Email
        {
            get { return email; }
            set
            {
                if (AccountPropertiesValidator.ValidateEmail(value))
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

    public enum PermitionAccount
    {
        NONE = -1,
        BLOCK = 0,
        ADMIN = 1,
        SIMPLE = 2,
        MANAGER = 3
    }

    class AccountPropertiesValidator
    {
        private static readonly char idSeperator = '-';

        public static bool ValidateID(string value)
        {
            if(string.IsNullOrEmpty(value))
            {
                throw new AccountExceptions.IDException("ID is required");
            }

            if(value.Length != 15)
            {
                throw new AccountExceptions.IDException("ID length must equals 15");
            }

            var idParts = value.Split(idSeperator);
            if (idParts.Length != 3)
            {
                throw new AccountExceptions.IDException("ID don't containt 2 separators");
            }

            if(!int.TryParse(idParts[2], out _))
            {
                throw new AccountExceptions.IDException("ID postfix is not number");
            }

            return true;
        }

        public static bool ValidateLogin(string value)
        {
            if(string.IsNullOrEmpty(value))
            {
                throw new AccountExceptions.LoginException("Login is required");
            }

            if (value.Length < 4)
            {
                throw new AccountExceptions.LoginException("Password is too short");
            }

            return true;
        }

        public static bool ValidatePassword(string value)
        {
            if(string.IsNullOrEmpty(value))
            {
                throw new AccountExceptions.PasswordException("Password is required");
            }

            if (value.Length < 8)
            {
                throw new AccountExceptions.PasswordException("Password is too weak");
            }

            return true;
        }

        public static bool ValidateEmail(string value)
        {
            if(string.IsNullOrEmpty(value))
            {
                throw new AccountExceptions.EmailException("Email is required");
            }

            var addr = new System.Net.Mail.MailAddress(value);
            if(addr.Address != value)
            {
                throw new AccountExceptions.EmailException("Incorect Email");
            }

            return true;
        }
    }

    class AccountExceptions
    {
        public class IDException : Exception
        {
            public IDException(string message) : base(message)
            {
            }
        }

        public class LoginException : Exception
        {
            public LoginException(string message) : base(message)
            {
            }
        }

        public class PasswordException : Exception
        {
            public PasswordException(string message) : base(message)
            {
            }
        }

        public class EmailException : Exception
        {
            public EmailException(string message) : base(message)
            {
            }
        }
    }
}
