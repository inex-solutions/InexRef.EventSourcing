using System.Xml.Serialization;

namespace InexRef.EventSourcing.Common.Hosting.ConfigurationElements
{
    public class HostingFlavourElement
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlElement]
        public ContainerBuilderElement[] AutofacContainerBuilder { get; set; }
    }
}