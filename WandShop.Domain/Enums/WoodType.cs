using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WandShop.Infrastructure.Converters;

namespace WandShop.Domain.Enums;

[TypeConverter(typeof(EnumDisplayTypeConverter<WoodType>))]
public enum WoodType
{
    Holly,
    Yew,
    Oak,
    Ash,
    Elder,
    Walnut,
    Willow,
    Maple,
    Cherry,
    Ebony
}
