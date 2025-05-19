using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WandShop.Domain.Models;

namespace WandShop.Domain.Repositories;

public interface IFlexibilityRepository
{
    #region Flexibility
    Task<Flexibility> GetFlexibilityAsync(int id);
    Task<Flexibility> GetFlexibilityByNameAsync(string name);
    Task<Flexibility> AddFlexibilityAsync(Flexibility flexibility);
    Task<Flexibility> UpdateFlexibilityAsync(Flexibility flexibility);
    Task<List<Flexibility>> GetAllFlexibilitiesAsync();

    #endregion
}
