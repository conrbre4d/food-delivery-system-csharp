namespace ArribaEats.Models
{
    // Represents an item within a specific order.
    public class OrderItem
    {
        public string Name { get; set; } // The name of the ordered item
        public int Quantity { get; set; } // The number of units ordered
        public decimal Price { get; set; } // The price per unit of the item

        // Initializes a new OrderItem with a name, quantity, and price.
        public OrderItem(string name, int qty, decimal price)
        {
            Name = name;
            Quantity = qty;
            Price = price;
        }
    }
}