using System;

namespace Lzma1900DotNetCore.Compress.Exceptions
{
    public class DataErrorException : ApplicationException
    {
        public DataErrorException() : base("Data Error") { }
    }
}
