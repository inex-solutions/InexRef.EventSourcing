using System.Xml.Serialization;

namespace InexRef.EventSourcing.Common.Hosting.ConfigurationElements
{
    public class ContainerBuilderElement
    {
        [XmlAttribute]
        public string Type { get; set; }
    }
}