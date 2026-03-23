using System;
using System.Collections.Generic;
using System.Linq;
using static System.Console;
using ArribaEats.Models;
using ArribaEats.Managers;
using ArribaEats.Utilities;

namespace ArribaEats.UI.Menus
{
    // Provides the menu interface for customer users.
    // Allows customers to browse restaurants, place orders, check order status, and rate restaurants.
    public class CustomerMenu
    {
        // Gets the customer user.
        private Customer customer;

        // Initializes a new instance of the CustomerMenu class.
        public CustomerMenu(User user)
        {
            this.customer = user as Customer;
        }

        // Displays the customer menu.
        public void Show()
        {
            WriteLine($"Welcome back, {customer.Name}!");

            bool running = true;
            while (running)
            {
                WriteLine("Please make a choice from the menu below:");
                WriteLine("1: Display your user information");
                WriteLine("2: Select a list of restaurants to order from");
                WriteLine("3: See the status of your orders");
                WriteLine("4: Rate a restaurant you've ordered from");
                WriteLine("5: Log out");
                WriteLine("Please enter a choice between 1 and 5:");

                string input = ReadLine();

                switch (input)
                {
                    case "1":
                        DisplayUserInfo();
                        break;
                    case "2":
                        SelectRestaurants();
                        break;
                    case "3":
                        ShowOrderStatus();
                        break;
                    case "4":
                        RateRestaurant();
                        break;
                    case "5":
                        WriteLine("You are now logged out.");
                        running = false;
                        break;
                    default:
                        WriteLine("Invalid choice.");
                        break;
                }
            }
        }

        /// Displays the customer's user information and order statistics.
        private void DisplayUserInfo()
        {
            TextPrinter.DisplayBaseUserInfo(customer);
            WriteLine($"Location: {customer.Location}");

            var orders = OrderManager.Instance.GetOrdersForUser(customer.Email);
            int count = orders.Count;
            decimal total = orders.SelectMany(o => o.Items).Sum(i => i.Quantity * i.Price);

            WriteLine($"You've made {count} order(s) and spent a total of ${total:0.00} here.");
        }

        // Displays the restaurant sorting options.
        private void SelectRestaurants()
        {
            bool running = true;
            while (running)
            {
                WriteLine("How would you like the list of restaurants ordered?");
                WriteLine("1: Sorted alphabetically by name");
                WriteLine("2: Sorted by distance");
                WriteLine("3: Sorted by style");
                WriteLine("4: Sorted by average rating");
                WriteLine("5: Return to the previous menu");
                WriteLine("Please enter a choice between 1 and 5:");

                string input = ReadLine();

                try
                {
                    switch (input)
                    {
                        case "1":
                            SortAlphabetically();
                            break;
                        case "2":
                            SortDistance();
                            break;
                        case "3":
                            SortStyle();
                            break;
                        case "4":
                            SortAverageRating();
                            break;
                        case "5":
                            running = false;
                            break;
                        default:
                            WriteLine("Invalid choice.");
                            break;
                    }
                }
                catch (OperationCanceledException)
                {
                    running = false;
                }
            }
        }

        // Sorts restaurants alphabetically by name.
        private void SortAlphabetically()
        {
            var clients = UserManager.Instance.GetSortedClients();
            DisplayRestaurants(clients);
        }

        // Sorts restaurants by distance from the customer's location.
        private void SortDistance()
        {
            var clients = UserManager.Instance.GetSortedClients()
                .OrderBy(c => customer.Location.DistanceTo(c.Location))
                .ToList();

            DisplayRestaurants(clients);
        }

        // Sorts restaurants by cuisine style.
        private void SortStyle()
        {
            var styleOrder = new List<string> { "Italian", "French", "Chinese", "Japanese", "American", "Australian" };

            var clients = UserManager.Instance.GetSortedClients()
                .OrderBy(c => styleOrder.IndexOf(c.RestaurantStyle ?? ""))
                .ToList();

            DisplayRestaurants(clients);
        }

        // Sorts restaurants by average customer rating.
        private void SortAverageRating()
        {
            UserManager.Instance.UpdateAllRatingDisplays();

            var clients = UserManager.Instance.GetSortedClients()
                .OrderByDescending(c => c.RestaurantRating)
                .ToList();

            DisplayRestaurants(clients);
        }

