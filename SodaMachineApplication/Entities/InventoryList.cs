using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SodaMachineApplication.Entities
{
    public class InventoryList : List<Inventory>
    {
        public string ToString(bool details)
        {
            if (!details)
            {
                return ToString();
            }

            var inventoryBuilder = new StringBuilder();

            foreach (var inventory in this)
            {
                if (inventory.Name != this.First().Name)
                {
                    inventoryBuilder.Append(", ");
                }

                inventoryBuilder.Append(inventory.Name + " Price: " + inventory.Price);
                if (inventory.OutOfStock)
                {
                    inventoryBuilder.Append(" (OutOfStock)");
                }
                else
                {
                    inventoryBuilder.Append(" (Stock: " + inventory.Quantity + ")");
                }
            }

            return inventoryBuilder.ToString();
        }
        public override string ToString()
        {
            var inventoryBuilder = new StringBuilder();

            foreach (var inventory in this)
            {
                if (inventory.Name != this.First().Name)
                {
                    inventoryBuilder.Append(", ");
                }

                inventoryBuilder.Append(inventory.Name);
            }

            return inventoryBuilder.ToString();
        }
    }
}