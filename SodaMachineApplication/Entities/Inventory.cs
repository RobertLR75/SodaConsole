namespace SodaMachineApplication.Entities
{
    public class Inventory
    {
        public string Name { get; }

        public int Quantity { get; set; }
        public decimal Price { get; }

        public bool OutOfStock => Quantity == 0;


        public Inventory(string name, decimal price, int quantity)
        {
            Name = name;
            Price = price;
            Quantity = quantity;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}