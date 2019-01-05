using System;

namespace InexRef.EventSourcing.Common.Hosting
{
    public class HostedEnvironmentConfigurationException : Exception
    {
        public HostedEnvironmentConfigurationException(string message) : base (message)
        {
            
        }
    }
}