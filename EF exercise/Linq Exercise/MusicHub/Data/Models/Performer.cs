using System.ComponentModel.DataAnnotations;

namespace MusicHub.Data.Models;

public class Performer
{
    public Performer()
    {
        this.PerformerSongs = new HashSet<SongPerformer>();
          
    }
    [Key]
    public int Id { get; set; }
    [Required]
    [MaxLength(DataValidation.PerformerNamesMaxLength)]
    public string FirstName { get; set; } = null!;
    [Required]
    [MaxLength(DataValidation.PerformerNamesMaxLength)]
    public string LastName { get; set; } = null!;
    public int Age { get; set; }
    public decimal NetWorth { get; set; }
    public virtual ICollection<SongPerformer> PerformerSongs{ get; set; }
    

}
