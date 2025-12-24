using System;
using System.Collections.Generic;
using System.Text;

namespace Friterie.Shared.Models
{

        public class Cart
        {
            public List<CartItem> Items { get; set; } = new();
            public decimal Total => Items.Sum(i => i.Product.Price * i.Quantity);
        }
    
}
