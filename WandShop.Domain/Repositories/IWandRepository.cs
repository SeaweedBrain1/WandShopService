using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WandShop.Domain.Models;
using WandShop.Domain.Models.Dto;

namespace WandShop.Domain.Repository
{
    public interface IWandRepository
    {
        #region Wand
        Task<Wand> GetWandAsync(int id);
        Task<Wand> AddWandAsync(Wand wand);
        Task<Wand> UpdateWandAsync(Wand wand);
        Task<List<Wand>> GetAllWandsAsync();
        Task<List<Wand>> GetFilteredWandsAsync(WandFilterDto filter);

        #endregion

    }
}
