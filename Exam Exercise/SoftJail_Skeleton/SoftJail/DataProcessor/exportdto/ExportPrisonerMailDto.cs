using System.Xml;
using System.Xml.Serialization;

namespace SoftJail.DataProcessor.ExportDto
{
    [XmlType("Message")]
    public class ExportPrisonerMailDto
    {
       [XmlElement("Description")]
        public string Description { get; set; }
    }
}
