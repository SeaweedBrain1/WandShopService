using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WandUser.Domain.Model.User;

namespace WandUser.Application.Service
{
    public interface IRegisterService
    {
        Task<User> RegisterAsync(string username, string email, string password);
        Task<User> RegisterWithRoleAsync(string username, string email, string password, string roleName);
    }
}
