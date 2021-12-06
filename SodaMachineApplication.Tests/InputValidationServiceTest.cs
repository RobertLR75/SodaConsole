using System;
using FluentAssertions;
using SodaMachineApplication.EventArgs;
using Xunit;

namespace SodaMachineApplication.Tests
{
    public class InputValidationServiceTest
    {
        [Fact]
        public void ValidateInput_Insert_20_Successfully()
        {
            var inputValidationService = new InputValidationService();

            var inputType = inputValidationService.ValidateInput("insert 20");
            inputType.Should().Be(InputValidationService.InputType.Insert);
        }

        [Fact]
        public void ValidateInput_Insert_Not_Successfully()
        {
            var inputValidationService = new InputValidationService();
            var act = () => inputValidationService.ValidateInput("insert");
            act.Should().Throw<ArgumentException>();

        }

        [Fact]
        public void ValidateInput_Insert_Not_Successfully_Decimal()
        {
            var inputValidationService = new InputValidationService();
            var act = () => inputValidationService.ValidateInput("insert 2f");
            act.Should().Throw<ArgumentException>();

        }

        [Fact]
        public void ValidateInput_Not_Successfully_Null()
        {
            var inputValidationService = new InputValidationService();
            var act = () => inputValidationService.ValidateInput("");
            act.Should().Throw<ArgumentNullException>();

        }

        [Fact]
        public void ValidateInput_Order_Coke_Successfully()
        {
            var inputValidationService = new InputValidationService();

            var inputType = inputValidationService.ValidateInput("order coke");
            inputType.Should().Be(InputValidationService.InputType.Order);
        }

        [Fact]
        public void ValidateInput_SMS_Order_Coke_Successfully()
        {
            var inputValidationService = new InputValidationService();

            var inputType = inputValidationService.ValidateInput("sms order coke");
            inputType.Should().Be(InputValidationService.InputType.Order);
        }

        [Fact]
        public void ValidateInput_Order_Not_Successfully()
        {
            var inputValidationService = new InputValidationService();
            var act = () => inputValidationService.ValidateInput("order");
            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void ValidateInput_SMS_Order_Not_Successfully()
        {
            var inputValidationService = new InputValidationService();

            var act = () => inputValidationService.ValidateInput("sms order");
            act.Should().Throw<ArgumentException>();
        }


        [Fact]
        public void ValidateInput_Sms_Recall_Successfully()
        {
            var inputValidationService = new InputValidationService();

            var inputType = inputValidationService.ValidateInput("recall");
            inputType.Should().Be(InputValidationService.InputType.Cancel);
        }

        [Fact]
        public void ValidateInput_Sms_NotValid_Successfully()
        {
            var inputValidationService = new InputValidationService();

            var inputType = inputValidationService.ValidateInput("notfound");
            inputType.Should().Be(InputValidationService.InputType.NotValid);
        }
    }
}