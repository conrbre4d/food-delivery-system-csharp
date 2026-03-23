using System.Collections.Generic;

namespace ArribaEats.Models
{
    /// <summary>
    /// Represents a client (restaurant) user in the Arriba Eats system.
    /// Clients manage their restaurant menu, orders, and cooking status.
    /// </summary>
    public class Client : User
    {
        /// <summary>
        /// Gets the restaurant's name.
        /// </summary>
        public string RestaurantName { get; }

        /// <summary>
        /// Gets the restaurant's cuisine style.
        /// </summary>
        public string RestaurantStyle { get; }

        /// <summary>
        /// Gets the restaurant's location.
        /// </summary>
        public Location Location { get; }

        /// <summary>
        /// Gets the restaurant's menu items.
        /// </summary>
        public List<MenuItem> MenuItems { get; }

        /// <summary>
        /// Gets the restaurant's average rating.
        /// </summary>
        public double RestaurantRating { get; set; }

        /// <summary>
        /// Gets the restaurant's rating display string.
        /// </summary>
        public string RatingDisplay { get; set; }

        /// <summary>
        /// Initializes a new instance of the Client class.
        /// </summary>
        /// <param name="name">The restaurant owner's full name.</param>
        /// <param name="age">The restaurant owner's age.</param>
        /// <param name="email">The restaurant owner's email address.</param>
        /// <param name="phone">The restaurant owner's phone number.</param>
        /// <param name="password">The restaurant owner's password.</param>
        /// <param name="restaurantName">The name of the restaurant.</param>
        /// <param name="restaurantStyle">The cuisine style of the restaurant.</param>
        /// <param name="location">The restaurant's location.</param>
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
