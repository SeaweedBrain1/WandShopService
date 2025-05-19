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
            //_context.Wands.RemoveRange(_context.Wands);
            //await _context.SaveChangesAsync();

            if (!_context.Flexibilities.Any())
            {
                var flexibilities = new List<Flexibility>
                {
                    new Flexibility { Name = "Supple" },
                    new Flexibility { Name = "Rigid" },
                    new Flexibility { Name = "SlightlyRigid" },
                    new Flexibility { Name = "VeryFlexible" }
                };

                _context.Flexibilities.AddRange(flexibilities);
                await _context.SaveChangesAsync();
            }

            var supple = _context.Flexibilities.First(f => f.Name == "Supple");
            var rigid = _context.Flexibilities.First(f => f.Name == "Rigid");
            var slightlyRigid = _context.Flexibilities.First(f => f.Name == "SlightlyRigid");
            var veryFlexible = _context.Flexibilities.First(f => f.Name == "VeryFlexible");

            if (!_context.Wands.Any())
            {


                var wands = new List<Wand>
                {


                    new Wand
                    {
                        WoodType = WoodType.Holly,
                        Length = 12.5m,
                        Core = WandCore.PhoenixFeather,
                        Flexibility = supple,
                        Price = 250.00m
                    },
                    new Wand
                    {
                        WoodType = WoodType.Yew,
                        Length = 14.0m,
                        Core = WandCore.DragonHeartstring,
                        Flexibility = rigid,
                        Price = 300.00m
                    },
                    new Wand
                    {
                        WoodType = WoodType.Oak,
                        Length = 13.0m,
                        Core = WandCore.UnicornHair,
                        Flexibility = slightlyRigid,
                        Price = 270.00m
                    },
                    new Wand
                    {
                        WoodType = WoodType.Willow,
                        Length = 15.5m,
                        Core = WandCore.ThestralTailHair,
                        Flexibility = veryFlexible,
                        Price = 350.00m
                    }
                };

                _context.Wands.AddRange(wands);

                await _context.SaveChangesAsync();
            }
        }
    }
}
