using Microsoft.EntityFrameworkCore;
using WandShop.Domain.Enums;
using WandShop.Domain.Models;

namespace WandShop.Domain.Repositories;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    public DbSet<Wand> Wands { get; set; }
    public DbSet<Flexibility> Flexibilities { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Wand>()
            .HasOne(w => w.Flexibility)
            .WithMany()
            .HasForeignKey(w => w.FlexibilityId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
