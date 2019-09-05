using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using WPF.Databases.Contexts;
using WPF.Models.Enums;
using WPF.Managers.Validators;

namespace WPF.Databases.Models
{
    [Table("ait_hostsdata")]
    public class AitHostDataModel : BaseEntityModel, ISerializable, ICloneable
    {
        private string assignedTo;
        private AitUserHostModel userHost;

        [Key, Column("hsd_id")]
        public string ID
        {
            get { return BaseID; }
            set
            {
                if (BasePropertiesValidator.ValidatePrimaryKey(value, TablePrefix))
                    SetField(ref BaseID, value, nameof(ID));
            }
        }

        [ForeignKey("UserHost"), Column("hsd_ushid")]
        public string AssignedTo
        {
            get { return assignedTo; }
            set
            {
                if (BasePropertiesValidator.ValidateID(value))
                    SetField(ref assignedTo, value, nameof(AssignedTo));
            }
        }

        [Column("hsd_name")]
        public string ComputerName { get; set; }

        [Column("hsd_language")]
        public string CurrentLanguage { get; set; }

        [Column("hsd_osi")]
        public string OSInfo { get; set; }

        [Column("hsd_processor")]
        public string ProcessorInfo { get; set; }

        [Column("hsd_memory")]
        public string PhysicalMemory { get; set; }

        [Column("hsd_create")]
        public DateTime Create { get; set; }

        [Column("hsd_lastupdate")]
        public DateTime? LastUpdate
        {
            get { return BaseLastUpdate; }
            set { SetField(ref BaseLastUpdate, value, nameof(LastUpdate)); }
        }

        public AitUserHostModel UserHost
        {
            get
            {
                if (userHost == null && !string.IsNullOrEmpty(AssignedTo) && Fill)
                {
                    userHost = Context.UsersHosts.Find(AssignedTo);
                }
                return userHost;
            }
            set { SetField(ref userHost, value, nameof(userHost)); }
        }

        [NotMapped]
        public TableInerfixEnum TablePrefix { get { return TableInerfixEnum.HSD; } }

        public AitHostDataModel(DBContext context) : base(context)
        {
            Create = DateTime.Now;
        }

        public AitHostDataModel(SerializationInfo info, StreamingContext context) : base(null)
        {
            ID = (string)info.GetValue(nameof(ID), typeof(string));
            AssignedTo = (string)info.GetValue(nameof(AssignedTo), typeof(string));
            ComputerName = (string)info.GetValue(nameof(ComputerName), typeof(string));
            CurrentLanguage = (string)info.GetValue(nameof(CurrentLanguage), typeof(string));
            OSInfo = (string)info.GetValue(nameof(OSInfo), typeof(string));
            ProcessorInfo = (string)info.GetValue(nameof(ProcessorInfo), typeof(string));
            PhysicalMemory = (string)info.GetValue(nameof(PhysicalMemory), typeof(string));
            Create = (DateTime)info.GetValue(nameof(Create), typeof(DateTime));
            LastUpdate = (DateTime?)info.GetValue(nameof(LastUpdate), typeof(DateTime?));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(ID), ID, typeof(int));
            info.AddValue(nameof(AssignedTo), AssignedTo, typeof(string));
            info.AddValue(nameof(ComputerName), ComputerName, typeof(string));
            info.AddValue(nameof(CurrentLanguage), CurrentLanguage, typeof(string));
            info.AddValue(nameof(OSInfo), OSInfo, typeof(string));
            info.AddValue(nameof(ProcessorInfo), ProcessorInfo, typeof(string));
            info.AddValue(nameof(PhysicalMemory), PhysicalMemory, typeof(string));
            info.AddValue(nameof(Create), Create, typeof(DateTime));
            info.AddValue(nameof(LastUpdate), LastUpdate, typeof(DateTime?));
        }

        public object Clone()
        {
            var clone = new AitHostDataModel(PDBContext.Instance.Context)
            {
                ID = ID,
                AssignedTo = AssignedTo,
                ComputerName = ComputerName,
                CurrentLanguage = CurrentLanguage,
                OSInfo = OSInfo,
                ProcessorInfo = ProcessorInfo,
                PhysicalMemory = PhysicalMemory,
                Create = Create,
                LastUpdate = LastUpdate
            };
            return clone;
        }
    }
}
