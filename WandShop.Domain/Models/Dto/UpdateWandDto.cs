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
    public string? FlexibilityName { get; set; }
    public decimal? Price { get; set; }
    public bool? Deleted { get; set; }

}


