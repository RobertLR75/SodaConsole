using SodaMachineApplication;
using SodaMachineApplication.EventArgs;

namespace SodaMachineExtendedConsole
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            SodaMachine sodaMachine = new SodaMachine();
            sodaMachine.Start();
        }
    }

    public class SodaMachine
    {

        private readonly SodaMachineService _sodaMachineService;
        private decimal _balance = 0;
        public SodaMachine()
        {
            _sodaMachineService = new SodaMachineService();
            _sodaMachineService.OrderProceeded += SodaMachineService_OnOrderProceeded;
            _sodaMachineService.OrderCancelled += SodaMachineService_OnOrderCancelled;
            _sodaMachineService.BalanceChanged += SodaMachineService_OnBalanceChanged;
        }

        /// <summary>
        /// This is the starter method for the machine
        /// </summary>
        public void Start()
        {
            while (true)
            {

                // Lagt til en inventory liste som viser beholdning (Stock)
                // Lagt til exit kommando som avslutter console applikasjon.
                // Jeg har laget en SodaMachineService som inneholder all logikk.
                // Fordelen er at da er det enkelt å bytte ut Host (Console Applikasjon) med API, Web App, Win Forms eller lignende.
                // Det er enkelt å lage enhetstester som tester logikken i applikasjonen.

                var inventoryList = _sodaMachineService.GetInventoryList();


                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Inventory: " + inventoryList.ToString(true));
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n\nAvailable commands:");
                Console.WriteLine("insert (money) - Money put into money slot");
                Console.WriteLine("order (" + inventoryList.ToString() + ") - Order from machines buttons");
                Console.WriteLine("sms order (" + inventoryList.ToString() + ") - Order sent by sms");
                Console.WriteLine("recall - gives money back");
                Console.WriteLine("exit - Quit application");
                Console.WriteLine("-------");

                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("Inserted money: " + _balance);
                Console.WriteLine("-------\n\n");
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.Cyan;
                var input = Console.ReadLine();
                Console.ResetColor();

                if (input?.ToLower() == "exit")
                {
                    break;
                }

                try
                {
                    _sodaMachineService.ProcessInput(input);
                }
                catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(e.Message);
                    Console.ResetColor();
                }
            }
        }

        /// <summary>
        /// Event som oppdaterer Console når Balance i PaymentService er oppdatert.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SodaMachineService_OnBalanceChanged(object? sender, DecimalEventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            _balance = e.Balance;
            Console.ResetColor();
        }


        /// <summary>
        /// Event som oppdaterer Console når en ordre er cancellert.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SodaMachineService_OnOrderCancelled(object? sender, DecimalEventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Returning " + e.Balance + " to customer");
            Console.ResetColor();
        }

        /// <summary>
        /// Event som oppdaterer Console når en ordre er prosessert.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SodaMachineService_OnOrderProceeded(object? sender, OrderEventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Giving " + e.Inventory + " out.");
            Console.WriteLine("Giving " + e.Balance + " out in change.");
            Console.WriteLine();
            Console.ResetColor();
        }
    }
}