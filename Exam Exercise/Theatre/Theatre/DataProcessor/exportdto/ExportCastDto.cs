﻿using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

namespace Theatre.DataProcessor.ExportDto;

[XmlType("Actor")]
public class ExportCastDto
{
    [XmlAttribute("FullName")]
    public string FullName { get; set; } = null!;

    [XmlAttribute("MainCharacter")]
    public string MainCharacter { get; set; } = null!;


}
