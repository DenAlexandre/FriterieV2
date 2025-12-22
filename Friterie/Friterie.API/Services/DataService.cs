namespace Friterie.API.Services;

 
using Friterie.Shared.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

public class DataService
{
    private readonly ConcurrentDictionary<int, User> _users = new();
    private readonly ConcurrentDictionary<int, Product> _products = new();
    //private readonly ConcurrentDictionary<int, Order> _orders = new();

    private int _userIdCounter = 1;
    private int _productIdCounter = 1;
    private int _orderIdCounter = 1;

    public void InitializeData()
    {
        // Initialiser les produits
        AddProduct(new Product
        {
            Id = _productIdCounter++,
            Name = "Burger Classic",
            Description = "Burger avec steak haché, salade, tomate, oignon",
            //Category = "Burgers",
            Price = 8.50m,
            ImageUrl = "/images/burger-classic.jpg",
            Stock = 50,
            IsAvailable = true
        });

        AddProduct(new Product
        {
            Id = _productIdCounter++,
            Name = "Burger Bacon",
            Description = "Burger avec bacon croustillant, fromage cheddar",
            //Category = "Burgers",
            Price = 9.50m,
            ImageUrl = "/images/burger-bacon.jpg",
            Stock = 45,
            IsAvailable = true
        });

        AddProduct(new Product
        {
            Id = _productIdCounter++,
            Name = "Burger Cheese",
            Description = "Triple fromage fondu, cornichons",
            //Category = "Burgers",
            Price = 9.00m,
            ImageUrl = "/images/burger-cheese.jpg",
            Stock = 40,
            IsAvailable = true
        });

        AddProduct(new Product
        {
            Id = _productIdCounter++,
            Name = "Sauce Ketchup",
            Description = "Sauce tomate sucrée",
            //Category = "Sauces",
            Price = 0.50m,
            ImageUrl = "/images/ketchup.jpg",
            Stock = 200,
            IsAvailable = true
        });

        AddProduct(new Product
        {
            Id = _productIdCounter++,
            Name = "Sauce Mayo",
            Description = "Mayonnaise maison",
            //Category = "Sauces",
            Price = 0.50m,
            ImageUrl = "/images/mayo.jpg",
            Stock = 200,
            IsAvailable = true
        });

        AddProduct(new Product
        {
            Id = _productIdCounter++,
            Name = "Sauce Andalouse",
            Description = "Sauce épicée belge",
            //Category = "Sauces",
            Price = 0.70m,
            ImageUrl = "/images/andalouse.jpg",
            Stock = 150,
            IsAvailable = true
        });

        AddProduct(new Product
        {
            Id = _productIdCounter++,
            Name = "Menu Classic",
            Description = "Burger + Frites + Boisson",
            //Category = "Menus",
            Price = 12.00m,
            ImageUrl = "/images/menu-classic.jpg",
            Stock = 30,
            IsAvailable = true
        });

        AddProduct(new Product
        {
            Id = _productIdCounter++,
            Name = "Menu XL",
            Description = "Double Burger + Grandes Frites + Boisson + Dessert",
            //Category = "Menus",
            Price = 15.00m,
            ImageUrl = "/images/menu-xl.jpg",
            Stock = 25,
            IsAvailable = true
        });
    }

    // Users
    public User? GetUserByEmail(string email) =>
        _users.Values.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

    public User? GetUserById(int id) => _users.GetValueOrDefault(id);

    public User AddUser(User user)
    {
        user.UserId = _userIdCounter++;
        _users[user.UserId] = user;
        return user;
    }

    // Products
    public List<Product> GetAllProducts() => _products.Values.ToList();

    public Product? GetProductById(int id) => _products.GetValueOrDefault(id);

    public void AddProduct(Product product) => _products[product.Id] = product;

    public bool UpdateProductStock(int productId, int quantity)
    {
        if (_products.TryGetValue(productId, out var product))
        {
            product.Stock -= quantity;
            return true;
        }
        return false;
    }

    // Orders
    //public Order AddOrder(Order order)
    //{
    //    order.Id = _orderIdCounter++;
    //    _orders[order.Id] = order;
    //    return order;
    //}

    //public Order? GetOrderById(int id) => _orders.GetValueOrDefault(id);

    //public List<Order> GetUserOrders(int userId) =>
    //    _orders.Values.Where(o => o.UserId == userId).OrderByDescending(o => o.OrderDate).ToList();

    //public bool UpdateOrderPaymentStatus(int orderId, bool isPaid, string paymentIntentId)
    //{
    //    if (_orders.TryGetValue(orderId, out var order))
    //    {
    //        order.IsPaid = isPaid;
    //        order.Status = isPaid ? "Paid" : "Pending";
    //        order.PaymentIntentId = paymentIntentId;
    //        return true;
    //    }
    //    return false;
    //}
}