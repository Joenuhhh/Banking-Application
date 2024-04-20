using CST_326.DAO;
using CST_326.Models;
using CST_326.Models.ViewModel;
using CST_326.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace CST_326.Controllers
{
    public class AccountController : Controller
    {
        private UserRepository userRepository;
        bool loggedIn = false;

        public AccountController()
        {
            userRepository = new UserRepository();


        }

        public IActionResult Index()
        {
            return View("Login");
        }
        public IActionResult ProcessLogin(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Login");
            }

            var user = userRepository.userDAO.FindUser(loginViewModel);
            if (user == null)
            {
                // Log the issue or return an error message to the view, for example:
                // ViewBag.ErrorMessage = "User not found or invalid credentials.";
                return View("Login");
                // Optionally, you might want to pass back the loginViewModel to the view to preserve user input
                // return View("Login", loginViewModel);
            }

            // Retrieve the user's accounts based on the user's ID
            var accounts = userRepository.userDAO.GetAccountsByUserId(user.UserId);

            // Create an instance of DashboardViewModel and populate its properties
            var viewModel = new DashboardViewModel
            {
                User = user,
                Accounts = accounts
            };
            loggedIn = true;
            // Pass the ViewModel to the Dashboard view
            return View("Dashboard", viewModel);
        }



        public IActionResult Register()
        {
            return View("Register");
        }
        [HttpPost]
        public IActionResult ProcessRegistration(RegistrationViewModel newUser)
        {
            Console.WriteLine("Processing registration for user: " + newUser.UserName);

            if (ModelState.IsValid)
            {
                Console.WriteLine("Model state is valid. Registering user...");
                UserDAO userDAO = new UserDAO();
                userDAO.RegisterUser(newUser);
                // Redirect to the login page after successful registration
                return RedirectToAction("Index", "Account");
            }
            else
            {
                Console.WriteLine("Model state is not valid. Returning to registration view.");
            }

            // If registration fails, return to the registration page
            return View("Register");
        }

        public IActionResult AddAccount(int userId)
        {
            // Retrieve the current user
            var user = userRepository.userDAO.GetUserById(userId);
            if (user == null)
            {
                // Handle the case where the user is not found
                return NotFound();
            }

            // Create a new account object with default values
            var newAccount = new Account();

            // Pass the user object and the new account object to the view
            var viewModel = new AddAccountViewModel
            {
                User = user,
                Account = newAccount
            };

            // Render the view with the populated data
            return View("AddAccount", viewModel);
        }

        [HttpPost]
        public IActionResult ProcessAddAccount(int userId, decimal initialBalance)
        {
            try
            {
                // Retrieve the current user
                var user = userRepository.userDAO.GetUserById(userId);

                if (user == null)
                {
                    // Handle the case where the user is not found
                    return NotFound();
                }

                var newAccount = new Account(user.UserId, "Checking", initialBalance);

                // Save the new account to the database by calling the AddAccount method
                userRepository.userDAO.AddAccount(newAccount);
                var accounts = userRepository.userDAO.GetAccountsByUserId(userId);
                var viewModel = new DashboardViewModel
                {
                    User = user,
                    Accounts = accounts
                };

                // After processing, redirect back to the user's dashboard
                return View("Dashboard", viewModel);

            }
            catch (Exception ex)
            {
                // Log the exception or handle it as necessary
                Console.WriteLine("An error occurred while processing the add account request: " + ex.Message);
                return RedirectToAction("Error", "Home"); // Redirect to an error page
            }
        }




        public IActionResult DeleteAccount(int accountId)
        {
            try
            {
                userRepository.userDAO.DeleteAccount(accountId);
                return Json(new { success = true, message = "Account deleted successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error deleting account: " + ex.Message });
            }
        }
        [HttpPost]
        public IActionResult TransferFunds(int fromAccountId, int toAccountId, decimal amount)
        {
            try
            {
                // Retrieve the accounts from the repository
                Account fromAccount = userRepository.userDAO.GetAccountById(fromAccountId);
                Account toAccount = userRepository.userDAO.GetAccountById(toAccountId);

                // Check if accounts exist and if there are sufficient funds
                if (fromAccount != null && toAccount != null && fromAccount.Balance >= amount)
                {
                    // Deduct amount from 'from' account
                    fromAccount.Balance -= amount;

                    // Add amount to 'to' account
                    toAccount.Balance += amount;

                    // Update accounts in the repository
                    userRepository.userDAO.UpdateAccount(fromAccount);
                    userRepository.userDAO.UpdateAccount(toAccount);

                    return Json(new { success = true, message = "Funds transferred successfully." });
                }
                else
                {
                    return Json(new { success = false, message = "Transfer failed. Please check your account balances and try again." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred while transferring funds: " + ex.Message });
            }
        }




    }
}
