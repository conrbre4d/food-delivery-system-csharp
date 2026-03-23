using System;
using System.Globalization;
using ArribaEats.UI.Menus;

namespace ArribaEats
{
    class Program
    {
        static void Main()
        {
            // Set default culture to en-US for consistent formatting
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("en-US");

            // Launch the authentication menu
            AuthenticationMenu menu = new AuthenticationMenu();
            menu.Show();
        }
    }
}