using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WandShop.Domain.Models;
using WandShop.Domain.Models.Dto;

namespace WandShop.Application.Service
{
    public interface IWandService
    {
        public Task<List<GetWandDto>> GetAllAsync();
        Task<GetWandDto> GetAsync(int id);
        Task<Wand> GetWandAsync(int id);
        Task<GetWandDto> UpdateAsync(UpdateWandDto updateWandDto);
        Task<GetWandDto> AddAsync(CreateWandDto createWandDto);
        GetWandDto Add(CreateWandDto createWandDto);
        Task<GetWandDto> DeleteWand(int id);
    }
}
