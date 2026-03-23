namespace ArribaEats.Models
{
    public class Order
    {
        public int OrderNumber { get; set; } // Unique order identifier
        public string RestaurantName { get; set; } // Restaurant fulfilling the order
        public string CustomerEmail { get; set; } // Email of the customer who placed the order
        public List<OrderItem> Items { get; set; } = new(); // List of ordered items
        public bool Delivered { get; set; } = false; // Whether the order was delivered
        public string DelivererName { get; set; } // Name of the deliverer
        public string DelivererPlate { get; set; } // Licence plate of the deliverer
        public List<Rating> Ratings { get; set; } = new(); // Ratings related to this order
        public bool StartedCooking { get; set; } = false; // If cooking has started
        public bool IsReadyForPickup { get; set; } = false; // If order is ready for pickup
        public bool PickedUp { get; set; } = false; // If order has been picked up by deliverer
        public bool DelivererArrived { get; set; } = false; // If deliverer has arrived at restaurant
    }
}
