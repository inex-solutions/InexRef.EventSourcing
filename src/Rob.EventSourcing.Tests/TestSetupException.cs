using System;

namespace Rob.EventSourcing.Tests
{
    public class TestSetupException : Exception
    {
        public TestSetupException(string message) : base(message)
        {
        }
    }
}
