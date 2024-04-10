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

        public Account(int userId, long accountNumber, string accountType, decimal balance)
        {
            UserId = userId;
            AccountNumber = accountNumber;
            AccountType = accountType;
            Balance = balance;
            CreatedAt = DateTime.Now;
        }
        public Account()
        {
            // Constructor without parameters
            AccountNumber = GenerateRandomAccountNumber();
            AccountType = "Checking";
            Balance = GenerateRandomBalance();
            CreatedAt = DateTime.Now;
        }
        public Account(int userId, string accountType)
        {
            // Constructor without parameters
            UserId = userId;
            AccountNumber = GenerateRandomAccountNumber();
            AccountType = accountType;
            Balance = GenerateRandomBalance();
            CreatedAt = DateTime.Now;
        }

        // Methods to handle deposit, withdrawal, etc., could be added here 
        public void Deposit(decimal amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Deposit amount must be positive.", nameof(amount));
            }
            Balance += amount;
        }

        public bool Withdraw(decimal amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Withdrawal amount must be positive.", nameof(amount));
            }
            if (Balance >= amount)
            {
                Balance -= amount;
                return true;
            }
            return false; // Insufficient funds
        }
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
