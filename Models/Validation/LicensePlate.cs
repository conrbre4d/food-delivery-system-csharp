using System;

namespace ArribaEats.Models.Validation
{
    /// <summary>
    /// Represents and validates vehicle license plates.
    /// </summary>
    public class LicensePlate
    {
        /// <summary>
        /// Gets the license plate value.
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// Initializes a new instance of the LicensePlate class.
        /// </summary>
        /// <param name="plate">The license plate to validate.</param>
        /// <exception cref="ArgumentException">Thrown when the license plate is invalid.</exception>
        public LicensePlate(string plate)
        {
            if (!IsValid(plate))
                throw new ArgumentException("Invalid license plate format.");
            Value = plate;
        }

        /// <summary>
        /// Validates whether a string is a valid license plate.
        /// Accepts any non-empty string.
        /// </summary>
        /// <param name="plate">The license plate to validate.</param>
        /// <returns>True if valid, false otherwise.</returns>
        public static bool IsValid(string plate)
        {
            return !string.IsNullOrWhiteSpace(plate);
        }

        public override string ToString() => Value;
    }
}
