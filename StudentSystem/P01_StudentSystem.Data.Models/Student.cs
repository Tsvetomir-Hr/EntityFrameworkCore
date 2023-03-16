namespace P01_StudentSystem.Data.Models;

using P01_StudentSystem.Data.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Student
{
    public Student()
    {
        this.Courses = new HashSet<Course>();
        this.Homeworks = new HashSet<Homework>();
    }

    [Key]
    public int StudentId { get; set; }
    [Required]
    [MaxLength(ValidationConstants.studentNameMaxLength)]
    [DataType("nvarchar")]
    public string Name { get; set; } = null!;

    [StringLength(10)]
    public string? PhoneNumber { get; set; }
    public DateTime RegisteredOn { get; set; }
    public DateTime?  Birthday  { get; set; }

    public ICollection<Course> Courses { get; set; }
    public ICollection<Homework> Homeworks { get; set; }

}
