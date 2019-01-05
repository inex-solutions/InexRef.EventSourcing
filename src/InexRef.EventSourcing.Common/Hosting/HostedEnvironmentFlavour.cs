using System;
using System.Collections.Generic;
using System.Linq;
using InexRef.EventSourcing.Common.Container;
using InexRef.EventSourcing.Common.Hosting.ConfigurationElements;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InexRef.EventSourcing.Common.Hosting
{
    public static class HostedEnvironmentFlavour
    {
        private static readonly HostingFlavoursConfigurationElement HostingFlavoursConfiguration;

        static HostedEnvironmentFlavour()
        {
            var hostingFlavours = new HostingFlavoursConfigurationElement();
            HostedEnvironmentConfiguration.ConfigurationRoot.GetSection("HostingFlavours").Bind(hostingFlavours);

            HostingFlavoursConfiguration = hostingFlavours;
            AvailableFlavours = hostingFlavours.AvailableFlavours.SplitAndTrim(",");

            VerifyConfiguration();
        }

        private static void VerifyConfiguration()
        {
            var flavourConfigurationBlocks = HostingFlavoursConfiguration.HostingFlavour.Select(f => f.Name).ToArray();

            if (!AvailableFlavours.All(f => flavourConfigurationBlocks.Contains(f)))
            {
                var msg =
                    "Hosting Flavours Configuration Error - not all specified available flavours have corresponding configuration blocks" +
                    $"flavour configuration blocks: {flavourConfigurationBlocks.ToBulletList()}\n" +
                    $"available flavours list: {AvailableFlavours.ToBulletList()}\n";
                throw new HostedEnvironmentConfigurationException(msg);
            }
        }

        public static IEnumerable<string> AvailableFlavours { get; }


        public static void ConfigureContainerForHostEnvironmentFlavour(ServiceCollection builder, string flavour)
        {
            foreach (var item in HostingFlavoursConfiguration.HostingFlavour.First(f => f.Name == flavour).AutofacContainerBuilder)
            {
                var type = Type.GetType(item.Type);
                var module = (ContainerConfigurationModule)Activator.CreateInstance(type);
                module.ConfigureContainer(builder);
            }
        }
    }
}
