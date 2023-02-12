using System;
namespace Bulwark.Admin.Repositories.Exceptions
{
    public class BulwarkAdminDbException : Exception
    {
        public BulwarkAdminDbException(
            string message) : base(message)
        { }

        public BulwarkAdminDbException(string message,
            Exception inner) : base(message, inner)
        {

        }
    }
}

