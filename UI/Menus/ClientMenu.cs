using System;
using System.Collections.Generic;
using System.Linq;
using static System.Console;
using ArribaEats.Models;
using ArribaEats.Managers;
using ArribaEats.Utilities;

namespace ArribaEats.UI.Menus
{
    public class ClientMenu
    {
        private Client user;

        public ClientMenu(Client user)
        {
            this.user = user;
        }

        public void Show()
        {
            WriteLine($"Welcome back, {user.Name}!");

            bool running = true;
            while (running)
            {
                WriteLine("Please make a choice from the menu below:");
                WriteLine("1: Display your user information");
                WriteLine("2: Add item to restaurant menu");
                WriteLine("3: See current orders");
                WriteLine("4: Start cooking order");
                WriteLine("5: Finish cooking order");
                WriteLine("6: Handle deliverers who have arrived");
                WriteLine("7: Log out");
                WriteLine("Please enter a choice between 1 and 7:");

                string input = ReadLine();

                switch (input)
                {
                    case "1":
                        TextPrinter.DisplayBaseUserInfo(user);
                        WriteLine($"Restaurant name: {user.RestaurantName}");
                        WriteLine($"Restaurant style: {user.RestaurantStyle}");
                        WriteLine($"Restaurant location: {user.Location.X}, {user.Location.Y}");
                        break;
                    case "2":
                        AddItem();
                        break;
                    case "3":
                        ShowCurrentOrders();
                        break;
                    case "4":
                        StartCookingOrder();
                        break;
                    case "5":
                        FinishCookingOrder();
                        break;
                    case "6":
                        HandleDeliverers();
                        break;
                    case "7":
                        WriteLine("You are now logged out.");
                        running = false;
                        break;
                    default:
                        WriteLine("Invalid choice.");
                        break;
                }
            }
        }

        private void AddItem()
        {
            WriteLine("This is your restaurant's current menu:");
            foreach (var item in user.MenuItems)
            {
                WriteLine($"{item.Price,7:C2}  {item.Name}");
            }

            WriteLine();
            WriteLine("Please enter the name of the new item (blank to cancel): ");
            string name = ReadLine()?.Trim();

            if (string.IsNullOrEmpty(name))
            {
                return;
            }

            decimal price;
            while (true)
            {
                WriteLine("Please enter the price of the new item (without the $): ");
                string input = ReadLine()?.Trim();

                if (decimal.TryParse(input, out price) && price >= 0 && price < 1000)
                {
                    break;
                }

                WriteLine("Invalid price.");
            }

            user.MenuItems.Add(new MenuItem(name, price));
            WriteLine($"Successfully added {name} ({price:C2}) to menu.");
        }

        private void ShowCurrentOrders()
        {
            var orders = OrderManager.Instance.Orders
                .Where(o => o.RestaurantName == user.RestaurantName && !o.Delivered)
                .OrderBy(o => o.OrderNumber)
                .ToList();

            if (!orders.Any())
            {
                WriteLine("Your restaurant has no current orders.");
                return;
            }

            foreach (var order in orders)
            {
                var customer = UserManager.Instance.Users.First(u => u.Email == order.CustomerEmail);

                string status;
                bool hasDeliverer = !string.IsNullOrWhiteSpace(order.DelivererName);
                bool isDelivered = order.Delivered;

                if (isDelivered && hasDeliverer)
                {
                    status = "Delivered";
                }
                else if (hasDeliverer && order.IsReadyForPickup)
                {
                    status = "Being Delivered";
                }
                else if (order.IsReadyForPickup)
                {
                    status = "Ready for Delivery";
                }
                else if (order.StartedCooking)
                {
                    status = "Cooking";
                }
                else
                {
                    status = "Ordered";
                }

                WriteLine($"Order #{order.OrderNumber} for {customer.Name}: {status}");

                var groupedItems = order.Items
                    .GroupBy(i => i.Name)
                    .Select(g => new { Name = g.Key, Quantity = g.Sum(i => i.Quantity) });

                foreach (var item in groupedItems)
                {
                    WriteLine($"{item.Quantity} x {item.Name}");
                }

                WriteLine();
            }
        }

