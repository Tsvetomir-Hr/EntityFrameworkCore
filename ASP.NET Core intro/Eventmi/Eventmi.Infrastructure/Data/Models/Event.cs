using Eventmi.Infrastructure.Data.Common;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Eventmi.Infrastructure.Data.Models;
/// <summary>
/// Събития
/// </summary>
[Comment("Events(СЪбития)")]
public class Event
{
    /// <summary>
    /// Идентификатор на запис
    /// </summary>
    [Key]
    [Comment("Идентификатор на запис")]
    public int Id { get; set; }

    /// <summary>
    /// Име на събитие
    /// </summary>
    [Required]
    [MaxLength(ValidationConstants.EventMaxLength)]
    [Comment("Име на събитие")]
    public string Name { get; set; } = null!;

    /// <summary>
    /// Начална дата и час
    /// </summary>
    [Comment("Начална дата и час")]
    public DateTime Start { get; set; }

    /// <summary>
    /// Крайна дата и час
    /// </summary>
    [Comment("Крайна дата и час")]
    public DateTime End { get; set; }

    /// <summary>
    /// Място на провеждане
    /// </summary>
    [Required]
    [MaxLength(ValidationConstants.EventPlaceMaxLength)]
    [Comment("Място на провеждане")]
    public string Place { get; set; } = null!;
}
