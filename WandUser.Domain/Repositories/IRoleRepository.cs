using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WandUser.Domain.Model.User;

namespace WandUser.Domain.Repositories
{
    public interface IRoleRepository
    {
        public Task<Role> GetRoleByName(string roleName);
    }
}
