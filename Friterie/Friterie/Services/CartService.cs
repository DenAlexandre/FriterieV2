namespace Friterie.BlazorServer.Services;

using Friterie.Shared.Models;

public class CartService
{
    private List<CartItem> _items = new();

    public event Action? OnChange;

    public List<CartItem> GetItems() => _items;

    public void AddItem(Product product, int quantity = 1)
    {
         var existingItem = _items.FirstOrDefault(i => i.ProductId == product.Id);

        if (existingItem != null)
        {
            existingItem.Quantity += quantity;
        }
        else
        {
            _items.Add(new CartItem
            {
                ProductId = product.Id,
                ProductName = product.Name,
                Price = product.Price,
                Quantity = quantity,
                ImageUrl = product.ImageUrl
            });
        }

        NotifyStateChanged();
    }

    public void UpdateQuantity(int productId, int quantity)
    {
        var item = _items.FirstOrDefault(i => i.ProductId == productId);
        if (item != null)
        {
            if (quantity <= 0)
            {
                _items.Remove(item);
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
        var item = _items.FirstOrDefault(i => i.ProductId == productId);
        if (item != null)
        {
            _items.Remove(item);
            NotifyStateChanged();
        }
    }

    public void Clear()
    {
        _items.Clear();
        NotifyStateChanged();
    }

    public decimal GetTotal() => _items.Sum(i => i.Price * i.Quantity);

    public int GetItemCount() => _items.Sum(i => i.Quantity);

    private void NotifyStateChanged() => OnChange?.Invoke();
}