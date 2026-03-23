using static System.Console;
using ArribaEats.Models;
using ArribaEats.Models.Validation;

namespace ArribaEats.Utilities
{
    /// <summary>
    /// Provides utility methods for displaying common text patterns in the user interface.
    /// </summary>
    public class TextPrinter
    {
        /// <summary>
        /// Displays base user information (name, age, email, phone).
        /// </summary>
        /// <param name="user">The user whose information to display.</param>
        public static void DisplayBaseUserInfo(User user)
        {
            WriteLine("Your user details are as follows:");
            WriteLine($"Name: {user.Name}");
            WriteLine($"Age: {user.Age}");
            WriteLine($"Email: {user.Email}");
            WriteLine($"Mobile: {user.Phone}");
        }

        /// <summary>
        /// Displays password requirements to the user.
        /// </summary>
        public static void PasswordSpecifications()
        {
            Password.DisplayRequirements();
        }
    }
}
