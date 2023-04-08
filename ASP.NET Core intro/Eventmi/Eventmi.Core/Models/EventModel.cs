using Eventmi.Infrastructure.Data.Common;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Eventmi.Core.Models;

public class EventModel : EventDetailsModel
{
    /// <summary>
    /// Идентификатор на запис
    /// </summary>
    [Key]
    [Comment("Идентификатор на запис")]
    public int Id { get; set; }

}
