using Boardgames.Common;
using System.ComponentModel.DataAnnotations;

namespace Boardgames.Data.Models;

public class Creator
{
    public Creator()
    {
        this.Boardgames = new HashSet<Boardgame>();
    }
    [Key]
    public int Id { get; set; }
    [Required]
    [MaxLength(GlobalConstants.CreatorNamesMaxLength)]
    public string FirstName { get; set; } = null!;
    [Required]
    [MaxLength(GlobalConstants.CreatorNamesMaxLength)]
    public string LastName { get; set; } = null!;

    public virtual ICollection<Boardgame> Boardgames { get; set; }
}
