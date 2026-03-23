using System;
using System.Linq;

namespace ArribaEats.Models.Validation
{
    /// <summary>
    /// Represents and validates passwords.
    /// </summary>
    public class Password
    {
        /// <summary>
        /// Gets the password value.
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// Initializes a new instance of the Password class.
        /// </summary>
        /// <param name="password">The password to validate.</param>
        /// <exception cref="ArgumentException">Thrown when the password does not meet requirements.</exception>
        public Password(string password)
        {
            if (!IsValid(password))
                throw new ArgumentException("Password does not meet requirements.");
            Value = password;
        }

        /// <summary>
        /// Validates whether a password meets requirements:
        /// - At least 8 characters long
        /// - Contains at least one digit
        /// - Contains at least one lowercase letter
        /// - Contains at least one uppercase letter
        /// </summary>
        /// <param name="password">The password to validate.</param>
        /// <returns>True if valid, false otherwise.</returns>
        public static bool IsValid(string password)
        {
            if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
                return false;

            bool hasDigit = password.Any(char.IsDigit);
            bool hasLower = password.Any(char.IsLower);
            bool hasUpper = password.Any(char.IsUpper);

            return hasDigit && hasLower && hasUpper;
        }

        /// <summary>
        /// Displays the password requirements.
        /// </summary>
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
