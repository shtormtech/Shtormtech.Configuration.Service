using System;

namespace shtormtech.configuration.common.Exceptions
{
    public class BranchNotFoundException : Exception
    {
        public BranchNotFoundException(string message) : base(message)
        {

        }
    }
}
