using System;

namespace Lzma1900DotNetCore.Compress.Exceptions
{
    public class InvalidParamException : ApplicationException
    {
        public InvalidParamException() : base("Invalid Parameter") { }
    }
}
