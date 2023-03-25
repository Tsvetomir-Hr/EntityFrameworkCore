using Artillery.Common;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Artillery.DataProcessor.ImportDto;

[XmlType("Shell")]
public class ImportShellDto
{
    [XmlElement("ShellWeight")]
    [Range(typeof(double), GlobalConstants.ShellWeightMin, GlobalConstants.ShellWeightMax)]
    public double ShellWeight { get; set; }

    [XmlElement("Caliber")]
    [Required]
    [MinLength(GlobalConstants.ShellCaliberMinLength)]
    [MaxLength(GlobalConstants.ShellCaliberMaxLength)]
    public string Caliber { get; set; } = null!;
}
