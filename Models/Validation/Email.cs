using System;
using System.Text.RegularExpressions;

namespace ArribaEats.Models.Validation
{
    /// <summary>
    // Represents and validates email addresses.
    /// </summary>
    public class Email
    {
        /// <summary>
        /// Gets the email address value.
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// Initializes a new instance of the Email class.
        /// </summary>
        /// <param name="address">The email address to validate.</param>
        /// <exception cref="ArgumentException">Thrown when the email address is invalid.</exception>
        public Email(string address)
        {
            if (!IsValid(address))
                throw new ArgumentException("Invalid email address format.");
            Value = address;
        }

        /// <summary>
        /// Validates whether a string is a valid email address.
        /// </summary>
        /// <param name="email">The email address to validate.</param>
        /// <returns>True if valid, false otherwise.</returns>
        public static bool IsValid(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public override string ToString() => Value;
    }
}
