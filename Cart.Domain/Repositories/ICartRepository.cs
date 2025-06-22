using Cart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cart.Domain.Repositories;

public interface ICartRepository
{
    Task AddItemAsync(int userId, int wandId);
    Task<CartUser?> GetCartAsync(int userId);
    Task<List<CartUser>> GetAllCartsAsync();
    Task RemoveItemAsync(int userId, int itemId);
    Task DeleteCartAsync(int userId);
}
