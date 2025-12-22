using System;

namespace Friterie.Shared.Models
{
    public class Orders
    {
        public int OrderId { get; set; }
        public int OrderUserId { get; set; }
        public DateTime OrderDatetime { get; set; }
        public decimal OrderTotal { get; set; }
        public int OrderStatus { get; set; }

        public string OrderStatusString
        {
            get
            {
                return OrderStatus switch
                {
                    0 => "Pending",
                    1 => "Paid",
                    2 => "Shipped",
                    3 => "Delivered",
                    4 => "Cancelled",
                    _ => "Unknown"
                };
            }
        }
        public string OrderIntentId { get; set; }
        public bool OrderIsPaid { get; set; }

        public List<OrderItem> Items { get; set; } = new List<OrderItem>();
    }
}
