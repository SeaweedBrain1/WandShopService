using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserEntity = WandUser.Domain.Model.User.User;

namespace WandUser.Domain.Model.Dto
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserEntity, UserDto>();
        }
    }
}
