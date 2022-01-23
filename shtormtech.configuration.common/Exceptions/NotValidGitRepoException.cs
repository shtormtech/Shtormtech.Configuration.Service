using System;

namespace shtormtech.configuration.common.Exceptions
{
    public class NotValidGitRepoException : Exception
    {
        public NotValidGitRepoException(string message) : base(message)
        {

        }
    }
}
