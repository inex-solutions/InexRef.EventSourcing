using System.IO;
using Microsoft.Extensions.Configuration;

namespace InexRef.EventSourcing.Common.Hosting
{
    public static class HostedEnvironmentConfiguration
    {
        static HostedEnvironmentConfiguration()
        {
            ConfigurationRoot = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddXmlFile(new DirectoryInfo(@"..\..\..\..\InexRef.EventSourcing.config.xml").FullName, optional: false, reloadOnChange: false)
                .AddXmlFile(new DirectoryInfo(@"..\..\..\..\InexRef.EventSourcing.local.config.xml").FullName, optional: true, reloadOnChange: false)
                .AddEnvironmentVariables("INEXREFEVTSRC_")
                .Build();
        }

        public static IConfigurationRoot ConfigurationRoot { get; }
    }
}