using SodaMachineApplication.Entities;
using SodaMachineApplication.EventArgs;

namespace SodaMachineApplication
{
    /// <summary>
    /// Dette er en Service som inventory (hvilke soda med pris og tilgjengelig anntall).
    /// </summary>
    public class InventoryService
    {

        private readonly InventoryList _inventoryList;
        public InventoryService()
        {
            _inventoryList = new InventoryList
            {
                new Inventory("coke", 20, 5),
                new Inventory("sprite", 15, 3),
                new Inventory ("fanta", 15, 3)
            };
        }

        public Inventory? GetInventory(string name)
        {
            return _inventoryList.FirstOrDefault(x => x.Name.ToLower() == name.ToLower());
        }

        public void UpdateQuantity(string name)
        {
            var inventory = _inventoryList.FirstOrDefault(x => x.Name.ToLower() == name.ToLower());

            if (inventory == null)
            {
                throw new ArgumentException(nameof(name));
            }

            if (inventory.Quantity == 0)
            {
                throw new Exception(inventory.Name + " is out of stock.");
            }

            inventory.Quantity --;
        }

        public InventoryList GetInventoryList()
        {
            return _inventoryList;
        }
    }
}