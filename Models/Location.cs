using System;

namespace ArribaEats.Models
{
    /// <summary>
    /// Represents a geographic location with coordinates and distance calculations.
    /// </summary>
    public class Location
    {
        /// <summary>
        /// Gets the X coordinate.
        /// </summary>
        public int X { get; }

        /// <summary>
        /// Gets the Y coordinate.
        /// </summary>
        public int Y { get; }

        /// <summary>
        /// Initializes a new instance of the Location class.
        /// </summary>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        public Location(int x, int y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Calculates the Manhattan distance between this location and another location.
        /// </summary>
        /// <param name="other">The other location to calculate distance to.</param>
        /// <returns>The Manhattan distance between the two locations.</returns>
        public int DistanceTo(Location other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            return Math.Abs(X - other.X) + Math.Abs(Y - other.Y);
        }

        public override string ToString() => $"{X},{Y}";
    }
}
