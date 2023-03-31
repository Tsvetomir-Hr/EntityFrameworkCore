﻿using Footballers.Common;
using Footballers.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Footballers.Data.Models;


public class Footballer
{
    public Footballer()
    {
        this.TeamsFootballers = new HashSet<TeamFootballer>();
    }
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(GlobalConstants.FootballerNameMaxLength)]
    public string Name { get; set; } = null!;

    public DateTime ContractStartDate { get; set; }

    public DateTime ContractEndDate { get; set; }

    public PositionType PositionType { get; set; }

    public BestSkillType BestSkillType { get; set; }

    [ForeignKey(nameof(Coach))]
    public int CoachId { get; set; }
    public virtual Coach Coach { get; set; } = null!;
    public virtual ICollection<TeamFootballer> TeamsFootballers { get; set; }

}

