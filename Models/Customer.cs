using System.Collections.Generic;

namespace ArribaEats.Models
{
    /// <summary>
    /// Represents a customer user in the Arriba Eats system.
    /// Customers can browse restaurants, place orders, and rate them.
    /// </summary>
    public class Customer : User
    {
        /// <summary>
        /// Gets the customer's location.
        /// </summary>
        public Location Location { get; }

        /// <summary>
        /// Initializes a new instance of the Customer class.
        /// </summary>
        /// <param name="name">The customer's full name.</param>
        /// <param name="age">The customer's age.</param>
        /// <param name="email">The customer's email address.</param>
        /// <param name="phone">The customer's phone number.</param>
        /// <param name="password">The customer's password.</param>
        /// <param name="location">The customer's location.</param>
        public Customer(string name, int age, string email, string phone, string password, Location location)
            : base(name, age, email, phone, password)
        {
            Location = location;
        }
    }
}
