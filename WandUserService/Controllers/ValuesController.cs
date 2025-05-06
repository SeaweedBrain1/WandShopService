using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

    public UserController(ILoginService loginService, IRegisterService registerService, IUserService userService)
    {
        _loginService = loginService;
        _registerService = registerService;
        _userService = userService;
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
}
