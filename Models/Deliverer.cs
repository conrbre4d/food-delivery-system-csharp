using ArribaEats.Models.Validation;

namespace ArribaEats.Models
{
    /// <summary>
    /// Represents a deliverer user in the Arriba Eats system.
    /// Deliverers accept delivery orders and manage order pickups and deliveries.
    /// </summary>
    public class Deliverer : User
    {
        /// <summary>
        /// Gets the deliverer's vehicle license plate.
        /// </summary>
        public LicensePlate LicensePlate { get; }

        /// <summary>
        /// Initializes a new instance of the Deliverer class.
        /// </summary>
        /// <param name="name">The deliverer's full name.</param>
        /// <param name="age">The deliverer's age.</param>
        /// <param name="email">The deliverer's email address.</param>
        /// <param name="phone">The deliverer's phone number.</param>
        /// <param name="password">The deliverer's password.</param>
        /// <param name="licensePlate">The deliverer's vehicle license plate.</param>
        public Deliverer(string name, int age, string email, string phone, string password, LicensePlate licensePlate)
            : base(name, age, email, phone, password)
        {
            LicensePlate = licensePlate;
        }
    }
}
