using Cart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cart.Application.Services;

public interface ICartService
{
    Task AddItemToCartAsync(int userId, int wandId);
    Task<CartUser> GetUserCartAsync(int userId);
    Task<List<CartUser>> GetAllCartsAsync();
    Task RemoveItemAsync(int userId, int itemId);
    Task DeleteCartAsync(int userId);
    Task PlaceOrderAsync(int userId);
}

