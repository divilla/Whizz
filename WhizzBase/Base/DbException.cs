using System;

namespace WhizzBase.Base
{
    public class DbException : Exception
    {
        public DbException(string message) : base(message) {}
    }
}
