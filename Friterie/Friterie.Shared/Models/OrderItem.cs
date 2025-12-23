using static Friterie.Shared.Models.EnumFriterie;

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
        public int OiTypeProductId { get; set; }


        public string TypeProductName
        {
            get
            {
                return OiTypeProductId switch
                {
                    0 => "All",
                    1 => "Burgers",
                    2 => "Viandes",
                    3 => "Sauces",
                    4 => "Menus",
                    _ => "Unknown"
                };


            }
        }

    }

}
