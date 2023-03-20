namespace Theatre.Data.Models
{
    using global::Theatre.Common;
    using System.ComponentModel.DataAnnotations;


    public class Theatre
    {
        public Theatre()
        {
            this.Tickets = new HashSet<Ticket>();
        }
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(ValidationConstants.TheatreNameMaxLength)]
        public string Name { get; set; } = null!;


        public sbyte NumberOfHalls { get; set; }

        [Required]
        [MaxLength(ValidationConstants.TheatreDirectorMaxLength)]
        public string Director { get; set; } = null!;

        public virtual ICollection<Ticket> Tickets { get; set; }

    }
}