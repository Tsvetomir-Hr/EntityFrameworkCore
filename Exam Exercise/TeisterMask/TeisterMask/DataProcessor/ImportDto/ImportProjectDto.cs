using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace TeisterMask.DataProcessor.ImportDto;

[XmlType("Project")]
public class ImportProjectDto
{
    [XmlElement("Name")]
    [Required]
    [MinLength(GlobalConstants.ProjectNameMinLength)]
    [MaxLength(GlobalConstants.ProjectNameMaxLength)]
    public string Name { get; set; } = null!;

    [XmlElement("OpenDate")]
    [Required]
    public string OpenDate { get; set; } = null!;

    [XmlElement("DueDate")]
    public string? DueDate { get; set; }

    [XmlArray("Tasks")]
    public ImportProjectTaskDto[] Tasks { get; set; } = null!;
}
