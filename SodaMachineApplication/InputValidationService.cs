namespace SodaMachineApplication;

public class InputValidationService
{
    public InputType ValidateInput(string? input)
    {
        if (string.IsNullOrEmpty(input))
        {
            throw new ArgumentNullException(nameof(input), "Not a valid command!");
        }

        if (input.ToLower().StartsWith("insert"))
        {
            return ValidateInsert(input);
        }

        if ((input.ToLower().StartsWith("order") || input.ToLower().StartsWith("sms order")))
        {
            return ValidateOrder(input);
        }

        if (input.ToLower().Equals("recall"))
        {
            return InputType.Cancel;
        }

        return InputType.NotValid;
    }



    private  InputType ValidateInsert(string input)
    {
        var inputList = input.Split(' ');

        if (inputList.Length != 2)
        {
            Console.WriteLine("");
            throw new ArgumentException("Invalid input format: " + input);
        }

        var amount = inputList[1];
        var res = decimal.TryParse(amount, out var credit);

        if (!res)
        {
            throw new ArgumentException("Invalid input format: " + input + ". Not a decimal format!", nameof(input));
        }

        return InputType.Insert;
    }

    private InputType ValidateOrder(string input)
    {

        if (input.StartsWith("order"))
        {
            var inputList = input.Split(' ');

            if (inputList.Length != 2)
            {
                throw new ArgumentException("Order is not a valid: " + input);
            }

            return InputType.Order;
        }
        else
        {
            var inputList = input.Split(' ');
            if (inputList.Length != 3)
            {
                throw new ArgumentException("Order is not a valid: " + input);
            }
        }
        return InputType.Order;
    }

    public enum InputType
    {
        NotValid,
        Insert,
        Order,
        Cancel


    }
}