using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using WPF.Databases.Contexts;
using WPF.ExtendedClasses;
using WPF.Validators;

namespace WPF.Databases.Models
{
    [Table("ait_files")]
    public class AitFilesModel : BaseEntityModel, ISerializable, ICloneable
    {
        private string creator, assignedTo, name, type, content;
        private AitAccountModel fileOwner;
        private DateTime create;

        [Key, Column("fls_id")]
        public string ID
        {
            get { return BaseID; }
            set
            {
                if (BasePropertiesValidator.ValidateID(value))
                    SetField(ref BaseID, value, nameof(ID));
            }
        }
        [ForeignKey("FileCreator"), Column("fls_accid")]
        public string Creator
        {
            get { return creator; }
            set
            {
                if (BasePropertiesValidator.ValidateID(value))
                    SetField(ref creator, value, nameof(Creator));
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
                    SetField(ref assignedTo, value, nameof(AssignedTo));
            }
        }
        [Column("fls_name")]
        public string Name
        {
            get { return name; }
            set { SetField(ref name, value, nameof(Name)); }
        }
        [Column("fls_type")]
        public string Type
        {
            get { return type; }
            set { SetField(ref type, value, nameof(Type)); }
        }
        [Column("fls_content")]
        public string Content
        {
            get { return content; }
            set { SetField(ref content, value, nameof(Content)); }
        }
        [Column("fls_create")]
        public DateTime Create
        {
            get { return create; }
            set { SetField(ref create, value, nameof(Create)); }
        }
        [Column("fls_lastupdate")]
        public DateTime? LastUpdate
        {
            get { return BaseLastUpdate;  }
            set { SetField(ref BaseLastUpdate, value, nameof(LastUpdate)); }
        }

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

        public AitFilesModel(DBContext context) : base(context)
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
            var file = new AitFilesModel(Context)
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