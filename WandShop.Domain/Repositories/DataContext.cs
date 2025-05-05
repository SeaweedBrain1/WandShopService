using Microsoft.EntityFrameworkCore;
using WandShop.Domain.Enums;
using WandShop.Domain.Models;

namespace WandShop.Domain.Repositories;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    public DbSet<Wand> Wands { get; set; }
}
