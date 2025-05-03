using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WandShop.Infrastructure.Converters;

namespace WandShop.Domain.Enums;

[TypeConverter(typeof(EnumDisplayTypeConverter<WandCore>))]
public enum WandCore
{
    PhoenixFeather,
    DragonHeartstring,
    UnicornHair,
    ThestralTailHair,
    VeelaHair,
    BasiliskHorn,
    ThunderbirdFeather,
    KelpieHair
}
