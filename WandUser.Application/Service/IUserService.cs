using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WandUser.Domain.Model.Dto;

namespace WandUser.Application.Service
{
    public interface IUserService
    {
        Task<List<UserDto>> GetAllUsersAsync();
        Task<UserDto> DeleteUserAsync(int userId);
        Task<UserDto> ResetPasswordAsync(int userId, string newPassword);
        Task<UserDto> GetUserByIdAsync(int userId);
        Task<UserDto> UpdateUserProfileAsync(int userId, string? newUsername, string? newEmail);


    }
}
