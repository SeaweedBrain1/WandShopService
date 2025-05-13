using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WandUser.Application.Service
{
    public interface IAuthorizationHelper
    {
        Task<bool> CanEditUserAsync(int currentUserId, List<string> currentUserRoles, int targetUserId);
    }

}
