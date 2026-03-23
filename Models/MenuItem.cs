namespace ArribaEats.Models
{
    // Represents a food or beverage item on a restaurant's menu.
    public class MenuItem
    {
        public string Name { get; set; } // The name of the menu item
        public decimal Price { get; set; } // The cost of the menu item

        // Initializes a new MenuItem with a name and price.
        public MenuItem(string name, decimal price)
        {
            Name = name;
            Price = price;
        }
    }
}