using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WandShop.Domain.Enums;
using WandShop.Domain.Models;
using WandShop.Domain.Repositories;

namespace WandShop.Domain.Seeders
{
    public class WandSeeder : IWandSeeder
    {
        private readonly DataContext _context;

        public WandSeeder(DataContext context)
        {
            _context = context;
        }

        public async Task Seed()
        {
            if (!_context.Wands.Any())
            {
                var wands = new List<Wand>
                {
                    new Wand
                    {
                        WoodType = WoodType.Holly,
                        Length = 12.5m,
                        Core = WandCore.PhoenixFeather,
                        Flexibility = Flexibility.Supple,
                        Price = 250.00m
                    },
                    new Wand
                    {
                        WoodType = WoodType.Yew,
                        Length = 14.0m,
                        Core = WandCore.DragonHeartstring,
                        Flexibility = Flexibility.Rigid,
                        Price = 300.00m
                    },
                    new Wand
                    {
                        WoodType = WoodType.Oak,
                        Length = 13.0m,
                        Core = WandCore.UnicornHair,
                        Flexibility = Flexibility.SlightlyRigid,
                        Price = 270.00m
                    },
                    new Wand
                    {
                        WoodType = WoodType.Willow,
                        Length = 15.5m,
                        Core = WandCore.ThestralTailHair,
                        Flexibility = Flexibility.VeryFlexible,
                        Price = 350.00m
                    }
                };

                _context.Wands.AddRange(wands);

                await _context.SaveChangesAsync();
            }
        }
    }
}
