using System.Xml.Serialization;

namespace Artillery.DataProcessor.ExportDto
{
    [XmlType("Gun")]
    public class ExportGunDto
    {
        [XmlAttribute("Manufacturer")]
        public string ManufacturerName { get; set; } = null!;

        [XmlAttribute("GunType")]
        public string GunType { get; set; } = null!;

        [XmlAttribute("GunWeight")]
        public int GunWeight { get; set; }

        [XmlAttribute("BarrelLength")]
        public double BarrelLength { get; set; }

        [XmlAttribute("Range")]
        public int Range { get; set; }

        [XmlArray("Countries")]
        public ExpoertContriesDto[] CountriesGuns { get; set; } = null!;
    }
}
