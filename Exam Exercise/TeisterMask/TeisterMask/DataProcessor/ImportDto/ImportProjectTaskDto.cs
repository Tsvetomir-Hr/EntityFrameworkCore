using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace TeisterMask.DataProcessor.ImportDto;

[XmlType("Task")]
public class ImportProjectTaskDto
{
    [XmlElement("Name")]
    [Required]
    [MinLength(GlobalConstants.TaskNameMinLength)]
    [MaxLength(GlobalConstants.TaskNameMaxLength)]
    public string Name { get; set; } = null!;

    [XmlElement("OpenDate")]
    [Required]
    public string OpenDate { get; set; } = null!;

    [XmlElement("DueDate")]
    [Required]
    public string DueDate { get; set; } = null!;

    [XmlElement("ExecutionType")]
    [Required]
    public string ExecutionType { get; set; } = null!;

    [XmlElement("LabelType")]
    [Required]
    public string LabelType { get; set; } = null!;

}
