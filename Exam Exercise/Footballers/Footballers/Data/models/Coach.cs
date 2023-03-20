using Footballers.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Footballers.Data.Models
{
    public class Coach
    {
        public Coach()
        {
            this.Footballers = new HashSet<Footballer>();
        }
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(ValidationConstants.CoachNameMaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(ValidationConstants.NationalityMaxLength)]
        public string Nationality { get; set; } = null!;

        public virtual ICollection<Footballer> Footballers { get; set; }
    }
}
