using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using WPF.Databases.Contexts;
using WPF.Models.Enums;
using WPF.Managers.Validators;

namespace WPF.Databases.Models
{
    [Table("ait_usershosts")]
    public class AitUserHostModel : BaseEntityModel, ISerializable, ICloneable
    {
        private string assignedTo;

        [Key, Column("ush_id")]
        public string ID
        {
            get { return BaseID; }
            set
            {
                if (BasePropertiesValidator.ValidatePrimaryKey(value, TablePrefix))
                    SetField(ref BaseID, value, nameof(ID));
            }
        }
        [ForeignKey("AccountData"), Column("ush_accid")]
        public string AssignedTo
        {
            get { return assignedTo; }
            set
            {
                if (BasePropertiesValidator.ValidateID(value))
                    SetField(ref assignedTo, value, nameof(AssignedTo));
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
        public DateTime? LastUpdate
        {
            get { return BaseLastUpdate; }
            set { SetField(ref BaseLastUpdate, value, nameof(LastUpdate)); }
        }

        [NotMapped]
        public IDInerfixEnum TablePrefix { get { return IDInerfixEnum.USH; } }

        public AitAccountModel AccountData { get; set; }

        public AitUserHostModel(DBContext context) : base(context)
        {
            Create = DateTime.Now;
        }

        public AitUserHostModel(SerializationInfo info, StreamingContext context) : base(null)
        {
            ID = (string)info.GetValue(nameof(ID), typeof(string));
            AssignedTo = (string)info.GetValue(nameof(AssignedTo), typeof(string));
            HostName = (string)info.GetValue(nameof(HostName), typeof(string));
            IsActive = (bool)info.GetValue(nameof(IsActive), typeof(bool));
            IsLoggedIn = (bool)info.GetValue(nameof(IsLoggedIn), typeof(bool));
            Create = (DateTime)info.GetValue(nameof(Create), typeof(DateTime));
            LastUpdate = (DateTime?)info.GetValue(nameof(LastUpdate), typeof(DateTime?));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(ID), ID, typeof(int));
            info.AddValue(nameof(AssignedTo), AssignedTo, typeof(string));
            info.AddValue(nameof(HostName), HostName, typeof(string));
            info.AddValue(nameof(IsActive), IsActive, typeof(bool));
            info.AddValue(nameof(IsLoggedIn), IsLoggedIn, typeof(bool));
            info.AddValue(nameof(Create), Create, typeof(DateTime));
            info.AddValue(nameof(LastUpdate), LastUpdate, typeof(DateTime?));
        }

        public object Clone()
        {
            var clone = new AitUserHostModel(Context)
            {
                ID = ID,
                AssignedTo = AssignedTo,
                HostName = HostName,
                IsActive = IsActive,
                IsLoggedIn = IsLoggedIn,
                Create = Create,
                LastUpdate = LastUpdate
            };
            return clone;
        }
    }
}
