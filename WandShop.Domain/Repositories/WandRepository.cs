using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WandShop.Domain.Models;
using WandShop.Domain.Models.Dto;
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
            if (wand.Flexibility != null) _context.Entry(wand.Flexibility).State = EntityState.Unchanged;

            _context.Wands.Add(wand);
            await _context.SaveChangesAsync();
            return wand;
        }

        public async Task<List<Wand>> GetAllWandsAsync()
        {
            return await _context
                .Wands
                .Include(w => w.Flexibility)
                .ToListAsync();
        }

        public async Task<List<Wand>> GetAllValidWandsAsync()
        {
            return await _context
                .Wands
                .Where(w => !w.Deleted)
                .Include(w => w.Flexibility)
                .ToListAsync();
        }

        public async Task<List<Wand>> GetFilteredWandsAsync(WandFilterDto filter)
        {
            var query = _context.Wands.AsQueryable();

            query = ApplyWoodTypeFilter(query, filter);
            query = ApplyLengthFilter(query, filter);
            query = ApplyFlexibilityFilter(query, filter);
            query = query.Include(w => w.Flexibility);
            query = ApplyWandCoreFilter(query, filter);

            return await query.ToListAsync();
        }

        public async Task<Wand> GetWandAsync(int id)
        {
            return await _context
                .Wands
                .Include(w => w.Flexibility)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<Wand> UpdateWandAsync(Wand wand)
        {
            _context.Entry(wand.Flexibility).State = EntityState.Unchanged;
            _context.Wands.Update(wand);
            await _context.SaveChangesAsync();
            return wand;
        }

        private IQueryable<Wand> ApplyWoodTypeFilter(IQueryable<Wand> query, WandFilterDto filter)
        {
            if (filter.WoodType.HasValue)
                query = query.Where(w => w.WoodType == filter.WoodType.Value);
            return query;
        }

        private IQueryable<Wand> ApplyLengthFilter(IQueryable<Wand> query, WandFilterDto filter)
        {
            if (filter.Length.HasValue)
            {
                var start = filter.Length.Value;
                if (start < 0)
                    throw new ArgumentException("Length cannot be negative.");
                var end = start + 1;
                query = query.Where(w => w.Length >= start && w.Length < end);
            }
            return query;
        }

        private IQueryable<Wand> ApplyFlexibilityFilter(IQueryable<Wand> query, WandFilterDto filter)
        {
            if (!string.IsNullOrWhiteSpace(filter.FlexibilityName))
                query = query.Where(w => w.Flexibility.Name.ToLower() == filter.FlexibilityName.ToLower());
            return query;
        }


        private IQueryable<Wand> ApplyWandCoreFilter(IQueryable<Wand> query, WandFilterDto filter)
        {
            if (filter.Core.HasValue)
                query = query.Where(w => w.Core == filter.Core.Value);
            return query;
        }
    }
}
