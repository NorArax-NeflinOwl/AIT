using System;
using System.Linq;
using WPF.Databases.Contexts;
using WPF.Models.Extensions.Exceptions;
using WPF.Models.Extensions;
using WPF.Models.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace WPF.Databases.Models
{
    public class BaseEntityModel : NotifyPropertyChangedExtension, ITriggerable, IDisposableExtended
    {
        private bool? isDeleted;

        protected string BaseID;
        protected DateTime? BaseLastUpdate;
        protected string BaseLastActionComment;

        protected DBContext Context;

        [NotMapped]
        public bool IsDisposed { get; set; }

        [NotMapped]
        protected bool Fill { get; set; }

        [NotMapped]
        public bool IsDeleted
        {
            get
            {
                if(isDeleted == null)
                {
                    using (var context = PDBContext.Instance.Context)
                    {
                        var sgiEntry = context.Stsgenids.Where(entry => entry.ID.Equals(BaseID)).FirstOrDefault();
                        if (sgiEntry != null)
                        {
                            isDeleted = sgiEntry.Delete != null;
                        }
                        else
                        {
                            isDeleted = true;
                        }
                    }
                }

                return (bool)isDeleted;
            }
        }

        public BaseEntityModel(DBContext context) : base(null)
        {
            Context = context;
        }

        public void FillObject()
        {
            Fill = true;
        }

        public void Insert()
        {
            if (Context?.IsDisposed == true)
            {
                Context = PDBContext.Instance.Context;
            }

            var creator = PDBContext.Instance.AccountID;
            if(!string.IsNullOrEmpty(creator))
            {
                creator = ConfigurationManager.AppSettings["TasksManager"].ToString();
            }

            var sts = new SysStsgenids
            {
                ID = BaseID,
                CreateBy = creator
            };

            Context.Add(sts);
            Context.Entry(sts).State = EntityState.Added;
            Context.Add(this);
            Context.Entry(this).State = EntityState.Added;
            Context.SaveChanges();
        }

        public void Update()
        {
            UpdateForReason(null);
        }

        public void UpdateForReason(string reason)
        {
            if (Context?.IsDisposed == true)
            {
                Context = PDBContext.Instance.Context;
            }

            BaseLastUpdate = DateTime.Now;
            BaseLastActionComment = reason;
            Context.Entry(this).State = EntityState.Modified;
            Context.Update(this);
            Context.SaveChanges();
        }

        public void ForceDelete()
        {
            DeleteForReason(null, true);
        }

        public void Delete()
        {
            DeleteForReason(null, false);
        }

        public void DeleteForReason(string reason, bool forceDelete = false)
        {
            if (Context?.IsDisposed == true)
            {
                Context = PDBContext.Instance.Context;
            }

            var sts = Context.Stsgenids.Where(q => q.ID.Equals(BaseID)).FirstOrDefault();
            if (sts != null)
            {
                if (sts.Delete == null)
                {
                    var deleter = PDBContext.Instance.AccountID;
                    if (!string.IsNullOrEmpty(deleter))
                    {
                        deleter = ConfigurationManager.AppSettings["TasksManager"].ToString();
                    }
                    sts.DeleteBy = deleter;
                    sts.Delete = DateTime.Now;

                    var deleteEntityMode = ConfigurationManager.AppSettings["DeleteEntryMode"].ToString();
                    if (deleteEntityMode.Equals("ON") || forceDelete)
                    {
                        Context.Remove(this);
                        Context.Entry(this).State = EntityState.Deleted;
                    }
                    else
                    {
                        isDeleted = true;
                    }

                    if(!string.IsNullOrEmpty(reason))
                        sts.DeleteReason = reason;

                    Context.Update(sts);
                    Context.Entry(sts).State = EntityState.Modified;

                    Context.SaveChanges();
                }
                else
                {
                    throw new SqliteExceptions.EntityIntegrationViolated($"Stsgenids {BaseID} was alredy mark as deleted. The integrity of the database was violated");
                }
            }
            else
            {
                throw new SqliteExceptions.EntityNotFound($"Stsgenids {BaseID} entity not found");
            }
        }

        public void Dispose()
        {
            IsDisposed = true;
            Context.Dispose();
            GC.Collect();
        }
    }
}
