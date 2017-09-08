using System;

namespace Rob.EventSourcing.Tests.SpecificationTests
{
    public class SpecificationException : Exception
    {
        public SpecificationException(string message) : base(message)
        {
            
        }
    }
}