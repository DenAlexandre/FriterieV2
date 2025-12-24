namespace Friterie.BlazorServer.Services;

using Friterie.Shared.Models;

public class CartService
{
    public Cart Cart { get; } = new();


    public event Action? OnChange;

    public List<CartItem> GetItems() => Cart.Items;

    public void AddItem(Product product, int quantity = 1)
    {
        var existingItem = Cart.Items.FirstOrDefault(i => i.Product.Id == product.Id);

        if (existingItem != null)
        {
            existingItem.Quantity += quantity;
        }
        else
        {
            Cart.Items.Add(new CartItem
            {
                Product = new Product
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    TypeProduct = product.TypeProduct,
                    Price = product.Price,
                    ImageUrl = product.ImageUrl,
                    Stock = product.Stock,
                    IsAvailable = product.IsAvailable


                },
                Quantity = quantity,
            });

        }

        NotifyStateChanged();
    }

    public void UpdateQuantity(int productId, int quantity)
    {
        var item = Cart.Items.FirstOrDefault(i => i.Product.Id == productId);
        if (item != null)
        {
            if (quantity <= 0)
            {
                Cart.Items.Remove(item);
            }
            else
            {
                item.Quantity = quantity;
            }
            NotifyStateChanged();
        }
    }

    public void RemoveItem(int productId)
    {
        var item = Cart.Items.FirstOrDefault(i => i.Product.Id == productId);
        if (item != null)
        {
            Cart.Items.Remove(item);
            NotifyStateChanged();
        }
    }

    public void Clear()
    {
        Cart.Items.Clear();
        NotifyStateChanged();
    }

    public decimal GetTotal() => Cart.Items.Sum(i => i.Product.Price * i.Quantity);

    public int GetItemCount() => Cart.Items.Sum(i => i.Quantity);

    private void NotifyStateChanged() => OnChange?.Invoke();
}