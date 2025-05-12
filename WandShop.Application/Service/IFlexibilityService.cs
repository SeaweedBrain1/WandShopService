using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WandShop.Domain.Models;

namespace WandShop.Application.Service
{
    public interface IFlexibilityService
    {
        Task<Flexibility> GetFlexibilityAsync(int id);
        Task<List<Flexibility>> GetAllFlexibilitiesAsync();
        Task<Flexibility> AddFlexibilityAsync(Flexibility flexibility);
        Task<Flexibility> UpdateFlexibilityAsync(Flexibility flexibility);
    }
}
