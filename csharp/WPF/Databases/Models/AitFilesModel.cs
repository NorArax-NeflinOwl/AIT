using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using WPF.ExtendedClasses;
using WPF.Validators;

namespace WPF.Databases.Models
{
    [Table("ait_files")]
    public class AitFilesModel : NotifyPropertyChangedExtension, ISerializable, ICloneable
    {
        private string id, creator, assignedTo;
        private AitAccountModel fileOwner;

        [Key, Column("fls_id")]
        public string ID
        {
            get { return id; }
            set
            {
                if (BasePropertiesValidator.ValidateID(value))
                    id = value;
            }
        }
        [ForeignKey("FileCreator"), Column("fls_accid")]
        public string Creator
        {
            get { return creator; }
            set
            {
                if (BasePropertiesValidator.ValidateID(value))
                    creator = value;
            }
        }
        [ForeignKey("FileOwner"), Column("fls_asgaccid")]
        public string AssignedTo
        {
            get
            {
                if (string.IsNullOrEmpty(assignedTo))
                    return Creator;
                return assignedTo;
            }
            set
            {
                if (BasePropertiesValidator.ValidateID(value))
                    assignedTo = value;
            }
        }
        [Column("fls_name")]
        public string Name { get; set; }
        [Column("fls_type")]
        public string Type { get; set; }
        [Column("fls_content")]
        public string Content { get; set; }
        [Column("fls_create")]
        public DateTime Create { get; set; }
        [Column("fls_lastupdate")]
        public DateTime? LastUpdate { get; set; }

        public AitAccountModel FileCreator { get; set; }
        public AitAccountModel FileOwner
        {
            get
            {
                if (fileOwner == null)
                    return FileCreator;
                return fileOwner;
            }
            set { fileOwner = value; }
        }

        public AitFilesModel() : base(null)
        {
        }

        public AitFilesModel(SerializationInfo info, StreamingContext context) : base(null)
        {
            ID = (string)info.GetValue(nameof(ID), typeof(string));
            Creator = (string)info.GetValue(nameof(Creator), typeof(string));
            AssignedTo = (string)info.GetValue(nameof(AssignedTo), typeof(string));
            Name = (string)info.GetValue(nameof(Name), typeof(string));
            Type = (string)info.GetValue(nameof(Type), typeof(string));
            Content = (string)info.GetValue(nameof(Content), typeof(string));
            Create = (DateTime)info.GetValue(nameof(Create), typeof(DateTime));
            LastUpdate = (DateTime?)info.GetValue(nameof(LastUpdate), typeof(DateTime?));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(ID), ID, typeof(int));
            info.AddValue(nameof(Creator), Creator, typeof(string));
            info.AddValue(nameof(AssignedTo), AssignedTo, typeof(string));
            info.AddValue(nameof(Name), Name, typeof(string));
            info.AddValue(nameof(Type), Type, typeof(string));
            info.AddValue(nameof(Content), Content, typeof(string));
            info.AddValue(nameof(Create), Create, typeof(DateTime));
            info.AddValue(nameof(LastUpdate), LastUpdate, typeof(DateTime?));
        }

        public object Clone()
        {
            var file = new AitFilesModel
            {
                ID = ID,
                Creator = Creator,
                AssignedTo = AssignedTo,
                Name = Name,
                Type = Type,
                Content = Content,
                Create = Create,
                LastUpdate = LastUpdate
            };
            return file;
        }

    }
}