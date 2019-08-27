using System;
using System.Linq;
using WPF.Databases.Contexts;
using WPF.Models.Extensions.Exceptions;
using WPF.Models.Extensions;
using WPF.Models.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WPF.Databases.Models
{
    public class BaseEntityModel : NotifyPropertyChangedExtension, ITriggerable, IDisposableExtended
    {
        protected string BaseID;
        protected DateTime? BaseLastUpdate;
        protected DBContext Context;

        [NotMapped]
        public bool IsDisposed { get; set; }

        [NotMapped]
        public bool Fill { get; set; }

        public BaseEntityModel(DBContext context) : base(null)
        {
            Context = context;
        }

        public void Insert()
        {
            if (Context?.IsDisposed == true)
            {
                Context = PDBContext.Instance.Context;
            }

            var sts = new SysStsgenids
            {
                ID = BaseID
            };
            Context.Add(sts);
            Context.Entry(sts).State = EntityState.Added;
            Context.Add(this);
            Context.Entry(this).State = EntityState.Added;
            Context.SaveChanges();
        }

        public void Update()
        {
            if (Context?.IsDisposed == true)
            {
                Context = PDBContext.Instance.Context;
            }

            BaseLastUpdate = DateTime.Now;
            Context.Entry(this).State = EntityState.Modified;
            Context.Update(this);
            Context.SaveChanges();
        }

        public void Delete()
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
                    sts.Delete = DateTime.Now;
                    Context.Update(sts);
                    Context.Entry(sts).State = EntityState.Modified;
                    Context.Remove(this);
                    Context.Entry(this).State = EntityState.Deleted;
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
