using System;

namespace shtormtech.configuration.common.Exceptions
{
    public class DirectoryNotEmptyException : Exception
    {
        public DirectoryNotEmptyException(string message, Exception? ex) : base(message, ex)
        {

        }
    }
}
