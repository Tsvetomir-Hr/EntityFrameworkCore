using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using Trucks.Common;

namespace Trucks.DataProcessor.ImportDto;

[XmlType("Despatcher")]
public class ImportDespatcherDto
{
    [Required]
    [XmlElement("Name")]
    [MinLength(GlobalConstants.DespatcherNameMinLength)]
    [MaxLength(GlobalConstants.DespatcherNameMaxLength)]
    public string Name { get; set; } = null!;

    [XmlElement("Position")]
    public string? Position { get; set; }

    [XmlArray("Trucks")]
    public ImportDespatcherTruckDto[] Trucks { get; set; } = null!;

}
