namespace P02_FootballBetting.Data.Models;

using P02_FootballBetting.Data.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Town
{
    public Town()
    {
        this.Teams = new HashSet<Team>();
    }
    [Key]
    public int TownId { get; set; }

    [Required]
    [MaxLength(ValidationConstants.TownNameMaxLength)]
    public string Name { get; set; } = null!;


    [ForeignKey(nameof(Country))]
    public int CountryId { get; set; }
    public virtual Country Country { get; set; } = null!;


    //when we have only one foreign key (one to many) we don't need inversed prop!!!
    public virtual ICollection<Team> Teams { get; set; }

    //TO DO: Create nav properties
}

