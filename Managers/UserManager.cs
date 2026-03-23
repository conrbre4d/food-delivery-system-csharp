using System.Collections.Generic;
using System.Linq;
using ArribaEats.Models;
using ArribaEats.Models.Validation;

namespace ArribaEats.Managers
{
    // Manages all user operations including registration, authentication, and user data.
    // Implements the Singleton pattern to ensure a single instance.
    public class UserManager
    {
        // Gets the singleton instance of UserManager.
        private static UserManager _instance;
        public static UserManager Instance => _instance ??= new UserManager();

        private readonly List<User> users = new();
        
        // Gets the list of all registered users.
        public List<User> Users => users;

        // Gets or sets the currently logged-in user.
        public User CurrentUser { get; set; }

        // Registers a new customer user.
        public Customer RegisterCustomer(string name, int age, string email, string phone, string password, Location location)
        {
            var customer = new Customer(name, age, email, phone, password, location);
            users.Add(customer);
            return customer;
        }

        // Registers a new deliverer user.
        public Deliverer RegisterDeliverer(string name, int age, string email, string phone, string password, LicensePlate licensePlate)
        {
            var deliverer = new Deliverer(name, age, email, phone, password, licensePlate);
            users.Add(deliverer);
            return deliverer;
        }

        // Registers a new client (restaurant) user.
        public Client RegisterClient(string name, int age, string email, string phone, string password, 
                                    string restaurantName, string restaurantStyle, Location location)
        {
            var client = new Client(name, age, email, phone, password, restaurantName, restaurantStyle, location);
            users.Add(client);
            return client;
        }

        // Checks whether an email address is already registered.
        public bool EmailExists(string email) => users.Any(u => u.Email == email);

        // Authenticates a user by email and password.
        public User Authenticate(string email, string password)
        {
            var user = users.FirstOrDefault(u => u.Email == email && u.Password == password);
            if (user != null)
            {
                CurrentUser = user;
            }
            return user;
        }

        // Gets all client (restaurant) users sorted alphabetically by restaurant name.
        public List<Client> GetSortedClients() =>
            users.OfType<Client>().OrderBy(c => c.RestaurantName).ToList();

        // Mapping of each deliverer to their currently assigned order.
        private readonly Dictionary<string, Order> delivererCurrentOrders = new();

        // Sets the current order for a deliverer.
        public void SetCurrentOrderForDeliverer(string delivererEmail, Order order)
        {
            if (order == null)
                delivererCurrentOrders.Remove(delivererEmail);
            else
                delivererCurrentOrders[delivererEmail] = order;
        }

        // Gets the current order assigned to a deliverer.
        public Order GetCurrentOrderForDeliverer(string delivererEmail) =>
            delivererCurrentOrders.TryGetValue(delivererEmail, out var order) ? order : null;

        // Updates the rating displays for all restaurants based on their submitted ratings.
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
