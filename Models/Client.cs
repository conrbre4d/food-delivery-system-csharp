using System.Collections.Generic;

namespace ArribaEats.Models
{
    // Represents a client (restaurant) user in the Arriba Eats system.
    // Clients manage their restaurant menu, orders, and cooking status.
    public class Client : User
    {
        public string RestaurantName { get; } // Gets the restaurant's name.
        public string RestaurantStyle { get; } // Gets the restaurant's cuisine style.
        public Location Location { get; } // Gets the restaurant's location.
        public List<MenuItem> MenuItems { get; } // Gets the restaurant's menu items.
        public double RestaurantRating { get; set; } // Gets the restaurant's average rating.
        public string RatingDisplay { get; set; } // Gets the restaurant's rating display string.

        // Initializes a new instance of the Client class.
        public Client(string name, int age, string email, string phone, string password, 
                     string restaurantName, string restaurantStyle, Location location)
            : base(name, age, email, phone, password)
        {
            RestaurantName = restaurantName;
            RestaurantStyle = restaurantStyle;
            Location = location;
            MenuItems = new List<MenuItem>();
            RestaurantRating = 0.0;
            RatingDisplay = "-";
        }
    }
}
