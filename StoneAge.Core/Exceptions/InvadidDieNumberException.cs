using System;

namespace StoneAge.Core.Exceptions
{
    public class InvadidDieNumberException : Exception
    {
        public InvadidDieNumberException(int invalidValue) : base(string.Format("{0} is not a valid number of a d6.", invalidValue))
        {
        }
    }
}
