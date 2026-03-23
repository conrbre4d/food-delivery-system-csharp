namespace ArribaEats.Models
{
    // Abstract base class representing a user in the Arriba Eats system.
    public abstract class User
    {
        // Gets the user's full name.
        public string Name { get; }

        // Gets the user's age.
        public int Age { get; }

        // Gets the user's email address.
        public string Email { get; }

        // Gets the user's phone number.
        public string Phone { get; }

        // Gets the user's password.
        public string Password { get; }

        // Initializes a new instance of the User class.
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
