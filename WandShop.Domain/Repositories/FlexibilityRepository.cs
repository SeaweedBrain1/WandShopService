using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WandShop.Domain.Models;

namespace WandShop.Domain.Repositories;

public class FlexibilityRepository : IFlexibilityRepository
{
    private readonly DataContext _context;

    public FlexibilityRepository(DataContext dataContext)
    {
        _context = dataContext;
    }

    public async Task<Flexibility> GetFlexibilityAsync(int id)
    {
        return await _context.Flexibilities.FirstOrDefaultAsync(f => f.Id == id);
    }

    public async Task<Flexibility> GetFlexibilityByNameAsync(string name)
    {
        return await _context.Flexibilities.FirstOrDefaultAsync(f => f.Name == name);
    }

    public async Task<List<Flexibility>> GetAllFlexibilitiesAsync()
    {
        return await _context.Flexibilities.ToListAsync();
    }

    public async Task<Flexibility> AddFlexibilityAsync(Flexibility flexibility)
    {
        _context.Flexibilities.Add(flexibility);
        await _context.SaveChangesAsync();
        return flexibility;
    }

    public async Task<Flexibility> UpdateFlexibilityAsync(Flexibility flexibility)
    {
        _context.Flexibilities.Update(flexibility);
        await _context.SaveChangesAsync();
        return flexibility;
    }
}
