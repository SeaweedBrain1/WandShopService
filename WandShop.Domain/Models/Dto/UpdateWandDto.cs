using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WandShop.Domain.Enums;

namespace WandShop.Domain.Models.Dto;

public class UpdateWandDto
{
    public WoodType? WoodType { get; set; }
    public decimal? Length { get; set; }
    public WandCore? Core { get; set; }
    public Flexibility? Flexibility { get; set; }
    public decimal? Price { get; set; }
    public bool? Deleted { get; set; }

    public Wand ToWand(Wand wand)
    {
        if (WoodType.HasValue) wand.WoodType = WoodType.Value;
        if (Length.HasValue) wand.Length = Length.Value;
        if (Core.HasValue) wand.Core = Core.Value;
        if (Flexibility.HasValue) wand.Flexibility = Flexibility.Value;
        if (Price.HasValue) wand.Price = Price.Value;
        if (Deleted.HasValue) wand.Deleted = Deleted.Value;

        return wand;
    }
}


