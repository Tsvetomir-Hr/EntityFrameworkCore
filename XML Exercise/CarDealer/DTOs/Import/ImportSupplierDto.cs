using System.Xml.Serialization;

namespace CarDealer.DTOs.Import;


[XmlType("Supplier")]
public class ImportSupplierDto
{
    //xml elements
    [XmlElement("name")]
    public string Name { get; set; }

    [XmlElement("isImporter")]
    public bool IsisImporter { get; set; }

}
