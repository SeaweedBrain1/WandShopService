using Cart.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WandUser.Application.Service.Helper;


namespace CartService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CartController : ControllerBase
{
    private ICartService _cartService;
    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    private int GetUserIdFromToken()
    {

        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null) throw new UnauthorizedAccessException("User ID not found in token.");

        return int.Parse(userIdClaim.Value);
    }

    [HttpPost("{wandId}")]
    public async Task<IActionResult> AddItemToCart([FromRoute] int wandId)
    {
        try
        {
            var userId = GetUserIdFromToken();
            var roles = User.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList();


            await _cartService.AddItemToCartAsync(userId, wandId);
            return Ok("Item added to cart.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    [Authorize(Policy = "ClientOnly")]
    public async Task<IActionResult> GetUserCart()
    {
        try
        {
            var userId = GetUserIdFromToken();
            var roles = User.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList();


            var cart = await _cartService.GetUserCartAsync(userId);
            return Ok(cart);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{itemId}")]
    public async Task<IActionResult> RemoveItem([FromRoute] int itemId)
    {
        try
        {
            var userId = GetUserIdFromToken();
            var roles = User.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList();


            await _cartService.RemoveItemAsync(userId, itemId);
            return Ok("Item removed.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete]
    [Authorize(Policy = "ClientOnly")]
    public async Task<IActionResult> DeleteCart()
    {
        try
        {
            var userId = GetUserIdFromToken();
            var roles = User.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList();


            await _cartService.DeleteCartAsync(userId);
            return Ok("Cart deleted.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("all")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> GetAllCarts()
    {
        var carts = await _cartService.GetAllCartsAsync();
        return Ok(carts);
    }

    [HttpPost("place-order")]
    [Authorize(Policy = "ClientOnly")]
    public async Task<IActionResult> PlaceOrder()
    {
        try
        {
            var userId = GetUserIdFromToken();
            await _cartService.PlaceOrderAsync(userId);
            return Ok("Order placed and invoice event sent.");
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

}
