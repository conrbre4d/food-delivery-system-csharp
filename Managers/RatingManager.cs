using System.Collections.Generic;
using System.Linq;
using ArribaEats.Models;

namespace ArribaEats.Managers
{
    // Manages restaurant ratings and reviews.
    // Implements the Singleton pattern to ensure a single instance.
    public class RatingManager
    {
        // Gets the singleton instance of RatingManager.
        private static RatingManager _instance;
        public static RatingManager Instance => _instance ??= new RatingManager();

        // Gets the list of all submitted ratings.s
        private readonly List<Rating> ratings = new();

        // Adds a new rating and updates the restaurant's rating display.
        public void AddRating(Rating rating)
        {
            ratings.Add(rating);
            UserManager.Instance.UpdateAllRatingDisplays();
        }

        // Retrieves all ratings for a specific restaurant.
        public List<Rating> GetRatingsForRestaurant(string restaurantName) =>
            ratings.Where(r => r.RestaurantName == restaurantName).ToList();
    }
}
