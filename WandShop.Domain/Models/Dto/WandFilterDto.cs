using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using WandShop.Domain.Enums;
using WandShop.Infrastructure.Converters;

namespace WandShop.Domain.Models.Dto;

public class WandFilterDto
{
    public WoodType? WoodType { get; set; }
    public decimal? Length { get; set; }
    //public Flexibility? Flexibility { get; set; }

    public WandCore? Core { get; set; }
}

