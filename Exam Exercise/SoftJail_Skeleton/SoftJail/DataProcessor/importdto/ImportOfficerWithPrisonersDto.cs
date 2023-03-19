using SoftJail.Common;
using SoftJail.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace SoftJail.DataProcessor.importdto
{
    [XmlType("Officer")]
    public class ImportOfficerWithPrisonersDto
    {
        [XmlElement("Name")]
        [MinLength(ValidationConstants.OfficerFullNameMinLength)]
        [MaxLength(ValidationConstants.OfficerFullNameMaxLength)]
        [Required]
        public string FullName { get; set; }

        [XmlElement("Money")]
        [Range(typeof(decimal), ValidationConstants.OfficerMinSalary, ValidationConstants.OfficerMaxSalary)]
        public decimal Salary { get; set; }

        [XmlElement("Position")]
        [Required]
        public string Position { get; set; }

        [XmlElement("Weapon")]
        [Required]
        public string Weapon { get; set; }

        [XmlElement("DepartmentId")]
        public int DeaprtmentId { get; set; }

        [XmlArray("Prisoners")]
        public ImportOfficerPrisonerDto[] OfficerPrisoners { get; set; }
    }
}
