using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WandShop.Domain.Models;
using WandShop.Domain.Models.Dto;
using WandShop.Domain.Repository;

namespace WandShop.Application.Service;

public class WandService : IWandService
{
    private IWandRepository _repository;
    //private readonly IMemoryCache _cache;
    //private readonly IDatabase _redisDb;

    public WandService(IWandRepository repository/*, IMemoryCache cache*/)
    {
        _repository = repository;
        //_cache = cache;
        //var redis = ConnectionMultiplexer.Connect("localhost:6379");
        //_redisDb = redis.GetDatabase();
    }

    public GetWandDto Add(CreateWandDto createWandDto)
    {
        var result = _repository.AddWandAsync(createWandDto.ToWand()).Result;

        return result.ToGetWandDto();
    }

    public async Task<GetWandDto> AddAsync(CreateWandDto createWandDto)
    {
        var result = await _repository.AddWandAsync(createWandDto.ToWand());

        return result.ToGetWandDto();
    }

    public async Task<GetWandDto> DeleteWand(int id)
    {
        var wandToDelete = await GetWandAsync(id);
        wandToDelete.Deleted = true;
        await UpdateAsync(wandToDelete.ToUpdateWandDto());
        return wandToDelete.ToGetWandDto();
        //var result = await _wandService.UpdateAsync(wand);
    }

    public async Task<List<GetWandDto>> GetAllAsync()
    {
        var result = await _repository.GetAllWandsAsync();

        return result.Select(w => w.ToGetWandDto()).ToList();
    }

    public async Task<GetWandDto> GetAsync(int id)
    {
        var result = await _repository.GetWandAsync(id);

        return result.ToGetWandDto();
    }

    public async Task<Wand> GetWandAsync(int id)
    {
        var result = await _repository.GetWandAsync(id);

        return result;
    }

    public async Task<GetWandDto> UpdateAsync(UpdateWandDto updateWandDto)
    {
        var wand = await _repository.GetWandAsync(updateWandDto.Id);
        if (wand == null) throw new Exception("Wand not found");

        //if (updateWandDto.WoodType.HasValue) wand.WoodType = updateWandDto.WoodType.Value;
        //if (updateWandDto.Length.HasValue) wand.Length = updateWandDto.Length.Value;
        //if (updateWandDto.Core.HasValue) wand.Core = updateWandDto.Core.Value;
        //if (updateWandDto.Flexibility.HasValue) wand.Flexibility = updateWandDto.Flexibility.Value;
        //if (updateWandDto.Price.HasValue) wand.Price = updateWandDto.Price.Value;

        var updated = await _repository.UpdateWandAsync(updateWandDto.ToWand(wand));
        return updated.ToGetWandDto();

        //var result = await _repository.UpdateWandAsync(wand);

        //return result.ToGetWandDto();
    }
}
