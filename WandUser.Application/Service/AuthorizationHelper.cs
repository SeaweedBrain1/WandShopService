using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WandUser.Domain.Repositories;

namespace WandUser.Application.Service;

public class AuthorizationHelper : IAuthorizationHelper
{
    private readonly IUserRepository _userRepository;

    public AuthorizationHelper(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<bool> CanEditUserAsync(int currentUserId, List<string> currentUserRoles, int targetUserId)
    {
        var isAdmin = currentUserRoles.Contains("Admin");
        var isEmployee = currentUserRoles.Contains("Employee");
        var isClient = currentUserRoles.Contains("Client");

        if (isAdmin)
            return true;

        if (isClient)
            return currentUserId == targetUserId;

        if (isEmployee)
        {
            if (currentUserId == targetUserId) return true;

            var targetUser = await _userRepository.GetUserByIdAsync(targetUserId);
            return targetUser != null && targetUser.Roles.All(r => r.Name == "Client");
        }

        return false;
    }
}
