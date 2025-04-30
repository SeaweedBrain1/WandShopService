using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WandShop.Domain.Enums;

namespace WandShop.Domain.Models
{
    public class Wand : BaseWand
    {
        public WandCore Core { get; set; }
        public Flexibility Flexibility { get; set; }
        public decimal Price { get; set; }
    }

}
