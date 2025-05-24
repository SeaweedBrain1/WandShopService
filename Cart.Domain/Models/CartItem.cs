using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Cart.Domain.Models;

public class CartItem
{
    public int Id { get; set; }

    [JsonIgnore]
    public CartUser CartUser { get; set; } = null!;

    public int WandId { get; set; }

    public int Quantity { get; set; } = 1;

}
