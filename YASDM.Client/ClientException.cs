using System;

namespace YASDM.Client
{
    public class ClientException : Exception
    {
        public ClientException(string message) : base(message) { }
    }
}