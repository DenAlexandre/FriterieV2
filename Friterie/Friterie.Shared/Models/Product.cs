namespace Friterie.Shared.Models
{

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TypeProduct TypeProduct { get; set; } = new TypeProduct();
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public int Stock { get; set; }
        public bool IsAvailable { get; set; } = true;
    }


    public class TypeProduct
    {
        public int TypeProductCode { get; set; }
        public string TypeProductNom { get; set; } = string.Empty;
        public bool Selected { get; set; }
    }
}
