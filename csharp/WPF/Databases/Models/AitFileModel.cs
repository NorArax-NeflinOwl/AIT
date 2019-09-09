using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using WPF.Databases.Contexts;
using WPF.Models.Enums;
using WPF.Managers.Validators;
using System.Linq;
using WPF.Managers.Helpers;
using WPF.Managers.Builders;

namespace WPF.Databases.Models
{
    [Table("ait_files")]
    public class AitFileModel : BaseEntityModel, ISerializable, ICloneable
    {
        private string creator, assignedTo, name, content;
        private FileTypesEnum type;
        private AitAccountModel fileCreator, fileOwner;
        private DateTime create;

        [Key, Column("fls_id")]
        public string ID
        {
            get { return BaseID; }
            set
            {
                if (BasePropertiesValidator.ValidatePrimaryKey(value, TablePrefix))
                    SetField(ref BaseID, value, nameof(ID));
            }
        }

        [ForeignKey("FileCreator"), Column("fls_accid")]
        public string Creator
        {
            get { return creator; }
            set
            {
                SetField(ref creator, value, nameof(Creator));
                LastUpdate = DateTime.Now;
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
                SetField(ref assignedTo, value, nameof(AssignedTo));
                LastUpdate = DateTime.Now;
            }
        }

        [Column("fls_name")]
        public string Name
        {
            get { return name; }
            set
            {
                SetField(ref name, value, nameof(Name));
                LastUpdate = DateTime.Now;
            }
        }

        [Column("fls_type")]
        public FileTypesEnum Type
        {
            get { return type; }
            set
            {
                SetField(ref type, value, nameof(Type));
                LastUpdate = DateTime.Now;
            }
        }

        [Column("fls_content")]
        public string Content
        {
            get { return content; }
            set
            {
                SetField(ref content, value, nameof(Content));
                LastUpdate = DateTime.Now;
            }
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

        [NotMapped]
        public TableInerfixEnum TablePrefix { get { return TableInerfixEnum.FLS; } }

        public AitAccountModel FileCreator
        {
            get
            {
                if(fileCreator == null && !string.IsNullOrEmpty(creator) && Fill)
                {
                    fileCreator = Context.Accounts.Where(q => q.ID.Equals(creator)).FirstOrDefault();
                }
                return fileCreator;
            }
            set
            {
                SetField(ref fileCreator, value, nameof(FileCreator));
                if (value != null)
                {
                    creator = value.ID;
                }
                LastUpdate = DateTime.Now;
            }
        }

        public AitAccountModel FileOwner
        {
            get
            {
                if (fileOwner == null && string.IsNullOrEmpty(assignedTo) && Fill)
                    return FileCreator;
                else if (!string.IsNullOrEmpty(assignedTo) && Fill)
                {
                    fileOwner = Context.Accounts.Where(q => q.ID.Equals(assignedTo)).FirstOrDefault();
                }
                return fileOwner;
            }
            set
            {
                SetField(ref fileOwner, value, nameof(FileOwner));
                if(value != null)
                {
                    assignedTo = value.ID;
                }
                LastUpdate = DateTime.Now;
            }
        }

        public AitFileModel(DBContext context) : base(context)
        {
            ID = EntryIdentificatorBuilder.RecordIDGenerator(TablePrefix);

            if (!string.IsNullOrEmpty(PDBContext.Instance.AccountID))
                Creator = PDBContext.Instance.AccountID;

            Create = DateTime.Now;
        }

        [NotMapped]
        public bool IsDetached { get { return string.IsNullOrEmpty(assignedTo) && string.IsNullOrEmpty(creator); } }

        public AitFileModel(SerializationInfo info, StreamingContext context) : base(null)
        {
            ID = (string)info.GetValue(nameof(ID), typeof(string));
            Creator = (string)info.GetValue(nameof(Creator), typeof(string));
            AssignedTo = (string)info.GetValue(nameof(AssignedTo), typeof(string));
            Name = (string)info.GetValue(nameof(Name), typeof(string));
            Type = (FileTypesEnum)info.GetValue(nameof(Type), typeof(FileTypesEnum));
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
            info.AddValue(nameof(Type), Type, typeof(FileTypesEnum));
            info.AddValue(nameof(Content), Content, typeof(string));
            info.AddValue(nameof(Create), Create, typeof(DateTime));
            info.AddValue(nameof(LastUpdate), LastUpdate, typeof(DateTime?));
        }

        public object Clone()
        {
            var file = new AitFileModel(PDBContext.Instance.Context)
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