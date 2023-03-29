using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using Trucks.Common;

namespace Trucks.DataProcessor.ImportDto
{
    [XmlType("Truck")]
    public class ImportDespatcherTruckDto
    {
        [StringLength(GlobalConstants.RegistrationNumberLength)]
        [RegularExpression("[A-Z]{2}[0-9]{4}[A-Z]{2}")]
        [XmlElement("RegistrationNumber")]
        [Required]
        public string? RegistrationNumber { get; set; } 

        [XmlElement("VinNumber")]
        [StringLength(GlobalConstants.VinNumberLength)]
        public string? VinNumber { get; set; }

        [XmlElement("TankCapacity")]
        [Range(GlobalConstants.TankCapacityMin, GlobalConstants.TankCapacityMax)]
        public int TankCapacity { get; set; }

        [XmlElement("CargoCapacity")]
        [Range(GlobalConstants.CargoCapacityMin, GlobalConstants.CargoCapacityMax)]
        public int CargoCapacity { get; set; }

        [Required]
        [XmlElement("CategoryType")]
        public string CategoryType { get; set; } = null!;

        [Required]
        [XmlElement("MakeType")]
        public string MakeType { get; set; } = null!;
    }
}
