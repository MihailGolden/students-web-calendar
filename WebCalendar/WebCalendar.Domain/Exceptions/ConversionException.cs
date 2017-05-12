using System;

namespace WebCalendar.Domain.Exceptions
{
    public class ConversionException : Exception
    {
        public ConversionException(string msg, Exception exc): base(msg, exc)
        {
        }
    }
}
