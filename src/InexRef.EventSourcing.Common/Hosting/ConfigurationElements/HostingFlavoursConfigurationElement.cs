using System.Xml.Serialization;

namespace InexRef.EventSourcing.Common.Hosting.ConfigurationElements
{
    public class HostingFlavoursConfigurationElement
    {
        [XmlAttribute]
        public string AvailableFlavours { get; set; }

        [XmlElement]
        public HostingFlavourElement[] HostingFlavour { get; set; }
    }
}