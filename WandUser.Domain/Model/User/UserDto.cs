using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WandUser.Domain.Model.User;

public record UserDto(int Id, string Username, string Email, ICollection<Role> Roles, DateTime CreatedAt, DateTime? LastLoginAt, bool IsActive)
{
}
