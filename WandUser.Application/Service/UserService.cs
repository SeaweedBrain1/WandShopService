using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WandUser.Domain.Model.User;
using WandUser.Domain.Repositories;

namespace WandUser.Application.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;

        public UserService(IUserRepository userRepository, IRoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        public async Task<UserDto> DeleteUserAsync(int userId)
        {
            var result = await _userRepository.DeleteUserAsync(userId);
            return result.ToUserDto();
        }

        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            var result = await _userRepository.GetAllUsersAsync();
            return result.Select(u => u.ToUserDto()).ToList(); ;
        }
    }
}
