using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WandShop.Domain.Models;
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

    public Wand Add(Wand wand)
    {
        var result = _repository.AddWandAsync(wand).Result;

        return result;
    }

    public async Task<Wand> AddAsync(Wand wand)
    {
        var result = await _repository.AddWandAsync(wand);

        return result;
    }

    public async Task<List<Wand>> GetAllAsync()
    {
        var result = await _repository.GetAllWandsAsync();

        return result;
    }

    public async Task<Wand> GetAsync(int id)
    {
        var result = await _repository.GetWandAsync(id);

        return result;
    }

    public async Task<Wand> UpdateAsync(Wand wand)
    {
        var result = await _repository.UpdateWandAsync(wand);

        return result;
    }
}
