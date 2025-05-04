public class main
{
    public static void Main(string[] args)
    {
        GiftCardService service = new GiftCardService();

        // gift card number user input
        Console.Write("Enter gift card number: ");
        string cardNumber = Console.ReadLine();

        // purchase amount user input
        Console.Write("Enter purchase amount: ");
        if (!decimal.TryParse(Console.ReadLine(), out decimal purchaseAmount))
        {
            Console.WriteLine("Invalid amount.");
            return;
        }

        try
        { 
            decimal paid = service.ProcessGiftCardPayment(cardNumber, purchaseAmount);
            Console.WriteLine($"Payment successful. Amount paid: {paid:C}");
        }
        catch (Exception ex)
        {
            // error catch exception
            Console.WriteLine($"Payment failed: {ex.Message}");
        }
    }
}