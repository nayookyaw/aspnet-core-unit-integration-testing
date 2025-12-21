

using Microsoft.EntityFrameworkCore;
using UserApi.Models;

namespace UserApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}
    public DbSet<User> Users => Set<User>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var user = modelBuilder.Entity<User>();
        user.HasKey(x => x.Id);
        user.Property(x => x.Username)
            .HasMaxLength(100)
            .IsRequired();
        user.Property(x => x.Email)
            .HasMaxLength(150)
            .IsRequired();
        user.HasIndex(x => x.Email).IsUnique();
        user.Property(x => x.CreatedAt).IsRequired();
    }
}