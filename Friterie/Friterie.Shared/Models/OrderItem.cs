namespace Friterie.Shared.Models
{

    public class OrderItem
    {
        public int OiId { get; set; }
        public int OiProductId { get; set; }
        public string OiProductName { get; set; }
        public int OiQuantity { get; set; }
        public decimal OiPrice { get; set; }
        public int OiOrderId { get; set; }
    }

}
