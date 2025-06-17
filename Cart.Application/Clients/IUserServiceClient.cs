using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WandUser.Domain.Model.Dto;

namespace Cart.Application.Clients
{
    public interface IUserServiceClient
    {
        Task<UserDto?> GetUserByIdAsync(int userId);
    }
}
