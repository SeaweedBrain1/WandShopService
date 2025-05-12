using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WandShop.Domain.Enums;
using WandShop.Domain.Models.Dto;
using WandShop.Infrastructure.Converters;

namespace WandShop.Domain.Models
{
    public class Wand : BaseWand
    {
        public WandCore Core { get; set; }
        public int FlexibilityId { get; set; }
        public Flexibility Flexibility { get; set; }
        public decimal Price { get; set; }

        public GetWandDto ToGetWandDto()
        {
            return new GetWandDto(this.Id, this.WoodType, this.Length, this.Core, StringDisplayConverter.ConvertToPrettyCase(this.Flexibility.Name), this.Price, this.Deleted);
        }

        public UpdateWandDto ToUpdateWandDto()
        {
            return new UpdateWandDto
            { 
                WoodType = this.WoodType, 
                Length = this.Length,  
                Core = this.Core,
                FlexibilityId = this.FlexibilityId,  
                Price = this.Price,  
                Deleted = this.Deleted  
            };
        }
    }

}
