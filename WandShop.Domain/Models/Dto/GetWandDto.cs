using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WandShop.Domain.Enums;

namespace WandShop.Domain.Models.Dto;

public record GetWandDto(int Id, WoodType WoodType, decimal Length, WandCore Core, string FlexibilityName, decimal Price)
{
}
