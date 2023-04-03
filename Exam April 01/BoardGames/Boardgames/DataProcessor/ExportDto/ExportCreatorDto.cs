using System.Xml.Serialization;

namespace Boardgames.DataProcessor.ExportDto;

[XmlType("Creator")]
public class ExportCreatorDto
{
    [XmlAttribute("BoardgamesCount")]
    public int BoardgamesCount { get; set; }

    [XmlElement("CreatorName")]
    public string FullName { get; set; } = null!;

    [XmlArray("Boardgames")]
    public ExportCreatorBoeardgameDto[] Boardgames { get; set; } = null!;
}
