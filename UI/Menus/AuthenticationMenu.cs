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
    /// <summary>
    /// Handles user authentication and registration for the Arriba Eats system.
    /// </summary>
    public class AuthenticationMenu
    {
        /// <summary>
        /// Gets the UserManager instance.
        /// </summary>
        private UserManager userManager = UserManager.Instance;

        /// <summary>
        /// Displays the authentication menu.
        /// </summary>
        public void Show()
        {
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

        /// <summary>
        /// Handles user login.
        /// </summary>
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

        /// <summary>
        /// Routes the user to the appropriate menu based on their type.
        /// </summary>
        /// <param name="user">The logged-in user.</param>
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

        /// <summary>
        /// Handles user registration.
        /// </summary>
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

        /// <summary>
        /// Registers a new customer user.
        /// </summary>
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

        /// <summary>
        /// Registers a new deliverer user.
        /// </summary>
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

        /// <summary>
        /// Registers a new client (restaurant owner) user.
        /// </summary>
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

        /// <summary>
        /// Gets a valid user name from input.
        /// </summary>
        /// <returns>A valid name containing only letters, spaces, hyphens, and apostrophes.</returns>
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

        /// <summary>
        /// Gets a valid age from input.
        /// </summary>
        /// <returns>A valid age between 18 and 100.</returns>
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

        /// <summary>
        /// Gets a valid email address from input.
        /// </summary>
        /// <returns>A valid, unique email address.</returns>
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

        /// <summary>
        /// Gets a valid phone number from input.
        /// </summary>
        /// <returns>A valid 10-digit phone number starting with 0.</returns>
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

        /// <summary>
        /// Gets a valid password from input.
        /// </summary>
        /// <returns>A valid password meeting all requirements.</returns>
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

        /// <summary>
        /// Gets a valid location from input.
        /// </summary>
        /// <param name="userType">The type of user (for context in messages).</param>
        /// <returns>A Location object.</returns>
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

        /// <summary>
        /// Gets a valid license plate from input.
        /// </summary>
        /// <returns>A valid LicensePlate object.</returns>
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

        /// <summary>
        /// Gets a valid restaurant name from input.
        /// </summary>
        /// <returns>A valid restaurant name.</returns>
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

        /// <summary>
        /// Gets a valid restaurant style from input.
        /// </summary>
        /// <returns>One of the supported restaurant styles.</returns>
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

        /// <summary>
        /// Validates whether a name is valid.
        /// </summary>
        /// <param name="name">The name to validate.</param>
        /// <returns>True if valid, false otherwise.</returns>
        private bool IsValidName(string name)
        {
            return Regex.IsMatch(name, @"^[A-Za-z'\- ]+$");
        }

        /// <summary>
        /// Validates whether a phone number is valid.
        /// </summary>
        /// <param name="phone">The phone number to validate.</param>
        /// <returns>True if valid (10 digits starting with 0), false otherwise.</returns>
        private bool IsValidPhone(string phone)
        {
            return phone.Length == 10 && phone.All(char.IsDigit) && phone.StartsWith("0");
        }

        /// <summary>
        /// Validates the format of a location string.
        /// </summary>
        /// <param name="input">The location input to validate.</param>
        /// <returns>True if valid format, false otherwise.</returns>
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
