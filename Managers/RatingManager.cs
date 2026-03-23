using System.Collections.Generic;
using System.Linq;
using ArribaEats.Models;

namespace ArribaEats.Managers
{
    /// <summary>
    /// Manages restaurant ratings and reviews.
    /// Implements the Singleton pattern to ensure a single instance.
    /// </summary>
    public class RatingManager
    {
        /// <summary>
        /// Gets the singleton instance of RatingManager.
        /// </summary>
        private static RatingManager _instance;
        public static RatingManager Instance => _instance ??= new RatingManager();

        /// <summary>
        /// Gets the list of all submitted ratings.
        /// </summary>
        private readonly List<Rating> ratings = new();

        /// <summary>
        /// Adds a new rating and updates the restaurant's rating display.
        /// </summary>
        /// <param name="rating">The rating to add.</param>
        public void AddRating(Rating rating)
        {
            ratings.Add(rating);
            UserManager.Instance.UpdateAllRatingDisplays();
        }

        /// <summary>
        /// Retrieves all ratings for a specific restaurant.
        /// </summary>
        /// <param name="restaurantName">The name of the restaurant.</param>
        /// <returns>A list of all ratings for the specified restaurant.</returns>
        public List<Rating> GetRatingsForRestaurant(string restaurantName) =>
            ratings.Where(r => r.RestaurantName == restaurantName).ToList();
    }
}
