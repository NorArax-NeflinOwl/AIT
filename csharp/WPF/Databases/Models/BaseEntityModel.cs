using System;
using System.Linq;
using WPF.Databases.Contexts;
using WPF.Models.Extensions.Exceptions;
using WPF.Models.Extensions;
using WPF.Models.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace WPF.Databases.Models
{
    public class BaseEntityModel : NotifyPropertyChangedExtension, ITriggerable, IDisposableExtended
    {
        protected string BaseID;
        protected DateTime? BaseLastUpdate;

        [NotMapped]
        public DBContext Context { get; set; }

        [NotMapped]
        public bool IsDisposed { get; set; }

        public BaseEntityModel(DBContext context) : base(null)
        {
            Context = context;
        }

        public void Insert()
        {
            using(var context = PDBContext.Instance.Context)
            {
                context.Add(new SysStsgenids
                {
                    ID = BaseID
                });
                context.Add(this);
            }
        }

        public void Update()
        {
            using (var context = PDBContext.Instance.Context)
            {
                BaseLastUpdate = DateTime.Now;
                context.Update(this);
            }
        }

        public void Delete()
        {
            var sts = Context.Stsgenids.Where(q => q.ID.Equals(BaseID)).FirstOrDefault();
            if (sts != null)
            {
                if (sts.Delete == null)
                {
                    sts.Delete = DateTime.Now;
                    using (var context = PDBContext.Instance.Context)
                    {
                        context.Update(sts);
                        context.Remove(this);
                    }
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
            Context.Dispose();

            IsDisposed = true;
            GC.Collect();
        }
    }
}
