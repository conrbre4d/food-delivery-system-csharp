using ArribaEats.Models.Validation;

namespace ArribaEats.Models
{
    // Represents a deliverer user in the Arriba Eats system.
    // Deliverers accept delivery orders and manage order pickups and deliveries.
    public class Deliverer : User
    {
        public LicensePlate LicensePlate { get; } // Gets the deliverer's vehicle license plate.

        // Initializes a new instance of the Deliverer class.
        public Deliverer(string name, int age, string email, string phone, string password, LicensePlate licensePlate)
            : base(name, age, email, phone, password)
        {
            LicensePlate = licensePlate;
        }
    }
}
