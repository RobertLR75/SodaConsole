using System;
using FluentAssertions;
using SodaMachineApplication.EventArgs;
using Xunit;

namespace SodaMachineApplication.Tests
{
    public class SodaMachineServiceTest
    {
        [Fact]
        public void AddAmount_Successfully()
        {
            SodaMachineService sodaMachineService = new SodaMachineService();
            var inventoryList = sodaMachineService.GetInventoryList();
            inventoryList.Should().NotBeNull();
            inventoryList.Should().HaveCountGreaterThan(0);
        }

        [Fact]
        public void ProcessInput_Insert_100_Successfully()
        {
            SodaMachineService sodaMachineService = new SodaMachineService();
            sodaMachineService.BalanceChanged += SodaMachineServiceOnBalanceChanged;
            sodaMachineService.ProcessInput("insert 20");
        }

        [Fact]
        public void ProcessInput_Order_Coke_Successfully()
        {
            SodaMachineService sodaMachineService = new SodaMachineService();
            sodaMachineService.OrderProceeded += SodaMachineServiceOnOrderProceeded;

            sodaMachineService.ProcessInput("insert 20");
            sodaMachineService.ProcessInput("order coke");
        }

        [Fact]
        public void ProcessInput__Not_Successfully_NotValidCommand()
        {
            SodaMachineService sodaMachineService = new SodaMachineService();

            var act = () => sodaMachineService.ProcessInput("notvalid");
            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void ProcessInput_Order_Coke_InsufficientCreditAmount_Not_Successfully()
        {
            SodaMachineService sodaMachineService = new SodaMachineService();
            sodaMachineService.ProcessInput("insert 10");

            var act = () => sodaMachineService.ProcessInput("order coke");
            act.Should().Throw<Exception>();
        }

        [Fact]
        public void ProcessInput_Order_Not_Successfully_InventoryNotExists()
        {
            SodaMachineService sodaMachineService = new SodaMachineService();
            sodaMachineService.ProcessInput("insert 20");

            var act = () => sodaMachineService.ProcessInput("order cokelight");
            act.Should().Throw<Exception>();
        }
        [Fact]
        public void ProcessInput_Order_Coke_InventoryOutOfStock_Successfully()
        {
            SodaMachineService sodaMachineService = new SodaMachineService();
            sodaMachineService.ProcessInput("insert 15");
            sodaMachineService.ProcessInput("order sprite");
            sodaMachineService.ProcessInput("insert 15");
            sodaMachineService.ProcessInput("order sprite");
            sodaMachineService.ProcessInput("insert 15");
            sodaMachineService.ProcessInput("order sprite");
            sodaMachineService.ProcessInput("insert 15");

            var act = () => sodaMachineService.ProcessInput("order sprite");
            act.Should().Throw<Exception>();
        }

        [Fact]
        public void ProcessInput_Order_Invalid_Input_Successfully()
        {
            SodaMachineService sodaMachineService = new SodaMachineService();
            var act = () => sodaMachineService.ProcessInput("insert20");
            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void ProcessInput_CancelOrder_Successfully()
        {
            SodaMachineService sodaMachineService = new SodaMachineService();
            sodaMachineService.OrderCancelled += SodaMachineServiceOnOrderCancelled;
            sodaMachineService.ProcessInput("recall");
        }

        private void SodaMachineServiceOnOrderCancelled(object? sender, DecimalEventArgs e)
        {
            e.Balance.Should().Be(0);
        }

        private void SodaMachineServiceOnBalanceChanged(object? sender, DecimalEventArgs e)
        {
            e.Balance.Should().Be(20);
        }

        private void SodaMachineServiceOnOrderProceeded(object? sender, OrderEventArgs e)
        {
            e.Inventory.Name.Should().Be("coke");
            e.Balance.Should().Be(0);
        }
    }
}