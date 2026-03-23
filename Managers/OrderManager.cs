using System.Collections.Generic;
using System.Linq;
using ArribaEats.Models;

namespace ArribaEats.Managers
{
    // Manages all orders in the Arriba Eats system.
    // Implements the Singleton pattern to ensure a single instance.
    public class OrderManager
    {
        // Gets the singleton instance of OrderManager.
        private static OrderManager _instance;
        public static OrderManager Instance => _instance ??= new OrderManager();

        // Gets the list of all orders.
        private readonly List<Order> orders = new();
        public List<Order> Orders => orders;

        // Gets all orders placed by a specific customer.
        public List<Order> GetOrdersForUser(string customerEmail) =>
            orders.Where(o => o.CustomerEmail == customerEmail).ToList();

        /// Adds a new order to the system.
        public void AddOrder(Order order) => orders.Add(order);
    }
}
