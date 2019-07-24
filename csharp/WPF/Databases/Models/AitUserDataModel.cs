using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using WPF.ExtendedClasses;
using WPF.Validators;

namespace WPF.Databases.Models
{
    [Table("ait_usersdata")]
    public class AitUserDataModel : NotifyPropertyChangedExtension, ISerializable, ICloneable
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
        public DateTime? Birthday { get; set; }

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

        public AitUserDataModel() : base(null)
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
            var userData = new AitUserDataModel
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
