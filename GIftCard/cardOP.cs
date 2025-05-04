using System;
using System.Data.SqlClient;

public class GiftCard
{
    public string CardNumber { get; set; }
    public decimal Balance { get; set; }
    public bool IsActive { get; set; }
    public DateTime ExpiryDate { get; set; }
}

public class GiftCardService
{   
    // connect to SQL server database
    private string connectionString = "Server=localhost;Database=giftcardDB;Trusted_Connection=True;";

    // retrieve card from database
    public GiftCard GetGiftCard(string cardNumber)
    {
        using SqlConnection connect = new SqlConnection(connectionString);
        connect.Open();

        string query = "SELECT GiftCardNumber, CurrentBalance, IsActive, ExpirationDate FROM GiftCard WHERE GiftCardNumber = @CardNumber";

        using SqlCommand cmd = new SqlCommand(query, connect);
        cmd.Parameters.AddWithValue("@CardNumber", cardNumber);

        using SqlDataReader reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            return new GiftCard
            {
                CardNumber = reader.GetString(0),
                Balance = reader.GetDecimal(1),
                IsActive = reader.GetBoolean(2),
                ExpiryDate = reader.GetDateTime(3)
            };
        }

        return null;
    }

    // update gift card balance in the database
    public void UpdateGiftCardBalance(string cardNumber, decimal newBalance)
    {
        using SqlConnection connect = new SqlConnection(connectionString);
        connect.Open();

        // query to update the balance
        string updateQuery = "UPDATE GiftCard SET CurrentBalance = @Balance WHERE GiftCardNumber = @CardNumber";

        using SqlCommand cmd = new SqlCommand(updateQuery, connect);
        cmd.Parameters.AddWithValue("@Balance", newBalance);
        cmd.Parameters.AddWithValue("@CardNumber", cardNumber);

        int rowsAffected = cmd.ExecuteNonQuery();
        if (rowsAffected == 0)
        {
            throw new InvalidOperationException("Failed to update balance. Card not found.");
        }

        Console.WriteLine($"Gift card {cardNumber} balance updated to {newBalance:C}");


    }

    // payment processing for a gift card
    public decimal ProcessGiftCardPayment(string cardNumber, decimal purchaseAmount)
    {
        // check for empty card number input
        if (string.IsNullOrWhiteSpace(cardNumber))
            throw new ArgumentException("Card number is required.");

        // check for purchase amount
        if (purchaseAmount <= 0)
            throw new ArgumentException("Purchase amount must be greater than zero.");

        // card details
        GiftCard card = GetGiftCard(cardNumber);

        // validation
        if (card == null)
            throw new InvalidOperationException("Gift card not found.");

        if (!card.IsActive)
            throw new InvalidOperationException("Gift card is inactive.");

        if (card.ExpiryDate < DateTime.UtcNow)
            throw new InvalidOperationException("Gift card is expired.");

        if (card.Balance <= 0)
            throw new InvalidOperationException("Gift card has no available balance.");

        if (purchaseAmount > card.Balance)
        {
            throw new InvalidOperationException($"Purchase amount is greater than available balance. \nAvailable balance: {card.Balance:C}");
        }

        // calculate the payment
        decimal paymentToProcess = Math.Min(purchaseAmount, card.Balance);
        decimal newBalance = card.Balance - paymentToProcess;

        // update balance in database
        UpdateGiftCardBalance(card.CardNumber, newBalance);

        return paymentToProcess;
    }
}
