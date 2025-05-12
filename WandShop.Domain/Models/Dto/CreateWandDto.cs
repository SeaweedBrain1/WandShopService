using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WandShop.Domain.Enums;

namespace WandShop.Domain.Models.Dto;

public record CreateWandDto(WoodType WoodType, decimal Length, WandCore Core, int FlexibilityId, decimal Price)
{
    public Wand ToWand()
    {
        return new Wand
        {
            WoodType = this.WoodType,
            Length = this.Length,
            Core = this.Core,
            FlexibilityId = this.FlexibilityId,
            Price = this.Price
        };
    }
}
