namespace Eventmi.Infrastructure.Data;

using Eventmi.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Контекст описващ базата данни
/// </summary>
public class EventmiDbContext : DbContext
{
    public EventmiDbContext()
    {

    }
    public EventmiDbContext(DbContextOptions<EventmiDbContext> options)
        : base(options)
    {

    }
    /// <summary>
    /// Таблица със сибития
    /// </summary>
    public DbSet<Event> Events { get; set; } = null!;

}