        // Displays a list of restaurants for the customer to select from.
        private void DisplayRestaurants(List<Client> clients)
        {
            if (!clients.Any())
            {
                WriteLine("No restaurants found.");
                return;
            }

            WriteLine("\nYou can order from the following restaurants:");
            WriteLine("   Restaurant Name       Loc    Dist  Style       Rating");

            for (int i = 0; i < clients.Count; i++)
            {
                var restaurant = clients[i];
                int distance = customer.Location.DistanceTo(restaurant.Location);
                string location = restaurant.Location.ToString();
                string ratingDisplay = restaurant.RatingDisplay;

                WriteLine($"{i + 1}: {restaurant.RestaurantName,-20} {location,-6} {distance,5}  {restaurant.RestaurantStyle,-10} {ratingDisplay,6}");
            }

            WriteLine($"{clients.Count + 1}: Return to the previous menu");
            WriteLine($"Please enter a choice between 1 and {clients.Count + 1}:");

            string input = ReadLine();
            if (!int.TryParse(input, out int choice) || choice < 1 || choice > clients.Count + 1)
            {
                WriteLine("Invalid choice.");
                return;
            }

            if (choice == clients.Count + 1)
                throw new OperationCanceledException();

            var selectedRestaurant = clients[choice - 1];
            RestaurantInteractionMenu(selectedRestaurant);
        }

        // Displays the restaurant interaction menu (menu browsing and reviews).
        private void RestaurantInteractionMenu(Client restaurant)
        {
            WriteLine($"\nPlacing order from {restaurant.RestaurantName}.");

            bool stay = true;
            while (stay)
            {
                WriteLine("1: See this restaurant's menu and place an order");
                WriteLine("2: See reviews for this restaurant");
                WriteLine("3: Return to main menu");
                WriteLine("Please enter a choice between 1 and 3: ");
                string input = ReadLine();

                switch (input)
                {
                    case "1":
                        PlaceOrder(restaurant);
                        break;
                    case "2":
                        ShowRestaurantReviews(restaurant.RestaurantName);
                        break;
                    case "3":
                        throw new OperationCanceledException();
                    default:
                        WriteLine("Invalid choice.");
                        break;
                }
            }
        }

        // Handles the order placement process.
        private void PlaceOrder(Client restaurant)
        {
            decimal total = 0;
            List<OrderItem> currentItems = new();

            while (true)
            {
                WriteLine($"\nCurrent order total: {total:C2}");
                for (int i = 0; i < restaurant.MenuItems.Count; i++)
                {
                    var item = restaurant.MenuItems[i];
                    WriteLine($"{i + 1}: {item.Price,7:C2}  {item.Name}");
                }

                int completeOrderOption = restaurant.MenuItems.Count + 1;
                int cancelOrderOption = completeOrderOption + 1;

                WriteLine($"{completeOrderOption}: Complete order");
                WriteLine($"{cancelOrderOption}: Cancel order");
                WriteLine($"Please enter a choice between 1 and {cancelOrderOption}: ");
                string input = ReadLine();

                if (!int.TryParse(input, out int choice) || choice < 1 || choice > cancelOrderOption)
                {
                    WriteLine("Invalid choice.");
                    continue;
                }

                if (choice == cancelOrderOption)
                    return;

                if (choice == completeOrderOption)
                {
                    var order = new Order
                    {
                        OrderNumber = OrderManager.Instance.Orders.Count + 1,
                        RestaurantName = restaurant.RestaurantName,
                        CustomerEmail = customer.Email,
                        Items = currentItems
                    };
                    OrderManager.Instance.AddOrder(order);
                    WriteLine($"Your order has been placed. Your order number is #{order.OrderNumber}.");
                    return;
                }

                var selectedItem = restaurant.MenuItems[choice - 1];
                WriteLine($"Adding {selectedItem.Name} to order.");

                int quantity = GetOrderQuantity();
                if (quantity == 0)
                    continue;

                currentItems.Add(new OrderItem(selectedItem.Name, quantity, selectedItem.Price));
                total += selectedItem.Price * quantity;
                WriteLine($"Added {quantity} x {selectedItem.Name} to order.");
            }
        }

        // Gets a valid order quantity from the customer.
        private int GetOrderQuantity()
        {
            while (true)
            {
                WriteLine("Please enter quantity (0 to cancel): ");
                string quantityInput = ReadLine();
                if (int.TryParse(quantityInput, out int quantity) && quantity >= 0)
                {
                    return quantity;
                }
                WriteLine("Invalid quantity.");
            }
        }

        // Displays all reviews for a restaurant.
        private void ShowRestaurantReviews(string restaurantName)
        {
            var reviews = RatingManager.Instance.GetRatingsForRestaurant(restaurantName);
            if (!reviews.Any())
            {
                WriteLine("No reviews have been left for this restaurant.");
                return;
            }

            foreach (var review in reviews)
            {
                DisplayReview(review);
            }
        }

