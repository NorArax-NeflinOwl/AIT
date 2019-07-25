using System;
using System.Runtime.Serialization;

namespace WPF.Models.Extensions.Exceptions
{
    public class SqliteExceptions
    {
        public class EntityNotFound : Exception
        {
            public EntityNotFound(string message) : base(message)
            {
            }
        }

        [Serializable]
        internal class EntityIntegrationViolated : Exception
        {

            public EntityIntegrationViolated(string message) : base(message)
            {
            }
        }
    }
}
