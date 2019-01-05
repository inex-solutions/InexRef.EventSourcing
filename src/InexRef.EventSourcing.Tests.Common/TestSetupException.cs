using System;

namespace InexRef.EventSourcing.Tests.Common
{
    public class TestSetupException : Exception
    {
        public TestSetupException(string message) : base(message)
        {
        }
    }
}