        // Displays a single review.
        private void DisplayReview(Rating review)
        {
            var reviewer = UserManager.Instance.Users.FirstOrDefault(u => u.Email == review.CustomerEmail);
            WriteLine($"Reviewer: {reviewer?.Name ?? "Unknown"}");
            WriteLine($"Rating: {new string('*', review.Stars)}");
            WriteLine($"Comment: {review.Comment}\n");
        }


        // Displays the status of all orders placed by the customer.
        private void ShowOrderStatus()
        {
            var orders = OrderManager.Instance.GetOrdersForUser(customer.Email)
                .OrderBy(o => o.OrderNumber)
                .ToList();

            if (!orders.Any())
            {
                WriteLine("You have not placed any orders.");
                return;
            }

            foreach (var order in orders)
            {
                DisplayOrderStatus(order);
            }
        }

        // Displays details for a single order.
        private void DisplayOrderStatus(Order order)
        {
            string status = GetOrderStatus(order);
            WriteLine($"Order #{order.OrderNumber} from {order.RestaurantName}: {status}");

            if (order.Delivered)
            {
                WriteLine($"This order was delivered by {order.DelivererName} (licence plate: {order.DelivererPlate})");
            }

            DisplayOrderItems(order);
            WriteLine();
        }

        // Gets the current status of an order.
        private string GetOrderStatus(Order order)
        {
            bool hasDeliverer = !string.IsNullOrWhiteSpace(order.DelivererName);

            if (order.Delivered && hasDeliverer)
                return "Delivered";
            else if (hasDeliverer && order.IsReadyForPickup)
                return "Being Delivered";
            else if (order.StartedCooking)
                return "Cooking";
            else
                return "Ordered";
        }

        // Displays all items in an order.
        private void DisplayOrderItems(Order order)
        {
            var groupedItems = order.Items
                .GroupBy(i => i.Name)
                .Select(g => new { Name = g.Key, Quantity = g.Sum(i => i.Quantity) });

            foreach (var item in groupedItems)
            {
                WriteLine($"{item.Quantity} x {item.Name}");
            }
        }

        // Handles the restaurant rating process.
        private void RateRestaurant()
        {
            var unratedOrders = GetUnratedOrders();

            if (!unratedOrders.Any())
            {
                WriteLine("No unrated orders available.");
                return;
            }

            var selectedOrder = SelectOrderToRate(unratedOrders);
            if (selectedOrder == null)
                return;

            int rating = GetRatingFromUser();
            if (rating == 0)
                return;

            string comment = GetCommentFromUser();
            SubmitRating(selectedOrder, rating, comment);
        }

        // Gets all delivered orders that haven't been rated.
        private List<Order> GetUnratedOrders()
        {
            return OrderManager.Instance.GetOrdersForUser(customer.Email)
                .Where(o => o.Delivered && !o.Ratings.Any(r => r.OrderNumber == o.OrderNumber))
                .ToList();
        }

        // Allows the customer to select which order to rate.
        private Order SelectOrderToRate(List<Order> orders)
        {
            WriteLine("Select a previous order to rate the restaurant it came from:");

            for (int i = 0; i < orders.Count; i++)
            {
                WriteLine($"{i + 1}: Order #{orders[i].OrderNumber} from {orders[i].RestaurantName}");
            }

            int menuOption = orders.Count + 1;
            WriteLine($"{menuOption}: Return to the previous menu");
            WriteLine($"Please enter a choice between 1 and {menuOption}:");

            string input = ReadLine();
            if (!int.TryParse(input, out int choice) || choice < 1 || choice > menuOption)
            {
                WriteLine("Invalid choice.");
                return null;
            }

            if (choice == menuOption)
                return null;

            return orders[choice - 1];
        }

        // Gets a rating (1-5) from the user.
        private int GetRatingFromUser()
        {
            while (true)
            {
                WriteLine("Please enter a rating for this restaurant (1-5, 0 to cancel):");
                string ratingInput = ReadLine();
                if (int.TryParse(ratingInput, out int rating) && rating >= 0 && rating <= 5)
                {
                    return rating;
                }
                WriteLine("Invalid rating.");
            }
        }

        // Gets a comment from the user to accompany the rating.
        private string GetCommentFromUser()
        {
            WriteLine("Please enter a comment to accompany this rating:");
            return ReadLine();
        }

        // Submits a rating for a restaurant.
        private void SubmitRating(Order order, int stars, string comment)
        {
            var newRating = new Rating
            {
                OrderNumber = order.OrderNumber,
                RestaurantName = order.RestaurantName,
                CustomerEmail = customer.Email,
                Stars = stars,
                Comment = comment,
                Date = DateTime.Now
            };

            RatingManager.Instance.AddRating(newRating);
            order.Ratings.Add(newRating);

            WriteLine($"Thank you for rating {order.RestaurantName}.");
        }
    }
}
