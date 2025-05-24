using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WandUser.Domain.Model.Dto;
using WandUser.Domain.Repositories;
using WandUser.Domain.Security;

namespace WandUser.Application.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        //private readonly IRoleRepository _roleRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, /*IRoleRepository roleRepository,*/ IPasswordHasher passwordHasher, IMapper mapper)
        {
            _userRepository = userRepository;
            //_roleRepository = roleRepository;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
        }

        public async Task<UserDto> DeleteUserAsync(int userId)
        {
            var result = await _userRepository.DeleteUserAsync(userId);
            return _mapper.Map<UserDto>(result);

        }

        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            var result = await _userRepository.GetAllUsersAsync();
            return result.Select(u => _mapper.Map<UserDto>(u)).ToList();
        }

        public async Task<UserDto> ResetPasswordAsync(int userId, string newPassword)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
                throw new Exception("User not found");

            user.PasswordHash = _passwordHasher.Hash(newPassword);
            await _userRepository.UpdateUserAsync(user);

            return _mapper.Map<UserDto>(user);

        }

        public async Task<UserDto> GetUserByIdAsync(int userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            //return user?.ToUserDto();
            return user != null ? _mapper.Map<UserDto>(user) : null;
        }

        public async Task<UserDto> UpdateUserProfileAsync(int userId, string? newUsername, string? newEmail)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null) throw new Exception("User not found");

            if (!string.IsNullOrWhiteSpace(newUsername))  user.Username = newUsername;

            if (!string.IsNullOrWhiteSpace(newEmail)) user.Email = newEmail;

            await _userRepository.UpdateUserAsync(user);
            return _mapper.Map<UserDto>(user);
        }


    }
}
