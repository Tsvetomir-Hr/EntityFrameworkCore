using System.Xml.Serialization;

namespace Boardgames.DataProcessor.ExportDto;

[XmlType("Boardgame")]
public class ExportCreatorBoeardgameDto
{
    [XmlElement("BoardgameName")]
    public string Name { get; set; } = null!;

    [XmlElement("BoardgameYearPublished")]
    public int YearPublished { get; set; }
}
