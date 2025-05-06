using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WandUser.Domain.Model.User;

namespace WandUser.Application.Service
{
    public interface IUserService
    {
        Task<List<User>> GetAllUsersAsync();
        Task<User> DeleteUserAsync(int userId);
    }
}
