using System;

namespace ArribaEats.Models
{
    // Represents a geographic location with coordinates and distance calculations.
    public class Location
    {
        public int X { get; } // Gets the X coordinate.
        public int Y { get; } // Gets the Y coordinate.

        // Initializes a new instance of the Location class.
        public Location(int x, int y)
        {
            X = x;
            Y = y;
        }

        // Calculates the Manhattan distance between this location and another location.
        public int DistanceTo(Location other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            return Math.Abs(X - other.X) + Math.Abs(Y - other.Y);
        }

        public override string ToString() => $"{X},{Y}";
    }
}
