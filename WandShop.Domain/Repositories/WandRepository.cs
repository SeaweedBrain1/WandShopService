using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WandShop.Domain.Models;
using WandShop.Domain.Repositories;

namespace WandShop.Domain.Repository
{
    public class WandRepository : IWandRepository
    {
        private readonly DataContext _context;

        public WandRepository(DataContext dataContext)
        {
            _context = dataContext;
        }

        public async Task<Wand> AddWandAsync(Wand wand)
        {
            _context.Wands.Add(wand);
            await _context.SaveChangesAsync();
            return wand;
        }

        public async Task<List<Wand>> GetAllWandsAsync()
        {
            return await _context.Wands.ToListAsync();
        }

        public async Task<Wand> GetWandAsync(int id)
        {
            return await _context.Wands.Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Wand> UpdateWandAsync(Wand wand)
        {
            _context.Wands.Update(wand);
            await _context.SaveChangesAsync();
            return wand;
        }
    }
}
