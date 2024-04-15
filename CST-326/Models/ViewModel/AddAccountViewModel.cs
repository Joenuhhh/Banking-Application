using CST_326.Models;

namespace CST_326.Models.ViewModel
{

    public class AddAccountViewModel
    {
        public User User { get; set; } // Represents the current user
        public Account Account { get; set; } // Represents the new account being added
        public decimal InitialBalance { get; set; } // Captures the initial balance input
    }
}
