using System;

namespace ArribaEats.Models.Validation
{
    // Represents and validates vehicle license plates.
    public class LicensePlate
    {
        public string Value { get; } // Gets the license plate value.

        // Initializes a new instance of the LicensePlate class.
        public LicensePlate(string plate)
        {
            if (!IsValid(plate))
                throw new ArgumentException("Invalid license plate format.");
            Value = plate;
        }

        // Validates whether a string is a valid license plate.
        // Accepts any non-empty string.
        public static bool IsValid(string plate)
        {
            return !string.IsNullOrWhiteSpace(plate);
        }

        public override string ToString() => Value;
    }
}
