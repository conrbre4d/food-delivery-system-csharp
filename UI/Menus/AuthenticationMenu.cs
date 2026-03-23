using System;
using System.Text.RegularExpressions;
using static System.Console;
using System.Linq;
using ArribaEats.Models;
using ArribaEats.Models.Validation;
using ArribaEats.Managers;
using ArribaEats.Utilities;

namespace ArribaEats.UI.Menus
{
    // Handles user authentication and registration for the Arriba Eats system.
    public class AuthenticationMenu
    {
        // Gets the UserManager instance.
        private UserManager userManager = UserManager.Instance;

        // Displays the authentication menu.
        public void Show()
        {
            WriteLine("");
            WriteLine("Welcome to Arriba Eats!");

            bool running = true;
            while (running)
            {
                WriteLine();
                WriteLine("Please make a choice from the menu below:");
                WriteLine("1: Login as a registered user");
                WriteLine("2: Register as a new user");
                WriteLine("3: Exit");
                WriteLine("Please enter a choice between 1 and 3: ");
                string input = ReadLine();

                switch (input)
                {
                    case "1":
                        Login();
                        break;
                    case "2":
                        Register();
                        break;
                    case "3":
                        WriteLine("Thank you for using Arriba Eats!");
                        running = false;
                        break;
                    default:
                        WriteLine("Invalid choice.");
                        break;
                }
            }
        }

        // Handles user login.
        private void Login()
        {
            WriteLine();
            WriteLine("Email: ");
            string email = ReadLine();
            WriteLine("Password: ");
            string password = ReadLine();

            User user = userManager.Authenticate(email, password);
            if (user != null)
            {
                WriteLine();
                RouteUserToMenu(user);
            }
            else
            {
                WriteLine("Invalid email or password.");
            }
        }

        // Routes the user to the appropriate menu based on their type.
        private void RouteUserToMenu(User user)
        {
            switch (user)
            {
                case Customer customer:
                    new CustomerMenu(customer).Show();
                    break;
                case Deliverer deliverer:
                    new DelivererMenu(deliverer).Show();
                    break;
                case Client client:
                    new ClientMenu(client).Show();
                    break;
                default:
                    WriteLine("Unknown user type.");
                    break;
            }
        }

        // Handles user registration.
        private void Register()
        {
            WriteLine();
            WriteLine("Which type of user would you like to register as?");
            WriteLine("1: Customer");
            WriteLine("2: Deliverer");
            WriteLine("3: Client (Restaurant Owner)");
            WriteLine("4: Return to the previous menu");
            WriteLine("Please enter a choice between 1 and 4: ");
            string choice = ReadLine();

            switch (choice)
            {
                case "1":
                    RegisterCustomer();
                    break;
                case "2":
                    RegisterDeliverer();
                    break;
                case "3":
                    RegisterClient();
                    break;
                case "4":
                    return;
                default:
                    WriteLine("Invalid choice.");
                    return;
            }
        }

        // Registers a new customer user.
        private void RegisterCustomer()
        {
            string name = GetValidName();
            int age = GetValidAge();
            string email = GetValidEmail();
            string phone = GetValidPhone();
            string password = GetValidPassword();
            Location location = GetValidLocation("customer");

            var customer = userManager.RegisterCustomer(name, age, email, phone, password, location);
            WriteLine($"You have been successfully registered as a customer, {name}!");
        }

        // Registers a new deliverer user.
        private void RegisterDeliverer()
        {
            string name = GetValidName();
            int age = GetValidAge();
            string email = GetValidEmail();
            string phone = GetValidPhone();
            string password = GetValidPassword();
            LicensePlate plate = GetValidLicensePlate();

            var deliverer = userManager.RegisterDeliverer(name, age, email, phone, password, plate);
            WriteLine($"You have been successfully registered as a deliverer, {name}!");
        }

        // Registers a new client (restaurant owner) user.
        private void RegisterClient()
        {
            string name = GetValidName();
            int age = GetValidAge();
            string email = GetValidEmail();
            string phone = GetValidPhone();
            string password = GetValidPassword();
            string restaurantName = GetValidRestaurantName();
            string restaurantStyle = GetValidRestaurantStyle();
            Location location = GetValidLocation("restaurant");

            var client = userManager.RegisterClient(name, age, email, phone, password, restaurantName, restaurantStyle, location);
            WriteLine($"You have been successfully registered as a restaurant owner, {name}!");
        }

        // Gets a valid user name from input.
        private string GetValidName()
        {
            string name;
            do
            {
                WriteLine("Please enter your name: ");
                name = ReadLine();

                if (string.IsNullOrWhiteSpace(name) || !IsValidName(name))
                {
                    WriteLine("Invalid name. Use letters, spaces, hyphens, and apostrophes only.");
                }
            } while (string.IsNullOrWhiteSpace(name) || !IsValidName(name));
            return name;
        }

