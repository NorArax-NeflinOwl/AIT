using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using WPF.Databases.Contexts;
using WPF.Models.Enums;
using WPF.Managers.Validators;
using System.Collections.Generic;
using System.Linq;
using WPF.Managers.Helpers;

namespace WPF.Databases.Models
{
    [Table("ait_accounts")]
    public class AitAccountModel : BaseEntityModel, ISerializable, ICloneable
    {
        private string login, password, email;
        private bool isActive;
        private PermitionAccountEnum permition;
        private DateTime create;
        private AitUserDataModel userData;
        private IList<AitFileModel> files;
        private IList<AitUserHostModel> userHosts;

        [Key, Column("acc_id")]
        public string ID
        {
            get { return BaseID; }
            set
            {
                if (BasePropertiesValidator.ValidatePrimaryKey(value, TablePrefix))
                    SetField(ref BaseID, value, nameof(ID));
            }
        }

        [Column("acc_login")]
        public string Login
        {
            get { return login; }
            set
            {
                if (AitAccountPropertiesValidator.ValidateLogin(value))
                    SetField(ref login, value, nameof(Login));
            }
        }

        [Column("acc_password")]
        public string Password
        {
            get { return password; }
            set
            {
                if (AitAccountPropertiesValidator.ValidatePassword(value))
                    SetField(ref password, value, nameof(Password));
            }
        }

        [Column("acc_email")]
        public string Email
        {
            get { return email; }
            set
            {
                if (AitAccountPropertiesValidator.ValidateEmail(value))
                    SetField(ref email, value, nameof(Email));
            }
        }

        [Column("acc_active")]
        public bool IsActive
        {
            get { return isActive; }
            set { SetField(ref isActive, value, nameof(IsActive)); }
        }

        [Column("acc_permition")]
        public PermitionAccountEnum Permition
        {
            get { return permition; }
            set { SetField(ref permition, value, nameof(Permition)); }
        }

        [Column("acc_create"), StringLength(10)]
        public DateTime Create
        {
            get { return create; }
            set { SetField(ref create, value, nameof(Create)); }
        }

        [Column("acc_lastupdate"), StringLength(10)]
        public DateTime? LastUpdate
        {
            get { return BaseLastUpdate; }
            set { SetField(ref BaseLastUpdate, value, nameof(LastUpdate)); }
        }

        [NotMapped]
        public TableInerfixEnum TablePrefix { get { return TableInerfixEnum.ACC; } }

        [NotMapped]
        public AitUserDataModel UserData
        {
            get
            {
                if (userData == null && Fill)
                {
                    userData = Context.UsersDatas.Where(q => ID.Equals(q.AssignedTo)).FirstOrDefault();
                }
                return userData;
            }
            set { SetField(ref userData, value, nameof(UserData)); }
        }

        [NotMapped]
        public IList<AitFileModel> Files
        {
            get
            {
                if (files == null && Fill)
                {
                    if (Permition.Equals(PermitionAccountEnum.ADMIN) || Permition.Equals(PermitionAccountEnum.MANAGER))
                    {
                        files = Context.Files.ToList();
                    }
                    else
                    {
                        files = Context.Files.Where(q => ID.Equals(q.Creator) || ID.Equals(q.AssignedTo)).ToList();
                    }
                }
                return files;
            }
            set { SetField(ref files, value, nameof(Files)); }
        }

        [NotMapped]
        public IList<AitUserHostModel> UserHosts
        {
            get
            {
                if (userHosts == null && Fill)
                {
                    userHosts = Context.UsersHosts.Where(q => ID.Equals(q.AssignedTo)).ToList();
                }
                return userHosts;
            }
            set { SetField(ref userHosts, value, nameof(UserHosts)); }
        }

        public AitAccountModel(DBContext context) : base(context)
        {
            ID = Generators.RecordIDGenerator(TablePrefix);
            IsActive = false;
            Permition = PermitionAccountEnum.SIMPLE;
            Create = DateTime.Now;
        }

        public AitAccountModel(SerializationInfo info, StreamingContext context) : base(null)
        {
            ID = (string)info.GetValue(nameof(ID), typeof(string));
            Login = (string)info.GetValue(nameof(Login), typeof(string));
            Password = (string)info.GetValue(nameof(Password), typeof(string));
            Email = (string)info.GetValue(nameof(Email), typeof(string));
            IsActive = (bool)info.GetValue(nameof(IsActive), typeof(bool));
            Permition = (PermitionAccountEnum)info.GetValue(nameof(Permition), typeof(PermitionAccountEnum));
            Create = (DateTime)info.GetValue(nameof(Create), typeof(DateTime));
            LastUpdate = (DateTime?)info.GetValue(nameof(LastUpdate), typeof(DateTime?));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(ID), ID, typeof(int));
            info.AddValue(nameof(Login), Login, typeof(string));
            info.AddValue(nameof(Password), Password, typeof(string));
            info.AddValue(nameof(Email), Email, typeof(string));
            info.AddValue(nameof(IsActive), IsActive, typeof(bool));
            info.AddValue(nameof(Permition), Permition, typeof(PermitionAccountEnum));
            info.AddValue(nameof(Create), Create, typeof(DateTime));
            info.AddValue(nameof(LastUpdate), LastUpdate, typeof(DateTime?));
        }

        public object Clone()
        {
            var file = new AitAccountModel(PDBContext.Instance.Context)
            {
                ID = ID,
                Login = Login,
                Password = Password,
                Email = Email,
                IsActive = IsActive,
                Permition = Permition,
                Create = Create,
                LastUpdate = LastUpdate
            };
            return file;
        }
    }
}
