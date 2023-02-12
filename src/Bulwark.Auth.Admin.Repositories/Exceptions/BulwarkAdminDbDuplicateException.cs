using System;
namespace Bulwark.Admin.Repositories.Exceptions
{
    public class BulwarkAdminDbDuplicateException : Exception
    {
        public BulwarkAdminDbDuplicateException(
            string message) : base(message)
        { }

        public BulwarkAdminDbDuplicateException(string message,
            Exception inner) : base(message, inner)
        {

        }
    }
}

