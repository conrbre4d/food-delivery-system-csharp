namespace ArribaEats.Models
{
    /// <summary>
    /// Abstract base class representing a user in the Arriba Eats system.
    /// </summary>
    public abstract class User
    {
        /// <summary>
        /// Gets the user's full name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the user's age.
        /// </summary>
        public int Age { get; }

        /// <summary>
        /// Gets the user's email address.
        /// </summary>
        public string Email { get; }

        /// <summary>
        /// Gets the user's phone number.
        /// </summary>
        public string Phone { get; }

        /// <summary>
        /// Gets the user's password.
        /// </summary>
        public string Password { get; }

        /// <summary>
        /// Initializes a new instance of the User class.
        /// </summary>
        /// <param name="name">The user's full name.</param>
        /// <param name="age">The user's age.</param>
        /// <param name="email">The user's email address.</param>
        /// <param name="phone">The user's phone number.</param>
        /// <param name="password">The user's password.</param>
        protected User(string name, int age, string email, string phone, string password)
        {
            Name = name;
            Age = age;
            Email = email;
            Phone = phone;
            Password = password;
        }
    }
}
