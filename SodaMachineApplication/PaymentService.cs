using SodaMachineApplication.EventArgs;

namespace SodaMachineApplication
{
    /// <summary>
    /// Dette er en service som håndterer credit og betaling for SodaMachine applikasjonen.
    /// </summary>
    public class PaymentService
    {
        private decimal _amount;
        public event EventHandler<DecimalEventArgs>? BalanceChanged;

        private void OnAmountChanged(decimal value)
        {
            EventHandler<DecimalEventArgs>? handler = BalanceChanged;
            handler?.Invoke(this, new DecimalEventArgs(value));
        }

        private decimal Amount
        {
            get => _amount;
            set
            {
                if (_amount != value)
                {
                    OnAmountChanged(value);
                }
                _amount = value;

            }
        }

        public decimal Balance => Amount;

        public void AddAmount(decimal amount)
        {
            Amount += amount;
        }

        public void WithdrawAmount(decimal amount)
        {
            Amount -= amount;
        }

        public void SettleAmount()
        {
            Amount = 0;
        }
    }
}