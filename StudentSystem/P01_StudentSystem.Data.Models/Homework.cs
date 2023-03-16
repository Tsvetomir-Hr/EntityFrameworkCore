﻿using P01_StudentSystem.Data.Models.Enum;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P01_StudentSystem.Data.Models;

public class Homework
{
    [Key]
    public int HomeworkId { get; set; }

    [Required]
    [MaxLength(250)]
    public string Content { get; set; } = null!;
    public ContentType ContentType  { get; set; }

    public DateTime SubmissionTime { get; set; }


    [ForeignKey(nameof(Student))]
    public int StudentId { get; set; }
    public Student Student { get; set; } = null!;


    [ForeignKey(nameof(Course))]
    public int CourseId { get; set; }
    public Course Course { get; set; } = null!;

}
