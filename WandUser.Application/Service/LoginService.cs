using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WandUser.Domain.Exceptions.Login;
using WandUser.Domain.Repositories;

namespace WandUser.Application.Service;

public class LoginService : ILoginService
{
    protected IJwtTokenService _jwtTokenService;
    protected IUserRepository _userRepository;

    public LoginService(IJwtTokenService jwtTokenService, IUserRepository userRepository)
    {
        _jwtTokenService = jwtTokenService;
        _userRepository = userRepository;
    }

    //public string Login(string username, string password)
    //{
    //    if (username == "admin" && password == "password")
    //    {
    //        var roles = new List<string> { "Client", "Employee", "Administrator" };
    //        var token = _jwtTokenService.GenerateToken(123, roles);
    //        return token;
    //    }
    //    else
    //    {
    //        throw new InvalidCredentialsException();
    //    }

    //}

    public async Task<string> LoginAsync(string username, string password)
    {
        var user = await _userRepository.GetUserByUsernameAsync(username);

        if (user == null)
            throw new InvalidCredentialsException();

        var isPasswordValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
        if (!isPasswordValid)
            throw new InvalidCredentialsException();

        user.LastLoginAt = DateTime.UtcNow;
        await _userRepository.UpdateUserAsync(user);

        var roles = user.Roles.Select(r => r.Name).ToList();
        var token = _jwtTokenService.GenerateToken(user.Id, roles);
        return token;
    }
}
