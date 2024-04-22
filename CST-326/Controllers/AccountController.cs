// AccountController.cs

using CST_326.DAO;
using CST_326.Models;
using CST_326.Models.ViewModel;
using CST_326.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Reflection;

namespace CST_326.Controllers
{
	public class AccountController : Controller
	{
		private UserRepository userRepository; // Repository for user-related operations
		bool loggedIn = false; // Flag indicating whether a user is logged in

		/// <summary>
		/// Constructor for AccountController class.
		/// Initializes UserRepository.
		/// </summary>
		public AccountController()
		{
			userRepository = new UserRepository();
		}

		/// <summary>
		/// Action method to display the login page.
		/// </summary>
		public IActionResult Index()
		{
			return View("Login");
		}

		/// <summary>
		/// Action method to process user login.
		/// </summary>
		/// <param name="loginViewModel">Login view model containing user credentials.</param>
		public IActionResult ProcessLogin(LoginViewModel loginViewModel)
		{
			// Check if the model state is valid
			if (!ModelState.IsValid)
			{
				return View("Login");
			}

			// Attempt to find the user based on login credentials
			var user = userRepository.userDAO.FindUser(loginViewModel);
			if (user == null)
			{
				// Return to the login view with an error message if the user is not found
				return View("Login");
			}

			// Retrieve the user's accounts based on the user's ID
			var accounts = userRepository.userDAO.GetAccountsByUserId(user.UserId);

			// Create a ViewModel containing user and account information
			var viewModel = new DashboardViewModel
			{
				User = user,
				Accounts = accounts
			};
			loggedIn = true; // Set loggedIn flag to true upon successful login

			// Display the dashboard view with the ViewModel
			return View("Dashboard", viewModel);
		}

		/// <summary>
		/// Action method to display the registration page.
		/// </summary>
		public IActionResult Register()
		{
			return View("Register");
		}

		/// <summary>
		/// Action method to process user registration.
		/// </summary>
		/// <param name="newUser">Registration view model containing user details.</param>
		[HttpPost]
		public IActionResult ProcessRegistration(RegistrationViewModel newUser)
		{
			Console.WriteLine("Processing registration for user: " + newUser.UserName);

			// Check if the model state is valid
			if (ModelState.IsValid)
			{
				Console.WriteLine("Model state is valid. Registering user...");
				// Instantiate UserDAO and register the new user
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

		/// <summary>
		/// Action method to display the add account page.
		/// </summary>
		/// <param name="userId">ID of the user for whom the account is being added.</param>
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

			// Create a ViewModel containing user and account information
			var viewModel = new AddAccountViewModel
			{
				User = user,
				Account = newAccount
			};

			// Render the view with the populated data
			return View("AddAccount", viewModel);
		}

		/// <summary>
		/// Action method to process adding a new account.
		/// </summary>
		/// <param name="userId">ID of the user for whom the account is being added.</param>
		/// <param name="initialBalance">Initial balance of the new account.</param>
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

				// Create a new account with provided details
				var newAccount = new Account(user.UserId, "Checking", initialBalance);

				// Save the new account to the database
				userRepository.userDAO.AddAccount(newAccount);

				// Retrieve updated list of accounts
				var accounts = userRepository.userDAO.GetAccountsByUserId(userId);
				var viewModel = new DashboardViewModel
				{
					User = user,
					Accounts = accounts
				};

				// Redirect back to the user's dashboard
				return View("Dashboard", viewModel);
			}
			catch (Exception ex)
			{
				// Log the exception and redirect to an error page
				Console.WriteLine("An error occurred while processing the add account request: " + ex.Message);
				return RedirectToAction("Error", "Home");
			}
		}

		/// <summary>
		/// Action method to delete an account.
		/// </summary>
		/// <param name="accountId">ID of the account to be deleted.</param>
		public IActionResult DeleteAccount(int accountId)
		{
			try
			{
				// Delete the account with the provided ID
				userRepository.userDAO.DeleteAccount(accountId);
				// Return success message as JSON
				return Json(new { success = true, message = "Account deleted successfully." });
			}
			catch (Exception ex)
			{
				// Return error message as JSON if deletion fails
				return Json(new { success = false, message = "Error deleting account: " + ex.Message });
			}
		}
	}
}
