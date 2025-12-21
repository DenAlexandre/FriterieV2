using System;

namespace Friterie.API.Models
{
    public class Orders
    {
        public int OrderId { get; set; }
        public int OrderUserId { get; set; }
        public DateTime OrderDatetime { get; set; }
        public decimal OrderTotal { get; set; }
        public int OrderStatus { get; set; }
        public string OrderIntentId { get; set; }
        public bool OrderIsPaid { get; set; }
    }
}
