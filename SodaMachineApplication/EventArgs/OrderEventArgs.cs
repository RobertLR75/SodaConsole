using SodaMachineApplication.Entities;

namespace SodaMachineApplication.EventArgs
{
    public class OrderEventArgs : DecimalEventArgs
    {
        public Inventory Inventory { get; set; }

        public OrderEventArgs(Inventory inventory, decimal balance) : base(balance)
        {
            Inventory = inventory;
        }
    }
}