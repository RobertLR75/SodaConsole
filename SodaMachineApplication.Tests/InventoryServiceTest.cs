using System;
using FluentAssertions;
using SodaMachineApplication.EventArgs;
using Xunit;

namespace SodaMachineApplication.Tests
{
    public class InventoryServiceTest
    {
        [Fact]
        public void GetInventory_Successfully_Found()
        {
            InventoryService inventoryService = new InventoryService();
            var inventory = inventoryService.GetInventory("coke");
            inventory.Should().NotBeNull();
            inventory?.Name.ToLower().Should().Be("coke");
        }

        [Fact]
        public void GetInventory_Not_Successfully_Found()
        {
            InventoryService inventoryService = new InventoryService();
            var inventory = inventoryService.GetInventory("notfound");
            inventory.Should().BeNull();
        }

        [Fact]
        public void UpdateQuantity_Successfully()
        {
            InventoryService inventoryService = new InventoryService();
            var inventory = inventoryService.GetInventory("coke");
            inventory.Should().NotBeNull();
            var quantity = inventory.Quantity;
            inventoryService.UpdateQuantity("coke");
            var updatedInventory = inventoryService.GetInventory("coke");
            updatedInventory.Should().NotBeNull();
            updatedInventory?.Quantity.Should().BeLessThan(quantity);
        }

        [Fact]
        public void UpdateQuantity_Not_Successfully_Inventory_NotFound()
        {
            InventoryService inventoryService = new InventoryService();
            var act = () => inventoryService.UpdateQuantity("notfound");
            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void UpdateQuantity_Not_Successfully_Inventory_OutOfStock()
        {
            InventoryService inventoryService = new InventoryService();
            inventoryService.UpdateQuantity("sprite");
            inventoryService.UpdateQuantity("sprite");
            inventoryService.UpdateQuantity("sprite");
            var act = () =>   inventoryService.UpdateQuantity("sprite");
            act.Should().Throw<Exception>();
        }

        [Fact]
        public void UpdateQuantity_Not_Successfully()
        {
            InventoryService inventoryService = new InventoryService();
            var act = () => inventoryService.UpdateQuantity("notfound");
            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void GetInventoryList_Successfully()
        {
            InventoryService inventoryService = new InventoryService();
            var inventoryList = inventoryService.GetInventoryList();
            inventoryList.Should().NotBeNull();
            inventoryList.Should().HaveCountGreaterThan(0);
        }
    }
}