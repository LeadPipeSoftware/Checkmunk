using System;

namespace Checkmunk.Domain.Users
{
    public class InvalidEmailAddressException : Exception
    {
        public InvalidEmailAddressException(string emailAddress)
            : base($"{emailAddress} is not a valid email address.")
        {
        }

        public InvalidEmailAddressException(string emailAddress, Exception inner)
            : base($"{emailAddress} is not a valid email address.", inner)
        {
        }
    }
}
