﻿namespace P02_FootballBetting.Data;


using Microsoft.EntityFrameworkCore;


using P02_FootballBetting.Data.Common;
using P02_FootballBetting.Data.Models;

public class FootballBettingContext : DbContext
{
    public FootballBettingContext() // use it when develop the app locally on our computer
    {

    }
    public FootballBettingContext(DbContextOptions options)
        : base(options)
    {

    }

    public DbSet<Team> Teams { get; set; }
    public DbSet<Color> Colors { get; set; }
    public DbSet<Town> Towns { get; set; }
    public DbSet<Country> Countries { get; set; }
    public DbSet<Position> Positions { get; set; }
    public DbSet<PlayerStatistic> PlayersStatistics { get; set; }
    public DbSet<Game> Games { get; set; }
    public DbSet<Bet> Bets { get; set; }
    public DbSet<User> Users { get; set;}
    public DbSet<Player> Players { get; set; }




    //Connection configiration
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(DbConfig.ConnenctionString);
        }
        base.OnConfiguring(optionsBuilder);
    }

    // For the Fluent API
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //making composite key 
        modelBuilder.Entity<PlayerStatistic>(entity =>
        {
            entity.HasKey(ps => new { ps.GameId, ps.PlayerId });
        });

        modelBuilder.Entity<Team>(entity =>
        {
            entity
            .HasOne(t=>t.PrimaryKitColor)
            .WithMany(c=>c.PrimaryKitTeams)
            .HasForeignKey(t=>t.PrimaryKitColorId)
            .OnDelete(DeleteBehavior.NoAction);

            entity
             .HasOne(t=>t.SecondaryKitColor)
             .WithMany(c=>c.SecondaryKitTeams)
             .HasForeignKey(t=>t.SecondaryKitColorId)
             .OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<Game>(entity =>
        {
            entity
            .HasOne(g=>g.HomeTeam)
            .WithMany(t=>t.HomeGames)
            .HasForeignKey(g=>g.HomeTeamId)
            .OnDelete(DeleteBehavior.NoAction);

            entity
            .HasOne(g=>g.AwayTeam)
            .WithMany(t=>t.AwayGames)
            .HasForeignKey(g=>g.AwayTeamId)
            .OnDelete(DeleteBehavior.NoAction);
        });
    }

}
