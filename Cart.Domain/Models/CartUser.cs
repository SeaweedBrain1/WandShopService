using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cart.Domain.Models;

public class CartUser
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public bool Deleted { get; set; } = false;

    public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
}
