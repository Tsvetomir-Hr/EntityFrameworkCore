using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace VaporStore.DataProcessor.ImportDto
{
    [XmlType("Purchase")]
    public class ImportPurchaseDto
    {
        [XmlAttribute("title")]
        [Required]
        public string GameName { get; set; } = null!;

        [XmlElement("Type")]
        [Required]
        public string Type { get; set; } = null!;

        [XmlElement("Key")]
        [Required]
        [RegularExpression("[A-Z0-9]{4}-[A-Z0-9]{4}-[A-Z0-9]{4}")]
        public string ProductKey { get; set; } = null!;

        [XmlElement("Card")]
        [Required]
        [RegularExpression("[0-9]{4}\\s[0-9]{4}\\s[0-9]{4}\\s[0-9]{4}")]
        public string CardNumber { get; set; } = null!;

        [Required]
        public string Date { get; set; } = null!;
    }
}
