using System;
using System.Linq;
using WPF.Databases.Contexts;
using WPF.Exceptions;
using WPF.ExtendedClasses;
using WPF.Interfaces;

namespace WPF.Databases.Models
{
    public class BaseEntityModel : NotifyPropertyChangedExtension, Triggerable
    {
        protected string BaseID;
        protected DateTime? BaseLastUpdate;

        public DBContext Context { get; set; }

        public BaseEntityModel(DBContext context) : base(null)
        {
            Context = context;
        }

        public void Add()
        {
            Context.Add(new SysStsgenids
            {
                ID = BaseID
            });
            Context.Add(this);
        }

        public void Update()
        {
            BaseLastUpdate = DateTime.Now;
            Context.Update(this);
        }

        public void Remove()
        {
            var sts = Context.Stsgenids.Where(q => q.ID.Equals(BaseID)).FirstOrDefault();
            if (sts != null)
            {
                if (sts.Delete == null)
                {
                    sts.Delete = DateTime.Now;
                    Context.Update(sts);
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
}
