using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WandUser.Domain.Model.Requests
{
    public class UpdateUserProfileRequest
    {
        public int UserId { get; set; } 
        public string? NewUsername { get; set; }
        public string? NewEmail { get; set; }
    }
}
