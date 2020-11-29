using System;

namespace WhizzBase.Base
{
    public class DbArgumentException : ArgumentException
    {
        public DbArgumentException(string message) : base(message) {}
        public DbArgumentException(string message, string paramName) : base(message, paramName) {}
    }
}
