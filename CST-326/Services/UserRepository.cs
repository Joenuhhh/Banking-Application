// UserRepository.cs

using CST_326.Models;
using CST_326.Models.ViewModel;
using System;

namespace CST_326.DAO
{
	/// <summary>
	/// Repository for user-related operations.
	/// </summary>
	public class UserRepository : IUserRepository<LoginViewModel>
	{
		/// <summary>
		/// Instance of UserDAO for database operations.
		/// </summary>
		public UserDAO userDAO = new UserDAO();

		/// <summary>
		/// Not implemented.
		/// </summary>
		/// <param name="user">LoginViewModel object to edit.</param>
		/// <returns>Not implemented exception.</returns>
		public User EditUser(LoginViewModel user)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Retrieves a user from the database.
		/// </summary>
		/// <param name="user">LoginViewModel object representing user credentials.</param>
		/// <returns>User object if found, otherwise null.</returns>
		public User GetUser(LoginViewModel user)
		{
			return userDAO.FindUser(user);
		}
	}
}
