using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WandShop.Domain.Models;

namespace WandShop.Application.Service
{
    public interface IWandService
    {
        public Task<List<Wand>> GetAllAsync();
        Task<Wand> GetAsync(int id);
        Task<Wand> UpdateAsync(Wand wand);
        Task<Wand> AddAsync(Wand wand);
        Wand Add(Wand wand);
    }
}
