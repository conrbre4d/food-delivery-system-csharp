using static System.Console;
using ArribaEats.Models;
using ArribaEats.Models.Validation;

namespace ArribaEats.Utilities
{
    // Provides utility methods for displaying common text patterns in the user interface.
    public class TextPrinter
    {
        // Displays base user information (name, age, email, phone).
        public static void DisplayBaseUserInfo(User user)
        {
            WriteLine("Your user details are as follows:");
            WriteLine($"Name: {user.Name}");
            WriteLine($"Age: {user.Age}");
            WriteLine($"Email: {user.Email}");
            WriteLine($"Mobile: {user.Phone}");
        }

        // Displays password requirements to the user.
        public static void PasswordSpecifications()
        {
            Password.DisplayRequirements();
        }
    }
}
