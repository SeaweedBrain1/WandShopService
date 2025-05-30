using Cart.Application.Clients;
using Cart.Domain.Exceptions;
using Cart.Domain.Models;
using Cart.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cart.Application.Services;

public class CartService : ICartService
{
    private readonly ICartRepository _repository;
    private readonly IWandServiceClient _wandClient;

    public CartService(ICartRepository repository, IWandServiceClient wandClient)
    {
        _repository = repository;
        _wandClient = wandClient;
    }

    public async Task AddItemToCartAsync(int userId, int wandId)
    {
        if (wandId <= 0)
            throw new ArgumentException("Invalid wand ID.");

        var isValid = await _wandClient.IsWandValidAsync(wandId);
        if (!isValid)
            throw new NotFoundException($"Wand with ID {wandId} does not exist or is marked as deleted.");

        await _repository.AddItemAsync(userId, wandId);

        //if (wandId <= 0)
        //    throw new ArgumentException("Invalid wand ID.");

        //await _repository.AddItemAsync(userId, wandId);
    }

    //public async Task<CartUser?> GetUserCartAsync(int userId)
    //{
    //    return await _repository.GetCartAsync(userId);
    //}

    public async Task<CartUser> GetUserCartAsync(int userId)
    {
        var cart = await _repository.GetCartAsync(userId);
        if (cart == null)
            throw new NotFoundException($"Cart for user {userId} not found.");

        return cart;
    }


    public async Task<List<CartUser>> GetAllCartsAsync()
    {
        return await _repository.GetAllCartsAsync();
    }

    public async Task RemoveItemAsync(int userId, int itemId)
    {
        var cart = await _repository.GetCartAsync(userId);
        if (cart == null)
            throw new NotFoundException($"Cart for user {userId} not found.");

        var item = cart.CartItems.FirstOrDefault(i => i.Id == itemId);
        if (item == null)
            throw new NotFoundException($"Item with ID {itemId} not found in cart.");

        await _repository.RemoveItemAsync(userId, itemId);
    }

    public async Task DeleteCartAsync(int userId)
    {
        var cart = await _repository.GetCartAsync(userId);
        if (cart == null)
            throw new NotFoundException($"Cart for user {userId} not found.");

        await _repository.DeleteCartAsync(userId);
    }
}
