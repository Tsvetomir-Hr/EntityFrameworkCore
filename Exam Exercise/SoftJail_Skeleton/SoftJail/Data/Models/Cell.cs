﻿using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace SoftJail.Data.Models
{
    public class Cell
    {
        public Cell()
        {
            this.Prisoners = new HashSet<Prisoner>();
        }
        [Key]
        public int Id { get; set; }

        public int CellNumber  { get; set; }

        public bool HasWindow  { get; set; }

        [ForeignKey(nameof(Department))]
        public int DepartmentId { get; set; }
        [Required]
        public Department Department { get; set; }

        public virtual ICollection<Prisoner> Prisoners { get; set; }
    }
}
