using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cart.Application.Clients;

public interface IWandServiceClient
{
    Task<bool> IsWandValidAsync(int wandId);
}