        // Gets a valid age from input.
        private int GetValidAge()
        {
            int age;
            do
            {
                WriteLine("Please enter your age (18-100): ");
                string ageInput = ReadLine();

                if (!int.TryParse(ageInput, out age) || age < 18 || age > 100)
                {
                    WriteLine("Invalid age. Must be between 18 and 100.");
                }
            } while (age < 18 || age > 100);
            return age;
        }

        // Gets a valid email address from input.
        private string GetValidEmail()
        {
            string email;
            do
            {
                WriteLine("Please enter your email address: ");
                email = ReadLine();

                if (!Email.IsValid(email))
                {
                    WriteLine("Invalid email address.");
                    email = null;
                }
                else if (userManager.EmailExists(email))
                {
                    WriteLine("This email address is already in use.");
                    email = null;
                }
            } while (string.IsNullOrWhiteSpace(email));
            return email;
        }

        // Gets a valid phone number from input.
        private string GetValidPhone()
        {
            string phone;
            do
            {
                WriteLine("Please enter your phone number (10 digits, starting with 0): ");
                phone = ReadLine();

                if (string.IsNullOrWhiteSpace(phone) || !IsValidPhone(phone))
                {
                    WriteLine("Invalid phone number. Must be 10 digits starting with 0.");
                    phone = null;
                }
            } while (string.IsNullOrWhiteSpace(phone));
            return phone;
        }

        // Gets a valid password from input.

        private string GetValidPassword()
        {
            string password;
            do
            {
                WriteLine();
                Password.DisplayRequirements();

                password = null;
                while (password == null)
                {
                    WriteLine("Please enter a password: ");
                    string input = ReadLine();

                    if (!Password.IsValid(input))
                    {
                        WriteLine("Invalid password.");
                        Password.DisplayRequirements();
                    }
                    else
                    {
                        password = input;
                    }
                }

                WriteLine("Please confirm your password: ");
                string confirm = ReadLine();
                if (password != confirm)
                {
                    WriteLine("Passwords do not match.");
                    password = null;
                }
            } while (password == null);
            return password;
        }

        // Gets a valid location from input.
        private Location GetValidLocation(string userType)
        {
            string locationInput;
            do
            {
                WriteLine($"Please enter your location (in the form of X,Y): ");
                locationInput = ReadLine();

                if (!IsValidLocationFormat(locationInput))
                {
                    WriteLine("Invalid location. Must be two integers separated by a comma.");
                    locationInput = null;
                }
            } while (locationInput == null);

            string[] parts = locationInput.Split(',');
            int x = int.Parse(parts[0].Trim());
            int y = int.Parse(parts[1].Trim());
            return new Location(x, y);
        }

        // Gets a valid license plate from input.
        private LicensePlate GetValidLicensePlate()
        {
            LicensePlate plate;
            do
            {
                WriteLine("Please enter your licence plate: ");
                string input = ReadLine();

                try
                {
                    plate = new LicensePlate(input);
                }
                catch
                {
                    WriteLine("Invalid licence plate.");
                    plate = null;
                }
            } while (plate == null);
            return plate;
        }

        // Gets a valid restaurant name from input.
        private string GetValidRestaurantName()
        {
            string name;
            do
            {
                WriteLine("Please enter your restaurant's name: ");
                name = ReadLine();
                if (string.IsNullOrWhiteSpace(name))
                {
                    WriteLine("Invalid restaurant name.");
                    name = null;
                }
            } while (name == null);
            return name;
        }

        // Gets a valid restaurant style from input.
        private string GetValidRestaurantStyle()
        {
            string[] styles = { "Italian", "French", "Chinese", "Japanese", "American", "Australian" };
            int choice;

            do
            {
                WriteLine("Please select your restaurant's style:");
                for (int i = 0; i < styles.Length; i++)
                {
                    WriteLine($"{i + 1}: {styles[i]}");
                }
                WriteLine("Please enter a choice between 1 and 6: ");
                string input = ReadLine();
                
                if (!int.TryParse(input, out choice) || choice < 1 || choice > styles.Length)
                {
                    WriteLine("Invalid choice.");
                    choice = 0;
                }
            } while (choice < 1 || choice > styles.Length);

            return styles[choice - 1];
        }

        // Validates whether a name is valid.
        private bool IsValidName(string name)
        {
            return Regex.IsMatch(name, @"^[A-Za-z'\- ]+$");
        }

        // Validates whether a phone number is valid.
        private bool IsValidPhone(string phone)
        {
            return phone.Length == 10 && phone.All(char.IsDigit) && phone.StartsWith("0");
        }

        // Validates the format of a location string.
        private bool IsValidLocationFormat(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return false;

            string[] parts = input.Split(',');
            if (parts.Length != 2) return false;

            bool xValid = int.TryParse(parts[0].Trim(), out _);
            bool yValid = int.TryParse(parts[1].Trim(), out _);

            return xValid && yValid;
        }
    }
}
