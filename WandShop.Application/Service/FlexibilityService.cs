using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WandShop.Domain.Models;
using WandShop.Domain.Repositories;

namespace WandShop.Application.Service;

public class FlexibilityService : IFlexibilityService
{
    private readonly IFlexibilityRepository _flexibilityRepository;
    public FlexibilityService(IFlexibilityRepository flexibilityRepository)
    {
        _flexibilityRepository = flexibilityRepository;
    }

    public async Task<Flexibility> GetFlexibilityAsync(int id)
    {
        return await _flexibilityRepository.GetFlexibilityAsync(id);
    }

    public async Task<List<Flexibility>> GetAllFlexibilitiesAsync()
    {
        return await _flexibilityRepository.GetAllFlexibilitiesAsync();
    }

    public async Task<Flexibility> AddFlexibilityAsync(Flexibility flexibility)
    {
        return await _flexibilityRepository.AddFlexibilityAsync(flexibility);
    }

    public async Task<Flexibility> UpdateFlexibilityAsync(Flexibility flexibility)
    {
        return await _flexibilityRepository.UpdateFlexibilityAsync(flexibility);
    }
}
