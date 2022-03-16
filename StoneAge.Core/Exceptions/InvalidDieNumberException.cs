using System;

namespace StoneAge.Core.Exceptions
{
    public class InvalidDieNumberException : Exception
    {
        public InvalidDieNumberException(int invalidValue)
            : base($"{invalidValue} is not a valid number of a d6.")
        {
        }
    }
}
