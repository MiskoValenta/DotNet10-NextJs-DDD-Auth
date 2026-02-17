using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Persistence;

public class AppDbContext : DbContext
{
  public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

  public DbSet<User> Users { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<User>().HasKey(u => u.Id);

    // Converstion Strongly Typed ID for Entity Framework
    modelBuilder.Entity<User>()
        .Property(u => u.Id)
        .HasConversion(
            id => id.Value,
            value => new UserId(value)
        );

    modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
  }
}
