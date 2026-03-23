using System;
using System.Collections.Generic;
using System.Linq;
using static System.Console;
using ArribaEats.Models;
using ArribaEats.Managers;
using ArribaEats.Utilities;

namespace ArribaEats.UI.Menus
{
    public class DelivererMenu
    {
        private Deliverer user;
        private Order currentOrder;
        private bool atRestaurant = false;

        public DelivererMenu(Deliverer user)
        {
            this.user = user;
        }

        public void Show()
        {
            currentOrder = UserManager.Instance.GetCurrentOrderForDeliverer(user.Email);

            WriteLine($"Welcome back, {user.Name}!");

            bool running = true;
            while (running)
            {
                WriteLine("Please make a choice from the menu below:");
                WriteLine("1: Display your user information");
                WriteLine("2: List orders available to deliver");
                WriteLine("3: Arrived at restaurant to pick up order");
                WriteLine("4: Mark this delivery as complete");
                WriteLine("5: Log out");
                WriteLine("Please enter a choice between 1 and 5:");

                string input = ReadLine();

                switch (input)
                {
                    case "1":
                        DisplayUserInfo();
                        break;
                    case "2":
                        ListOrdersToDeliver();
                        break;
                    case "3":
                        ArrivedAtRestaurant();
                        break;
                    case "4":
                        CompleteDelivery();
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

        private void DisplayUserInfo()
        {
            TextPrinter.DisplayBaseUserInfo(user);
            WriteLine($"Licence plate: {user.LicensePlate}");

            var order = currentOrder ?? UserManager.Instance.GetCurrentOrderForDeliverer(user.Email);
            if (order != null && !order.Delivered)
            {
                var restaurant = UserManager.Instance.Users.OfType<Client>().FirstOrDefault(u => u.RestaurantName == order.RestaurantName);
                var customer = UserManager.Instance.Users.OfType<Customer>().FirstOrDefault(u => u.Email == order.CustomerEmail);
                WriteLine("Current delivery:");
                WriteLine($"Order #{order.OrderNumber} from {restaurant.RestaurantName} at {restaurant.Location.X},{restaurant.Location.Y}.");
                WriteLine($"To be delivered to {customer.Name} at {customer.Location.X},{customer.Location.Y}.");
            }
        }

        private void ListOrdersToDeliver()
        {
            if (currentOrder != null)
            {
                WriteLine("You have already selected an order for delivery.");
                return;
            }

            int dx = 0, dy = 0;
            string locInput;
            do
            {
                WriteLine("Please enter your location (in the form of X,Y): ");
                locInput = ReadLine();
                string[] parts = locInput.Split(',');
                if (parts.Length != 2 || !int.TryParse(parts[0], out dx) || !int.TryParse(parts[1], out dy))
                {
                    WriteLine("Invalid location.");
                    locInput = null;
                }
            } while (locInput == null);

            if (currentOrder != null)
            {
                WriteLine("You have already selected an order for delivery.");
                return;
            }

            var availableOrders = OrderManager.Instance.Orders
                .Where(o => string.IsNullOrWhiteSpace(o.DelivererName) && !o.Delivered)
                .OrderBy(o => o.OrderNumber)
                .ToList();

            if (!availableOrders.Any())
            {
                WriteLine("No available orders to deliver.");
                return;
            }

            WriteLine("\nThe following orders are available for delivery. Select an order to accept it:");
            WriteLine("   Order  Restaurant Name       Loc    Customer Name    Loc    Dist");

            for (int i = 0; i < availableOrders.Count; i++)
            {
                var o = availableOrders[i];
                var restaurant = UserManager.Instance.Users.OfType<Client>().First(u => u.RestaurantName == o.RestaurantName);
                var customer = UserManager.Instance.Users.OfType<Customer>().First(u => u.Email == o.CustomerEmail);
                int dist = Math.Abs(dx - restaurant.Location.X) + Math.Abs(dy - restaurant.Location.Y) +
                        Math.Abs(restaurant.Location.X - customer.Location.X) + Math.Abs(restaurant.Location.Y - customer.Location.Y);
                WriteLine($"{i + 1}: {o.OrderNumber,-6} {restaurant.RestaurantName,-20} {restaurant.Location.X},{restaurant.Location.Y,-4} {customer.Name,-16} {customer.Location.X},{customer.Location.Y,-6} {dist,4}");
            }

            WriteLine($"{availableOrders.Count + 1}: Return to the previous menu");
            WriteLine($"Please enter a choice between 1 and {availableOrders.Count + 1}: ");

            string choice = ReadLine();
            if (!int.TryParse(choice, out int index) || index < 1 || index > availableOrders.Count + 1)
            {
                WriteLine("Invalid choice.");
                return;
            }

            if (index == availableOrders.Count + 1)
                return;

            currentOrder = availableOrders[index - 1];
            currentOrder.DelivererName = user.Name;
            currentOrder.DelivererPlate = user.LicensePlate.ToString();
            UserManager.Instance.SetCurrentOrderForDeliverer(user.Email, currentOrder);

            var selectedRestaurant = UserManager.Instance.Users.OfType<Client>().First(u => u.RestaurantName == currentOrder.RestaurantName);
            WriteLine($"Thanks for accepting the order. Please head to {selectedRestaurant.RestaurantName} at {selectedRestaurant.Location.X},{selectedRestaurant.Location.Y} to pick it up.");
        }

        private void ArrivedAtRestaurant()
        {
            if (currentOrder == null)
            {
                currentOrder = UserManager.Instance.GetCurrentOrderForDeliverer(user.Email);
            }

            if (currentOrder == null)
            {
                WriteLine("You have not yet accepted an order.");
                return;
            }

            if (currentOrder.PickedUp)
            {
                WriteLine("You have already picked up this order.");
                return;
            }

            if (atRestaurant)
            {
                WriteLine("You already indicated that you have arrived at this restaurant.");
                return;
            }

            var restaurant = UserManager.Instance.Users.OfType<Client>().FirstOrDefault(u => u.RestaurantName == currentOrder.RestaurantName);
            var customer = UserManager.Instance.Users.OfType<Customer>().FirstOrDefault(u => u.Email == currentOrder.CustomerEmail);

            WriteLine($"Thanks. We have informed {restaurant.RestaurantName} that you have arrived and are ready to pick up order #{currentOrder.OrderNumber}.");
            WriteLine("Please show the staff this screen as confirmation.");

            currentOrder.DelivererArrived = true;
            UserManager.Instance.SetCurrentOrderForDeliverer(user.Email, currentOrder);

            if (!currentOrder.IsReadyForPickup)
            {
                WriteLine("The order is still being prepared, so please wait patiently until it is ready.");
            }

            WriteLine($"When you have the order, please deliver it to {customer.Name} at {customer.Location.X},{customer.Location.Y}.");

            atRestaurant = true;
        }

        private void CompleteDelivery()
        {
            if (currentOrder == null)
            {
                WriteLine("You have not yet accepted an order.");
                return;
            }

            if (!currentOrder.IsReadyForPickup)
            {
                WriteLine("This order is not ready for pickup yet.");
                return;
            }

            if (!currentOrder.PickedUp)
            {
                WriteLine("You have not yet picked up this order.");
                return;
            }

            if (currentOrder.Delivered)
            {
                WriteLine("This order has already been delivered.");
                return;
            }

            currentOrder.Delivered = true;
            UserManager.Instance.SetCurrentOrderForDeliverer(user.Email, currentOrder);
            WriteLine("Thank you for making the delivery.");

            UserManager.Instance.SetCurrentOrderForDeliverer(user.Email, null);
            currentOrder = null;
            atRestaurant = false;
        }
    }
}
