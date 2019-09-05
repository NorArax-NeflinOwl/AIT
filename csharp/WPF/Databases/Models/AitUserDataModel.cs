using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using WPF.Models.Enums;
using WPF.Managers.Validators;
using WPF.Databases.Contexts;
using System.Linq;

namespace WPF.Databases.Models
{
    [Table("ait_usersdata")]
    public class AitUserDataModel : BaseEntityModel, ISerializable, ICloneable
    {
        private string assignedTo, nick, firstname, middlename, lastname;
        private DateTime? birthday;
        private AitAccountModel accountData;
        private AitHostDataModel hostData;

        [Key, Column("usd_id")]
        public string ID
        {
            get { return BaseID; }
            set
            {
                if (BasePropertiesValidator.ValidatePrimaryKey(value, TablePrefix))
                    SetField(ref BaseID, value, nameof(ID));
            }
        }

        [ForeignKey("AccountData"), Column("usd_accid")]
        public string AssignedTo
        {
            get { return assignedTo; }
            set
            {
                if (BasePropertiesValidator.ValidateID(value))
                    SetField(ref assignedTo, value, nameof(AssignedTo));
            }
        }
        [Column("usd_nick")]
        public string Nick
        {
            get { return nick; }
            set { SetField(ref nick, value, nameof(Nick)); }
        }

        [Column("usd_firstname")]
        public string FirstName
        {
            get { return firstname; }
            set { SetField(ref firstname, value, nameof(FirstName)); }
        }

        [Column("usd_middlename")]
        public string MiddleName
        {
            get { return middlename; }
            set { SetField(ref middlename, value, nameof(MiddleName)); }
        }

        [Column("usd_lastname")]
        public string LastName
        {
            get { return lastname; }
            set { SetField(ref lastname, value, nameof(LastName)); }
        }

        [Column("usd_birthday")]
        public DateTime? Birthday
        {
            get { return birthday; }
            set { SetField(ref birthday, value, nameof(Birthday)); }
        }

        [Column("usd_lastupdate")]
        public DateTime? LastUpdate
        {
            get { return BaseLastUpdate; }
            set { SetField(ref BaseLastUpdate, value, nameof(LastUpdate)); }
        }

        public AitAccountModel AccountData
        {
            get
            {
                if (accountData == null && !string.IsNullOrEmpty(AssignedTo) && Fill)
                {
                    accountData = Context.Accounts.Find(AssignedTo);
                }
                return accountData;
            }
            set { SetField(ref accountData, value, nameof(AccountData)); }
        }

        public AitHostDataModel HostData
        {
            get
            {
                if (hostData == null && !string.IsNullOrEmpty(AssignedTo) && Fill)
                {
                    hostData = Context.HostDatas.Where(q => string.IsNullOrEmpty(q.AssignedTo) && q.AssignedTo.Equals(assignedTo)).FirstOrDefault();
                }
                return hostData;
            }
            set { SetField(ref hostData, value, nameof(AccountData)); }
        }

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

        [NotMapped]
        public TableInerfixEnum TablePrefix { get { return TableInerfixEnum.USD; } }

        public AitUserDataModel(DBContext context) : base(context)
        { }

        public AitUserDataModel(SerializationInfo info, StreamingContext context) : base(null)
        {
            ID = (string)info.GetValue(nameof(ID), typeof(string));
            AssignedTo = (string)info.GetValue(nameof(AssignedTo), typeof(string));
            Nick = (string)info.GetValue(nameof(Nick), typeof(string));
            FirstName = (string)info.GetValue(nameof(FirstName), typeof(string));
            MiddleName = (string)info.GetValue(nameof(MiddleName), typeof(string));
            LastName = (string)info.GetValue(nameof(LastName), typeof(string));
            Birthday = (DateTime?)info.GetValue(nameof(Birthday), typeof(DateTime?));
            LastUpdate = (DateTime?)info.GetValue(nameof(LastUpdate), typeof(DateTime?));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(ID), ID, typeof(int));
            info.AddValue(nameof(AssignedTo), AssignedTo, typeof(string));
            info.AddValue(nameof(Nick), Nick, typeof(string));
            info.AddValue(nameof(FirstName), FirstName, typeof(string));
            info.AddValue(nameof(MiddleName), MiddleName, typeof(string));
            info.AddValue(nameof(LastName), LastName, typeof(string));
            info.AddValue(nameof(Birthday), Birthday, typeof(DateTime?));
            info.AddValue(nameof(LastUpdate), LastUpdate, typeof(DateTime?));
        }

        public object Clone()
        {
            var userData = new AitUserDataModel(PDBContext.Instance.Context)
            {
                ID = ID,
                AssignedTo = AssignedTo,
                Nick = Nick,
                FirstName = FirstName,
                MiddleName = MiddleName,
                LastName = LastName,
                Birthday = Birthday,
                LastUpdate = LastUpdate,
            };

            return userData;
        }
    }
}
