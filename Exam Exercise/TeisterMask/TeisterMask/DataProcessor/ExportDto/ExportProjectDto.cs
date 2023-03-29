using System.Xml.Serialization;

namespace TeisterMask.DataProcessor.ExportDto;

[XmlType("Project")]
public class ExportProjectDto
{
    [XmlAttribute("TasksCount")]
    public int TasksCount { get; set; }

    [XmlElement("ProjectName")]
    public string Name { get; set; } = null!;

    [XmlElement("HasEndDate")]
    public string? DueDate { get; set; }

    [XmlArray("Tasks")]
    public ExportProjectTaskDto[] Tasks { get; set; } = null!;
}
