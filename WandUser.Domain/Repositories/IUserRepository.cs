using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WandUser.Domain.Model.User;

namespace WandUser.Domain.Repositories;

public interface IUserRepository
{
    Task<User> GetUserByUsernameAsync(string username);
    Task<User> GetUserByEmailAsync(string email);
    Task<User> AddUserAsync(User user);
    Task<User> DeleteUserAsync(int userId);
    Task<List<User>> GetAllUsersAsync();
    Task<User> UpdateUserAsync(User user);
    Task<User> GetUserByIdAsync(int userId);
}
