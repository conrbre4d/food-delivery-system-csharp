using System.Collections.Generic;
using System.Linq;
using ArribaEats.Models;

namespace ArribaEats.Managers
{
    /// <summary>
    /// Manages all orders in the Arriba Eats system.
    /// Implements the Singleton pattern to ensure a single instance.
    /// </summary>
    public class OrderManager
    {
        /// <summary>
        /// Gets the singleton instance of OrderManager.
        /// </summary>
        private static OrderManager _instance;
        public static OrderManager Instance => _instance ??= new OrderManager();

        /// <summary>
        /// Gets the list of all orders.
        /// </summary>
        private readonly List<Order> orders = new();
        public List<Order> Orders => orders;

        /// <summary>
        /// Gets all orders placed by a specific customer.
        /// </summary>
        /// <param name="customerEmail">The email address of the customer.</param>
        /// <returns>A list of all orders placed by the customer.</returns>
        public List<Order> GetOrdersForUser(string customerEmail) =>
            orders.Where(o => o.CustomerEmail == customerEmail).ToList();

        /// <summary>
        /// Adds a new order to the system.
        /// </summary>
        /// <param name="order">The order to add.</param>
        public void AddOrder(Order order) => orders.Add(order);
    }
}
