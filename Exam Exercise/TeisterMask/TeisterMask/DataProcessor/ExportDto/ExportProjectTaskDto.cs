using System.Xml.Serialization;
using TeisterMask.Data.Models.Enums;

namespace TeisterMask.DataProcessor.ExportDto;

[XmlType("Task")]
public class ExportProjectTaskDto
{
    [XmlElement("Name")]
    public string Name { get; set; } = null!;

    [XmlElement("Label")]
    public string LabelType { get; set; } = null!;
}
