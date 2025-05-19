using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WandUser.Application.Service;
using WandUser.Domain.Exceptions.Login;
using WandUser.Domain.Exceptions.Register;
using WandUser.Domain.Model.Requests;
using WandUser.Domain.Model.User;

namespace WandUserService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    protected ILoginService _loginService;
    protected IRegisterService _registerService;
    protected IUserService _userService;
    protected IAuthorizationHelper _authorizationHelper;

    public UserController(ILoginService loginService, IRegisterService registerService, IUserService userService, IAuthorizationHelper authorizationHelper)
    {
        _loginService = loginService;
        _registerService = registerService;
        _userService = userService;
        _authorizationHelper = authorizationHelper;
    }


    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        try
        {
            var token = await _loginService.LoginAsync(request.Username, request.Password);
            return Ok(new { token });
        }
        catch (InvalidCredentialsException ex)
        {
            return Unauthorized(ex.Message);
        }
    }

    [HttpGet]
    [Authorize]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Get()
    {
        try
        {
            var result = await _userService.GetAllUsersAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        try
        {
            var token = await _registerService.RegisterAsync(request.Username, request.Email, request.Password);
            return Ok(new { token });
        }
        catch (UsernameAlreadyTakenException ex)
        {
            return Conflict(ex.Message);
        }
        catch (UserAlreadyExistsException ex)
        {
            return Conflict(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("registerWithRole")]
    [Authorize]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> RegisterWithRole([FromBody] RegisterWithRoleRequest request)
    {
        try
        {
            var token = await _registerService.RegisterWithRoleAsync(request.Username, request.Email, request.Password, request.RoleName);
            return Ok(new { token });
        }
        catch (UsernameAlreadyTakenException ex)
        {
            return Conflict(ex.Message);
        }
        catch (UserAlreadyExistsException ex)
        {
            return Conflict(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    [Authorize]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _userService.DeleteUserAsync(id);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPatch("reset-password")]
    [Authorize]
    [Authorize(Policy = "ClientEmployeeOrAdmin")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
    {
        try
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var roles = User.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList();

            var allowed = await _authorizationHelper.CanEditUserAsync(currentUserId, roles, request.UserId);
            if (!allowed)
                return Forbid();



            var result = await _userService.ResetPasswordAsync(request.UserId, request.NewPassword);
            return Ok(new
            {
                message = "Password reset successfully.",
                result
            });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPatch("update-profile")]
    [Authorize(Policy = "ClientEmployeeOrAdmin")]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateUserProfileRequest request)
    {
        try
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var roles = User.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList();

            var allowed = await _authorizationHelper.CanEditUserAsync(currentUserId, roles, request.UserId);
            if (!allowed) return Forbid();

            var result = await _userService.UpdateUserProfileAsync(request.UserId, request.NewUsername, request.NewEmail);
            return Ok(new
            {
                message = "Profile updated successfully.",
                result
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }


}
