namespace P02_FootballBetting.Data.Models;

using P02_FootballBetting.Data.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Player
{
    public Player()
    {
        this.PlayersStatistics = new HashSet<PlayerStatistic>();
    }
    [Key]
    public int PlayerId { get; set; }

    [Required]
    [MaxLength(ValidationConstants.PlayerNameMaxLength)]
    public string Name { get; set; } = null!;

    public int SquadNumber { get; set; }

    //SQL TYPE = BIT
    //By default  bool is NOT NULL, by default is required
    public bool IsInjured { get; set; }


    [ForeignKey(nameof(Team))]
    public int TeamId { get; set; }
    public virtual Team Team { get; set; } = null!;

    [ForeignKey(nameof(Position))]
    public int PositionId { get; set; }
    public virtual Position Position { get; set; } = null!;

    public virtual ICollection<PlayerStatistic> PlayersStatistics { get; set; }

}

