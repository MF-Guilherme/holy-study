using HolyStudy.Models;
using Microsoft.EntityFrameworkCore;

namespace HolyStudy.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options){ }


    public DbSet<Theme> Themes { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<Passage> Passages { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Book>()
            .HasIndex(b => b.Abbreviation)
            .IsUnique();

        modelBuilder.Entity<Passage>()
            .Property(p => p.EndVerse)
            .IsRequired(false);
    }
}