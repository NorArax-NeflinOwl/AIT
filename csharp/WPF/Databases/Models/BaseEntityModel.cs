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

        [NotMapped]
        public bool IsDisposed { get; set; }

        public BaseEntityModel(DBContext context) : base(null)
        {
        }

        public void Insert()
        {
            using(var context = PDBContext.Instance.Context)
            {
                var sts = new SysStsgenids
                {
                    ID = BaseID
                };
                context.Add(sts);
                context.Entry(sts).State = EntityState.Added;
                context.Add(this);
                context.Entry(this).State = EntityState.Added;
                context.SaveChanges();
            }
        }

        public void Update()
        {
            using (var context = PDBContext.Instance.Context)
            {
                BaseLastUpdate = DateTime.Now;
                context.Update(this);
                context.Entry(this).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void Delete()
        {
            using(var context = PDBContext.Instance.Context)
            {
                var sts = context.Stsgenids.Where(q => q.ID.Equals(BaseID)).FirstOrDefault();
                if (sts != null)
                {
                    if (sts.Delete == null)
                    {
                        sts.Delete = DateTime.Now;
                        context.Update(sts);
                        context.Entry(sts).State = EntityState.Modified;
                        context.Remove(this);
                        context.Entry(this).State = EntityState.Deleted;
                        context.SaveChanges();
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
        }

        public void Dispose()
        {
            IsDisposed = true;
            GC.Collect();
        }
    }
}
