using System;
using System.Linq;

namespace ArribaEats.Models.Validation
{
    // Represents and validates passwords.
    public class Password
    {
        public string Value { get; } // Gets the password value.

        // Initializes a new instance of the Password class.
        public Password(string password)
        {
            if (!IsValid(password))
                throw new ArgumentException("Password does not meet requirements.");
            Value = password;
        }

        // Validates whether a password meets requirements:
        // - At least 8 characters long
        // - Contains at least one digit
        // - Contains at least one lowercase letter
        // - Contains at least one uppercase letter
        public static bool IsValid(string password)
        {
            if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
                return false;

            bool hasDigit = password.Any(char.IsDigit);
            bool hasLower = password.Any(char.IsLower);
            bool hasUpper = password.Any(char.IsUpper);

            return hasDigit && hasLower && hasUpper;
        }

        // Displays the password requirements.
        public static void DisplayRequirements()
        {
            Console.WriteLine("Your password must:");
            Console.WriteLine("- be at least 8 characters long");
            Console.WriteLine("- contain a number");
            Console.WriteLine("- contain a lowercase letter");
            Console.WriteLine("- contain an uppercase letter");
        }

        public override string ToString() => Value;
    }
}
