namespace ArribaEats.Models
{
    public class OrderItem
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        public OrderItem(string name, int qty, decimal price)
        {
            Name = name;
            Quantity = qty;
            Price = price;
        }
    }
}
