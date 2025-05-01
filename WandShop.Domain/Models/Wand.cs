using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WandShop.Domain.Enums;
using WandShop.Domain.Models.Dto;

namespace WandShop.Domain.Models
{
    public class Wand : BaseWand
    {
        public WandCore Core { get; set; }
        public Flexibility Flexibility { get; set; }
        public decimal Price { get; set; }

        public GetWandDto ToGetWandDto()
        {
            return new GetWandDto(this.Id, this.WoodType, this.Length, this.Core, this.Flexibility, this.Price, this.Deleted);
        }

        public UpdateWandDto ToUpdateWandDto()
        {
            return new UpdateWandDto
            {
                Id = this.Id,  
                WoodType = this.WoodType, 
                Length = this.Length,  
                Core = this.Core,  
                Flexibility = this.Flexibility,  
                Price = this.Price,  
                Deleted = this.Deleted  
            };
        }
    }

}
