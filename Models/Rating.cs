namespace ArribaEats.Models
{
    public class Rating
    {
        public int OrderNumber { get; set; } // Associated order ID
        public string RestaurantName { get; set; } // Rated restaurant name
        public string CustomerEmail { get; set; } // Email of the customer who gave the rating
        public int Stars { get; set; } // Rating score (1-5)
        public string Comment { get; set; } // Optional comment
        public DateTime Date { get; set; } // Timestamp of when the rating was given
    }
}
