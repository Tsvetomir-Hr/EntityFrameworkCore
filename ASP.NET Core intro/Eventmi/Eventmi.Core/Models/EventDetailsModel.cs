using Eventmi.Infrastructure.Data.Common;
using System.ComponentModel.DataAnnotations;

namespace Eventmi.Core.Models
{
    public class EventDetailsModel
    {
        /// <summary>
        /// Име на събитие
        /// </summary>
        [Display(Name = "Name of the event")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Field {0} is required!")]
        [MaxLength(ValidationConstants.EventMaxLength), MinLength(ValidationConstants.EventMinLength, ErrorMessage = "Field {0} must be between {2} and {1} symbols!")]
        public string Name { get; set; } = null!;

        /// <summary>
        /// Начална дата и час
        /// </summary>
        [Display(Name = "Start date and time")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Field {0} is required!")]
        public DateTime Start { get; set; }

        /// <summary>
        /// Крайна дата и час
        /// </summary>
        [Display(Name = "End date and time")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Field {0} is required!")]
        public DateTime End { get; set; }

        /// <summary>
        /// Място на провеждане
        /// </summary>
        [Display(Name = "Place of the event")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Field {0} is required!")]
        [MaxLength(ValidationConstants.EventPlaceMaxLength), MinLength(ValidationConstants.EventPlaceMinLength, ErrorMessage = "Field {0} must be between {2} and {1} symbols!")]
        public string Place { get; set; } = null!;
    }
}
