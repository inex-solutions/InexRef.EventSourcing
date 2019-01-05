using System;

namespace InexRef.EventSourcing.Tests.Common.SpecificationFramework
{
    public class SpecificationException : Exception
    {
        public SpecificationException(string message) : base(message)
        {
            
        }
    }
}