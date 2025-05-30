using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WandShop.Domain.Models;
using WandShop.Domain.Models.Dto;
using WandShop.Domain.Repositories;
using WandShop.Domain.Repository;

namespace WandShop.Application.Service;

public class WandService : IWandService
{
    private IWandRepository _wandRepository;
    private IFlexibilityRepository _flexibilityRepository;
    private readonly IMapper _mapper;
    //private readonly IMemoryCache _cache;
    private readonly IDatabase _redisDb;

    public WandService(IWandRepository wandRepository, IFlexibilityRepository flexibilityRepository, IMapper mapper/*, IMemoryCache cache*/)
    {
        _wandRepository = wandRepository;
        _flexibilityRepository = flexibilityRepository;
        _mapper = mapper;
        //_cache = cache;
        var redis = ConnectionMultiplexer.Connect("redis:6379");
        _redisDb = redis.GetDatabase();
    }

    public GetWandDto Add(CreateWandDto createWandDto)
    {
        var result = _wandRepository.AddWandAsync(_mapper.Map<Wand>(createWandDto)).Result;

        return _mapper.Map<GetWandDto>(result);
    }

    public async Task<GetWandDto> AddAsync(CreateWandDto createWandDto)
    {
        var flexibility = _flexibilityRepository.GetFlexibilityByNameAsync(createWandDto.FlexibilityName);
        if (flexibility is null) throw new ArgumentException($"Flexibility {createWandDto.FlexibilityName} does not exist.");
        var wand = _mapper.Map<Wand>(createWandDto);
        wand.Flexibility = await flexibility;

        var result = await _wandRepository.AddWandAsync(wand);

        return _mapper.Map<GetWandDto>(result);
    }

    public async Task<GetWandDto> DeleteWand(int id)
    {

        var wand = await _wandRepository.GetWandAsync(id);
        if (wand == null)
            throw new Exception("Wand not found");

        var updateDto = new UpdateWandDto
        {
            Deleted = true
        };

        await UpdateAsync(id, updateDto);
        return _mapper.Map<GetWandDto>(wand);
    }

    public async Task<List<GetWandDto>> GetAllAsync()
    {
        var result = await _wandRepository.GetAllWandsAsync();

        return result.Select(w => _mapper.Map<GetWandDto>(w)).ToList();
    }

    public async Task<List<GetWandDto>> GetAllValidAsync()
    {
        var result = await _wandRepository.GetAllValidWandsAsync();

        return result.Select(w => _mapper.Map<GetWandDto>(w)).ToList();
    }

    public async Task<GetWandDto> GetAsync(int id)
    {
        var result = await GetWandAsync(id);
        return _mapper.Map<GetWandDto>(result);
    }

    public async Task<List<GetWandDto>> GetWandsByAsync(WandFilterDto filter)
    {
        var result = await _wandRepository.GetFilteredWandsAsync(filter);

        return result.Select(w => _mapper.Map<GetWandDto>(w)).ToList();
    }

    public async Task<GetWandDto> UpdateAsync(int id, UpdateWandDto updateWandDto)
    {
        var wand = await _wandRepository.GetWandAsync(id);
        if (wand == null) throw new Exception("Wand not found");

        _mapper.Map(updateWandDto, wand);

        if (!string.IsNullOrWhiteSpace(updateWandDto.FlexibilityName))
        {
            var flexibility = await _flexibilityRepository.GetFlexibilityByNameAsync(updateWandDto.FlexibilityName);
            if (flexibility == null) throw new ArgumentException($"Flexibility '{updateWandDto.FlexibilityName}' does not exist.");

            wand.Flexibility = flexibility;
        }

        var updated = await _wandRepository.UpdateWandAsync(wand);
        var key = $"wand:{id}";
        await _redisDb.KeyDeleteAsync(key);
        await _redisDb.StringSetAsync(key, JsonSerializer.Serialize(updated), TimeSpan.FromDays(1));

        return _mapper.Map<GetWandDto>(updated);
    }

    private async Task<Wand> GetWandAsync(int id)
    {
        var key = $"wand:{id}";
        var redisValue = await _redisDb.StringGetAsync(key);
        Wand? wand = null;

        if (redisValue.HasValue)
            wand = JsonSerializer.Deserialize<Wand>(redisValue);

        if (wand == null)
        {
            wand = await _wandRepository.GetWandAsync(id);
            if (wand != null)
                await _redisDb.StringSetAsync(key, JsonSerializer.Serialize(wand), TimeSpan.FromDays(1));
        }


        //var result = await _repository.GetWandAsync(id);

        return wand;
    }
}
