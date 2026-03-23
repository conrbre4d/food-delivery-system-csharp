using System.Collections.Generic;

namespace ArribaEats.Models
{
    // Represents a customer user in the Arriba Eats system.
    // Customers can browse restaurants, place orders, and rate them.
    public class Customer : User
    {

        public Location Location { get; } // Gets the customer's location.

        // Initializes a new instance of the Customer class.
        public Customer(string name, int age, string email, string phone, string password, Location location)
            : base(name, age, email, phone, password)
        {
            Location = location;
        }
    }
}
