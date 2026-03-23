using System.Collections.Generic;
using System.Linq;
using ArribaEats.Models;
using ArribaEats.Models.Validation;

namespace ArribaEats.Managers
{
    /// <summary>
    /// Manages all user operations including registration, authentication, and user data.
    /// Implements the Singleton pattern to ensure a single instance.
    /// </summary>
    public class UserManager
    {
        /// <summary>
        /// Gets the singleton instance of UserManager.
        /// </summary>
        private static UserManager _instance;
        public static UserManager Instance => _instance ??= new UserManager();

        private readonly List<User> users = new();
        
        /// <summary>
        /// Gets the list of all registered users.
        /// </summary>
        public List<User> Users => users;

        /// <summary>
        /// Gets or sets the currently logged-in user.
        /// </summary>
        public User CurrentUser { get; set; }

        /// <summary>
        /// Registers a new customer user.
        /// </summary>
        /// <param name="name">The customer's full name.</param>
        /// <param name="age">The customer's age.</param>
        /// <param name="email">The customer's email address.</param>
        /// <param name="phone">The customer's phone number.</param>
        /// <param name="password">The customer's password.</param>
        /// <param name="location">The customer's location.</param>
        /// <returns>The newly created Customer user.</returns>
        public Customer RegisterCustomer(string name, int age, string email, string phone, string password, Location location)
        {
            var customer = new Customer(name, age, email, phone, password, location);
            users.Add(customer);
            return customer;
        }

        /// <summary>
        /// Registers a new deliverer user.
        /// </summary>
        /// <param name="name">The deliverer's full name.</param>
        /// <param name="age">The deliverer's age.</param>
        /// <param name="email">The deliverer's email address.</param>
        /// <param name="phone">The deliverer's phone number.</param>
        /// <param name="password">The deliverer's password.</param>
        /// <param name="licensePlate">The deliverer's vehicle license plate.</param>
        /// <returns>The newly created Deliverer user.</returns>
        public Deliverer RegisterDeliverer(string name, int age, string email, string phone, string password, LicensePlate licensePlate)
        {
            var deliverer = new Deliverer(name, age, email, phone, password, licensePlate);
            users.Add(deliverer);
            return deliverer;
        }

        /// <summary>
        /// Registers a new client (restaurant) user.
        /// </summary>
        /// <param name="name">The restaurant owner's full name.</param>
        /// <param name="age">The restaurant owner's age.</param>
        /// <param name="email">The restaurant owner's email address.</param>
        /// <param name="phone">The restaurant owner's phone number.</param>
        /// <param name="password">The restaurant owner's password.</param>
        /// <param name="restaurantName">The name of the restaurant.</param>
        /// <param name="restaurantStyle">The cuisine style of the restaurant.</param>
        /// <param name="location">The restaurant's location.</param>
        /// <returns>The newly created Client user.</returns>
        public Client RegisterClient(string name, int age, string email, string phone, string password, 
                                    string restaurantName, string restaurantStyle, Location location)
        {
            var client = new Client(name, age, email, phone, password, restaurantName, restaurantStyle, location);
            users.Add(client);
            return client;
        }

        /// <summary>
        /// Checks whether an email address is already registered.
        /// </summary>
        /// <param name="email">The email address to check.</param>
        /// <returns>True if the email is already in use, false otherwise.</returns>
        public bool EmailExists(string email) => users.Any(u => u.Email == email);

        /// <summary>
        /// Authenticates a user by email and password.
        /// </summary>
        /// <param name="email">The user's email address.</param>
        /// <param name="password">The user's password.</param>
        /// <returns>The authenticated user, or null if credentials are invalid.</returns>
        public User Authenticate(string email, string password)
        {
            var user = users.FirstOrDefault(u => u.Email == email && u.Password == password);
            if (user != null)
            {
                CurrentUser = user;
            }
            return user;
        }

        /// <summary>
        /// Gets all client (restaurant) users sorted alphabetically by restaurant name.
        /// </summary>
        /// <returns>A list of all client users sorted by restaurant name.</returns>
        public List<Client> GetSortedClients() =>
            users.OfType<Client>().OrderBy(c => c.RestaurantName).ToList();

        /// <summary>
        /// Mapping of each deliverer to their currently assigned order.
        /// </summary>
        private readonly Dictionary<string, Order> delivererCurrentOrders = new();

        /// <summary>
        /// Sets the current order for a deliverer.
        /// </summary>
        /// <param name="delivererEmail">The deliverer's email address.</param>
        /// <param name="order">The order to assign, or null to clear the current order.</param>
        public void SetCurrentOrderForDeliverer(string delivererEmail, Order order)
        {
            if (order == null)
                delivererCurrentOrders.Remove(delivererEmail);
            else
                delivererCurrentOrders[delivererEmail] = order;
        }

        /// <summary>
        /// Gets the current order assigned to a deliverer.
        /// </summary>
        /// <param name="delivererEmail">The deliverer's email address.</param>
        /// <returns>The current order, or null if no order is assigned.</returns>
        public Order GetCurrentOrderForDeliverer(string delivererEmail) =>
            delivererCurrentOrders.TryGetValue(delivererEmail, out var order) ? order : null;

        /// <summary>
        /// Updates the rating displays for all restaurants based on their submitted ratings.
        /// </summary>
        public void UpdateAllRatingDisplays()
        {
            foreach (var client in users.OfType<Client>())
            {
                var ratings = RatingManager.Instance.GetRatingsForRestaurant(client.RestaurantName);
                if (ratings.Any())
                {
                    double avg = ratings.Average(r => r.Stars);
                    client.RestaurantRating = avg;
                    client.RatingDisplay = avg.ToString("0.0");
                }
                else
                {
                    client.RestaurantRating = -1.0;
                    client.RatingDisplay = "-";
                }
            }
        }
    }
}
