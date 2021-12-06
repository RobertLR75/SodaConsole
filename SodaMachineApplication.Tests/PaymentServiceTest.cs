using FluentAssertions;
using Xunit;

namespace SodaMachineApplication.Tests
{
    public class PaymentServiceTest
    {
        [Fact]
        public void AddAmount_Successfully()
        {
            PaymentService paymentService = new PaymentService();
            paymentService.AddAmount(1);
            paymentService.Balance.Should().Be(1);

        }

        [Fact]
        public void WithdrawAmount_Successfully()
        {
            PaymentService paymentService = new PaymentService();
            paymentService.AddAmount(10);
            paymentService.Balance.Should().Be(10);
            paymentService.WithdrawAmount(5);
            paymentService.Balance.Should().Be(5);
        }

        [Fact]
        public void SettleAmount_Successfully()
        {
            PaymentService paymentService = new PaymentService();
            paymentService.AddAmount(10);
            paymentService.Balance.Should().Be(10);
            paymentService.SettleAmount();
            paymentService.Balance.Should().Be(0);
        }

    }
}