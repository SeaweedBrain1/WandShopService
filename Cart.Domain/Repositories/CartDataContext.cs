using Cart.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cart.Domain.Repositories;

public class CartDataContext : DbContext
{
    public CartDataContext(DbContextOptions<CartDataContext> options) : base(options) { }

    public DbSet<CartUser> CartUsers { get; set; }
    public DbSet<CartItem> CartItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CartUser>()
        .HasMany(cu => cu.CartItems)
        .WithOne(ci => ci.CartUser);
    }
}
