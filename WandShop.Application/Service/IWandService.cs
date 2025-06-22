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
        Task<List<GetWandDto>> GetAllAsync();
        Task<List<GetWandDto>> GetAllValidAsync();
        Task<GetWandDto> GetAsync(int id);
        Task<GetWandDto> UpdateAsync(int id, UpdateWandDto updateWandDto);
        Task<GetWandDto> AddAsync(CreateWandDto createWandDto);
        GetWandDto Add(CreateWandDto createWandDto);
        Task<GetWandDto> DeleteWand(int id);
        Task<List<GetWandDto>> GetWandsByAsync(WandFilterDto filter);
    }
}
