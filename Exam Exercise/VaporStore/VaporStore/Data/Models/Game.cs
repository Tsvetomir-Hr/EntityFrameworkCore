﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VaporStore.Data.Models;

public class Game
{
    public Game()
    {
        this.Purchases = new HashSet<Purchase>();
        this.GameTags = new HashSet<GameTag>();
    }
    [Key]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = null!;

    public decimal Price { get; set; }

    public DateTime ReleaseDate { get; set; }

    [ForeignKey(nameof(Developer))]
    public int DeveloperId { get; set; }
    [Required]
    public virtual Developer Developer { get; set; } = null!;

    [ForeignKey(nameof(Genre))]
    public int GenreId { get; set; }
    [Required]
    public virtual Genre Genre { get; set; } = null!;


    public virtual ICollection<Purchase> Purchases { get; set; } = null!;

    public virtual ICollection<GameTag> GameTags { get; set; } = null!;
}
