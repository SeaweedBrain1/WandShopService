using Cart.Application.Clients;
using Cart.Domain.Exceptions;
using Cart.Domain.Models;
using Cart.Domain.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WandUser.Application.Producer;
using WandUser.Domain.Repositories;

namespace Cart.Application.Services;

public class CartService : ICartService
{
    private readonly ICartRepository _repository;
    private readonly IWandServiceClient _wandClient;
    private readonly IKafkaProducer _kafkaProducer;
    private readonly IUserServiceClient _userClient;

    public CartService(ICartRepository repository, IWandServiceClient wandClient, IKafkaProducer kafkaProducer, IUserServiceClient userClient)
    {
        _repository = repository;
        _wandClient = wandClient;
        _kafkaProducer = kafkaProducer;
        _userClient = userClient;
    }

    public async Task AddItemToCartAsync(int userId, int wandId)
    {
        if (wandId <= 0)
            throw new ArgumentException("Invalid wand ID.");

        var isValid = await _wandClient.IsWandValidAsync(wandId);
        if (!isValid)
            throw new NotFoundException($"Wand with ID {wandId} does not exist or is marked as deleted.");

        await _repository.AddItemAsync(userId, wandId);
    }





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


    public async Task PlaceOrderAsync(int userId)
    {
        var cart = await _repository.GetCartAsync(userId);
        if (cart == null || !cart.CartItems.Any())
            throw new InvalidOperationException("Cart is empty or does not exist.");

        var user = await _userClient.GetUserByIdAsync(userId);
        if (user == null)
            throw new Exception("User not found");

        var orderItems = new List<object>();
        decimal total = 0;

        foreach (var item in cart.CartItems)
        {
            var wand = await _wandClient.GetWandByIdAsync(item.WandId); // lub WandId
            if (wand == null)
                throw new Exception($"Wand with ID {item.WandId} not found.");

            orderItems.Add(new
            {
                ProductName = wand.Id.ToString(),
                Quantity = item.Quantity
            });

            total += wand.Price * item.Quantity;
        }

        var orderMessage = new
        {
            Email = user.Email,
            Items = orderItems,
            Total = total
        };

        var jsonMessage = JsonConvert.SerializeObject(orderMessage);
        await _kafkaProducer.SendMessageAsync("invoice-email-topic", jsonMessage);

        await _repository.DeleteCartAsync(userId);
    }
}
