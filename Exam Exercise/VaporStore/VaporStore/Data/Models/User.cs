

using System.ComponentModel.DataAnnotations;
using VaporStore.Common;

namespace VaporStore.Data.Models
{
    public class User
    {
        public User()
        {
            this.Cards = new HashSet<Card>();
        }
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(GlobalConstants.UsernameMaxLength)]
        public string Username { get; set; } = null!;
        [Required]
        public string FullName { get; set; } = null!;
        [Required]
        public string Email { get; set; } = null!;
        public int Age { get; set; }

        public virtual ICollection<Card> Cards { get; set; }
    }
}
