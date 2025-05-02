using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WandUser.Application.Service;
using WandUser.Domain.Exceptions.Login;
using WandUser.Domain.Model.Requests;

namespace WandUserService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LoginController : ControllerBase
{
    protected ILoginService _loginService;

    public LoginController(ILoginService loginService)
    {
        _loginService = loginService;
    }


    [HttpPost]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        try
        {
            var token = _loginService.Login(request.Username, request.Password);
            return Ok(new { token });
        }
        catch (InvalidCredentialsException)
        {
            return Unauthorized();
        }
    }

    [HttpGet]
    [Authorize]
    [Authorize(Policy = "AdminOnly")]
    public IActionResult AdminPage()
    {
        return Ok("Administrator data only.");
    }
}
