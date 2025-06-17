using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService
{
    public class OrderMessage
    {
        public string Email { get; set; }
        public List<OrderItem> Items { get; set; }
        public decimal Total { get; set; }
    }

    public class OrderItem
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
    }
}
