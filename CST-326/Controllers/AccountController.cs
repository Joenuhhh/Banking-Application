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



    }
}
