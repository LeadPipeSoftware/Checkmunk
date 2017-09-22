using System;

namespace Checkmunk.Application.Common
{
    public class InvalidCommandOrQueryException : Exception
    {
        public InvalidCommandOrQueryException()
        {
        }

        public InvalidCommandOrQueryException(string message)
            : base(message)
        {
        }

        public InvalidCommandOrQueryException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
