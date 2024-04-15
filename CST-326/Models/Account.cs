using System;

namespace CST_326.Models
{
    public class Account
    {
        public int AccountId { get; set; }
        public int UserId { get; set; } // Foreign key to User
        public long AccountNumber { get; set; }
        public string AccountType { get; set; }
        public decimal Balance { get; set; }
        public DateTime CreatedAt { get; set; }

        // Constructor with parameters for UserId, AccountType, and Balance
        public Account(int userId, string accountType, decimal balance)
        {
            UserId = userId;
            AccountNumber = GenerateRandomAccountNumber();
            AccountType = accountType;
            Balance = balance;
            CreatedAt = DateTime.Now;
        }

        // Default constructor
        public Account()
        {
            // Constructor without parameters
            AccountNumber = GenerateRandomAccountNumber();
            AccountType = "Checking";
            Balance = GenerateRandomBalance();
            CreatedAt = DateTime.Now;
        }

        // Other constructors and methods...

        // Methods to handle deposit, withdrawal, etc., could be added here 

        private long GenerateRandomAccountNumber()
        {
            // Generate a random 10-digit number for the account number
            Random random = new Random();
            return random.Next(100000000, 999999999);
        }

        private decimal GenerateRandomBalance()
        {
            // Generate a random balance between 1 and 500,000
            Random random = new Random();
            return (decimal)random.NextDouble() * 500000;
        }
    }
}
