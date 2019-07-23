using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WPF.Validators;

namespace WPF.Models
{
    [Table("ait_files")]
    public class AitFilesModel
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

        public AitFilesModel()
        {
            Create = DateTime.Now;
        }
    }
}
