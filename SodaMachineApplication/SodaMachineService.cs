using SodaMachineApplication.Entities;
using SodaMachineApplication.EventArgs;

namespace SodaMachineApplication
{
    /// <summary>
    /// SodaMachineService er en service som håndterer all logikk for brusmaskinen.
    /// Den har en public metode, ProcessInput som Validerer input, legger til credit, utfører Order eller gjør en Cancel av order.
    /// SodaMachineService bruker 3 forskjellige spesialiserte services:
    /// PaymentService - Håndterer credit, betaling og tilbakebetaling.
    /// InventoryService - Håndterer hvilke brus som er tilgjengelig med pris og lagerstatus.
    /// InputValidationSerice - Validerer Input og sjekket at det er en gyldig kommando.
    /// </summary>
    public class SodaMachineService
    {
        private readonly PaymentService _paymentService;
        private readonly InventoryService _inventoryService;
        private readonly InputValidationService _inputValidationService;

        public event EventHandler<OrderEventArgs> OrderProceeded;
        public event EventHandler<DecimalEventArgs> OrderCancelled;

        public SodaMachineService()
        {
            _inputValidationService = new InputValidationService();
            _inventoryService = new InventoryService();
            _paymentService = new PaymentService();
        }


        /// <summary>
        /// Public metode som prosesserer input.
        /// </summary>
        /// <param name="input"></param>
        /// <exception cref="ArgumentException">Hvis input er ugyldig kastes Exception</exception>
        public void ProcessInput(string? input)
        {
            var inputType = _inputValidationService.ValidateInput(input);

            if (inputType == InputValidationService.InputType.NotValid)
            {
                throw new ArgumentException("Input is not a valid command: " + input);
            }

            if (inputType == InputValidationService.InputType.Insert)
            {
                AddCredit(input);
            }

            if (inputType == InputValidationService.InputType.Order)
            {
                ProcessOrder(input);
            }

            if (inputType == InputValidationService.InputType.Cancel)
            {
                CancelOrder();
            }
        }

        /// <summary>
        /// Oppretter kreditt med PaymentService.
        /// </summary>
        /// <param name="input"></param>
        private void AddCredit(string input)
        {
            var inputList = input.Split(' ');

            var amount = inputList[1];
            var credit = decimal.Parse(amount);
            _paymentService.AddAmount(credit);
        }


        /// <summary>
        /// Utfører en order og en sms order.
        /// </summary>
        /// <param name="input"></param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="Exception"></exception>
        private void ProcessOrder(string input)
        {
            var inputList = input.Split(' ');
            var order = input.ToLower().StartsWith("order") ? inputList[1] : inputList[2];

            var inventory = _inventoryService.GetInventory(order);

            if (inventory == null)
            {
                throw new ArgumentException("Order is not a valid inventory: " + order);
            }

            if (inventory.OutOfStock)
            {
                throw new Exception("Order: " + inventory.Name + " is out of stock");
            }

            if (_paymentService.Balance < inventory.Price)
            {
                throw new Exception("Insufficient CreditAmount.  Need " + (inventory.Price - _paymentService.Balance) + " more!");
            }

            _paymentService.WithdrawAmount(inventory.Price);
            var balance = _paymentService.Balance;
            _inventoryService.UpdateQuantity(order);
            _paymentService.SettleAmount();
            OnOrderProcessed(inventory, balance);
        }

        private void CancelOrder()
        {
            var balance = _paymentService.Balance;
            _paymentService.SettleAmount();
            PaymentServiceOnAmountSettled(balance);
        }

        private void PaymentServiceOnAmountSettled(decimal balance)
        {
            EventHandler<DecimalEventArgs> handler = OrderCancelled;
            handler?.Invoke(this, new DecimalEventArgs(balance));
        }

        /// <summary>
        /// Event som utføres når PaymentSerice sin Balance er oppdatert.
        /// </summary>
        public event EventHandler<DecimalEventArgs>? BalanceChanged
        {
            add => _paymentService.BalanceChanged += value;
            remove => _paymentService.BalanceChanged -= value;
        }

        private void OnOrderProcessed(Inventory inventory, decimal balance)
        {
            EventHandler<OrderEventArgs> handler = OrderProceeded;
            handler?.Invoke(this, new OrderEventArgs(inventory, balance));
        }

        public InventoryList GetInventoryList()
        {
            return _inventoryService.GetInventoryList();
        }
    }
}