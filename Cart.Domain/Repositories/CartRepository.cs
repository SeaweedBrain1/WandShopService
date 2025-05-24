using Cart.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cart.Domain.Repositories;

public class CartRepository : ICartRepository
{
    private readonly CartDataContext _context;

    public CartRepository(CartDataContext context)
    {
        _context = context;
    }

    private async Task<CartUser> GetOrCreateCartAsync(int userId)
    {
        var cart = await GetCartAsync(userId);

        if (cart == null)
        {
            cart = new CartUser { UserId = userId };
            _context.CartUsers.Add(cart);
            await _context.SaveChangesAsync();
        }

        return cart;
    }

    public async Task AddItemAsync(int userId, int wandId)
    {
        var cart = await GetOrCreateCartAsync(userId);

        var existingItem = cart.CartItems.FirstOrDefault(i => i.WandId == wandId);
        if (existingItem != null)
            existingItem.Quantity++;
        else
            cart.CartItems.Add(new CartItem { WandId = wandId });

        await _context.SaveChangesAsync();
    }

    public async Task<CartUser?> GetCartAsync(int userId)
    {
        return await _context.CartUsers
            .Where(c => c.UserId == userId && !c.Deleted)
            .Include(c => c.CartItems)
            .FirstOrDefaultAsync();
    }

    public async Task<List<CartUser>> GetAllCartsAsync()
    {
        return await _context.CartUsers
            .Include(c => c.CartItems)
            .ToListAsync(); 
    }

    public async Task RemoveItemAsync(int userId, int itemId)
    {
        var cart = await _context.CartUsers
            .Include(c => c.CartItems)
            .FirstOrDefaultAsync(c => c.UserId == userId && !c.Deleted);

        var item = cart?.CartItems.FirstOrDefault(i => i.Id == itemId);
        if (item != null)
        {
            if (item.Quantity > 1)
                item.Quantity--;
            else
                _context.CartItems.Remove(item);

            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteCartAsync(int userId)
    {
        var cart = await _context.CartUsers
            .FirstOrDefaultAsync(c => c.UserId == userId && !c.Deleted);

        if (cart != null)
        {
            cart.Deleted = true;
            await _context.SaveChangesAsync();
        }
    }
}