namespace SodaMachineApplication.EventArgs
{
    public class DecimalEventArgs : System.EventArgs
    {
        public decimal Balance { get; set; }

        public DecimalEventArgs(decimal balance)
        {
            Balance = balance;
        }
    }
}