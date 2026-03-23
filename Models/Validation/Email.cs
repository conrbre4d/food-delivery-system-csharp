using System;
using System.Text.RegularExpressions;

namespace ArribaEats.Models.Validation
{
    // Represents and validates email addresses.
    public class Email
    {
        public string Value { get; } // Gets the email address value.

        // Initializes a new instance of the Email class.
        public Email(string address)
        {
            if (!IsValid(address))
                throw new ArgumentException("Invalid email address format.");
            Value = address;
        }

        // Validates whether a string is a valid email address.
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
