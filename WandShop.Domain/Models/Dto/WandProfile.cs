using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WandShop.Domain.Models.Dto;

public class WandProfile : Profile
{
    public WandProfile()
    {
        CreateMap<CreateWandDto, Wand>()
            .ForMember(dest => dest.Flexibility, opt => opt.Ignore());

        CreateMap<Wand, GetWandDto>()
            .ForCtorParam("FlexibilityName", opt => opt.MapFrom(src => src.Flexibility.Name));

        CreateMap<Wand, UpdateWandDto>()
            .ForMember(dest => dest.FlexibilityName, opt => opt.MapFrom(src => src.Flexibility.Name));


        CreateMap<UpdateWandDto, Wand>()
            .ForMember(dest => dest.Flexibility, opt => opt.Ignore()) 
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

    }
}
