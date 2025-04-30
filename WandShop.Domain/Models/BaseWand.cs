using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WandShop.Domain.Enums;

namespace WandShop.Domain.Models
{
    public class BaseWand
    {
        public int Id { get; set; }
        public WoodType WoodType { get; set; }
        public decimal Length { get; set; }
        public bool Deleted { get; set; } = false;
    }

}
