namespace P01_StudentSystem.Data.Models;

using P01_StudentSystem.Data.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.AccessControl;

public class Resource
{
    [Key]
    public int ResourceId { get; set; }

    [Required]
    [Column("nvarchar")]
    [MaxLength(ValidationConstants.CourseNameMaxLength)]
    public string Name { get; set; } = null!;

    [Required]
    [MaxLength(300)]
    public string Url { get; set; } = null!;

    public ResourceType ResourceType { get; set; }


    [ForeignKey(nameof(Course))]
    public int CourseId { get; set; }
    public Course Course { get; set; } = null!;
}