        private void StartCookingOrder()
        {
            var ordered = OrderManager.Instance.Orders
                .Where(o => o.RestaurantName == user.RestaurantName && !o.Delivered && !o.StartedCooking)
                .OrderBy(o => o.OrderNumber)
                .ToList();

            if (!ordered.Any())
            {
                WriteLine("No orders are waiting to be cooked.");
                return;
            }

            WriteLine("Select an order once you are ready to start cooking:");
            for (int i = 0; i < ordered.Count; i++)
            {
                var customer = UserManager.Instance.Users.First(u => u.Email == ordered[i].CustomerEmail);
                WriteLine($"{i + 1}: Order #{ordered[i].OrderNumber} for {customer.Name}");
            }
            WriteLine($"{ordered.Count + 1}: Return to the previous menu");
            WriteLine($"Please enter a choice between 1 and {ordered.Count + 1}:");

            string input = ReadLine();
            if (!int.TryParse(input, out int index) || index < 1 || index > ordered.Count + 1)
            {
                WriteLine("Invalid choice.");
                return;
            }

            if (index == ordered.Count + 1) return;

            var selectedOrder = ordered[index - 1];
            selectedOrder.StartedCooking = true;

            WriteLine($"Order #{selectedOrder.OrderNumber} is now marked as cooking. Please prepare the order, then mark it as finished cooking:");

            var groupedItems = selectedOrder.Items
                .GroupBy(i => i.Name)
                .Select(g => new { Name = g.Key, Quantity = g.Sum(i => i.Quantity) });

            foreach (var item in groupedItems)
            {
                WriteLine($"{item.Quantity} x {item.Name}");
            }
        }

        private void FinishCookingOrder()
        {
            var orders = OrderManager.Instance.Orders
                .Where(o => o.RestaurantName == user.RestaurantName && !o.Delivered)
                .ToList();

            var cooking = orders
                .Where(o => o.StartedCooking && !o.IsReadyForPickup)
                .ToList();

            if (!cooking.Any())
            {
                WriteLine("No cooking orders to finish.");
                return;
            }

            WriteLine("Select an order once you have finished preparing it:");
            for (int i = 0; i < cooking.Count; i++)
            {
                var customer = UserManager.Instance.Users.First(u => u.Email == cooking[i].CustomerEmail);
                WriteLine($"{i + 1}: Order #{cooking[i].OrderNumber} for {customer.Name}");
            }
            WriteLine($"{cooking.Count + 1}: Return to the previous menu");
            WriteLine("Please enter a choice between 1 and {0}: ", cooking.Count + 1);

            string input = ReadLine();
            if (!int.TryParse(input, out int index) || index < 1 || index > cooking.Count + 1)
            {
                WriteLine("Invalid choice.");
                return;
            }

            if (index == cooking.Count + 1) return;

            var selectedOrder = cooking[index - 1];
            selectedOrder.IsReadyForPickup = true;

            WriteLine($"Order #{selectedOrder.OrderNumber} is now ready for collection.");

            if (string.IsNullOrWhiteSpace(selectedOrder.DelivererPlate))
            {
                WriteLine("No deliverer has been assigned yet.");
            }
            else if (selectedOrder.DelivererArrived)
            {
                WriteLine($"Please take it to the deliverer with licence plate {selectedOrder.DelivererPlate}, who is waiting to collect it.");
            }
            else
            {
                WriteLine($"The deliverer with licence plate {selectedOrder.DelivererPlate} will be arriving soon to collect it.");
            }
        }

        private void HandleDeliverers()
        {
            var orders = OrderManager.Instance.Orders
            .Where(o => o.RestaurantName == user.RestaurantName
                    && !o.Delivered
                    && o.IsReadyForPickup
                    && o.DelivererArrived
                    && !o.PickedUp)
            .ToList();

            WriteLine("These deliverers have arrived and are waiting to collect orders.");
            WriteLine("Select an order to indicate that the deliverer has collected it:");

            for (int i = 0; i < orders.Count; i++)
            {
                var customer = UserManager.Instance.Users.First(u => u.Email == orders[i].CustomerEmail);
                string status = "Ordered";
                if (orders[i].IsReadyForPickup)
                    status = "Cooked";
                else if (orders[i].StartedCooking)
                    status = "Cooking";

                WriteLine($"{i + 1}: Order #{orders[i].OrderNumber} for {customer.Name} (Deliverer licence plate: {orders[i].DelivererPlate}) (Order status: {status})");
            }

            WriteLine($"{orders.Count + 1}: Return to the previous menu");
            WriteLine($"Please enter a choice between 1 and {orders.Count + 1}:");

            string input = ReadLine();
            if (!int.TryParse(input, out int index) || index < 1 || index > orders.Count + 1)
            {
                WriteLine("Invalid choice.");
                return;
            }

            if (index == orders.Count + 1)
                return;

            var selectedOrder = orders[index - 1];

            if (!selectedOrder.IsReadyForPickup)
            {
                WriteLine("This order has not yet been cooked.");
                return;
            }

            selectedOrder.PickedUp = true;
            WriteLine($"Order #{selectedOrder.OrderNumber} is now marked as being delivered.");
        }
    }
}
