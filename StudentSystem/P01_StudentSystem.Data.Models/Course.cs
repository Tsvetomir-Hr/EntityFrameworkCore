namespace P01_StudentSystem.Data.Models;

using P01_StudentSystem.Data.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Course
{
    public Course()
    {
        this.Students = new HashSet<Student>();
        this.Resources = new HashSet<Resource>();
        this.Homeworks = new HashSet<Homework>();
    }
    [Key]
    public int CourseId { get; set; }

    [Required]
    [DataType("nvarchar")]
    [MaxLength(ValidationConstants.CourseNameMaxLength)]
    public string Name { get; set; } = null!;

    [DataType("nvarchar")]
    [MaxLength(500)]
    public string? Description { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal Price { get; set; }

    public ICollection<Student> Students { get; set; }
    public ICollection<Resource> Resources { get; set; }
    public ICollection<Homework> Homeworks { get; set; }

}
