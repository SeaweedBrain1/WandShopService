using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WandUser.Domain.Exceptions.Register;
using WandUser.Domain.Model.User;
using WandUser.Domain.Repositories;
using WandUser.Domain.Security;

namespace WandUser.Application.Service
{
    public class RegisterService : IRegisterService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IRoleRepository _roleRepository;

        public RegisterService(IUserRepository userRepository, IPasswordHasher passwordHasher, IRoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _roleRepository = roleRepository;
        }

        public async Task<User> RegisterAsync(string username, string email, string password)
        {
            var existingEmail = await _userRepository.GetUserByEmailAsync(email);
            if (existingEmail != null)
                throw new UserAlreadyExistsException();

            var existingUsername = await _userRepository.GetUserByUsernameAsync(username);
            if (existingUsername != null)
                throw new UsernameAlreadyTakenException();

            var clientRole = await _roleRepository.GetRoleByName("Client");
            if (clientRole == null)
                throw new Exception("Role 'Client' not found in database.");

            var newUser = new User
            {
                Username = username,
                Email = email,
                PasswordHash = _passwordHasher.Hash(password),
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                Roles = new List<Role> { clientRole }
            };

            await _userRepository.AddUserAsync(newUser);

            return newUser;
        }


        public async Task<User> RegisterWithRoleAsync(string username, string email, string password, string roleName)
        {
            var existingEmail = await _userRepository.GetUserByEmailAsync(email);
            if (existingEmail != null)
                throw new UserAlreadyExistsException();

            var existingUsername = await _userRepository.GetUserByUsernameAsync(username);
            if (existingUsername != null)
                throw new UsernameAlreadyTakenException();

            var roleFromDb = await _roleRepository.GetRoleByName(roleName);
            if (roleFromDb == null)
                throw new Exception("This role was not found in database.");

            var newUser = new User
            {
                Username = username,
                Email = email,
                PasswordHash = _passwordHasher.Hash(password),
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                Roles = new List<Role> { roleFromDb }
            };

            await _userRepository.AddUserAsync(newUser);

            return newUser;
        }

        
    }
}
