// UserDAO.cs

using CST_326.Models;
using CST_326.Models.ViewModel;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace CST_326.DAO
{
	public class UserDAO
	{
		// Connection string for MySQL database
		string myConnectionString = "Server=jonahmysqlserver.mysql.database.azure.com;Database=capstone;UserId=joenuh;Password=Jonah124;SslMode=Preferred;";

		/// <summary>
		/// Retrieves a user by ID from the database.
		/// </summary>
		/// <param name="userId">ID of the user to retrieve.</param>
		/// <returns>User object if found, otherwise null.</returns>
		public User GetUserById(int userId)
		{
			User user = null;

			string sqlStatement = "SELECT * FROM users WHERE UserId = @UserId";

			using (MySqlConnection connection = new MySqlConnection(myConnectionString))
			{
				MySqlCommand command = new MySqlCommand(sqlStatement, connection);
				command.Parameters.AddWithValue("@UserId", userId);

				try
				{
					connection.Open();
					MySqlDataReader reader = command.ExecuteReader();
					if (reader.Read())
					{
						user = new User
						{
							UserId = reader.GetInt32(0),
							FirstName = reader.GetString(1),
							LastName = reader.GetString(2),
							UserName = reader.GetString(3),
							Email = reader.GetString(4),
							PhoneNumber = reader.GetString(5),
							Password = reader.GetString(6)
						};
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine("Error retrieving user: " + ex.Message);
				}
			}

			return user;
		}

		/// <summary>
		/// Finds a user by username and password in the database.
		/// </summary>
		/// <param name="user">LoginViewModel containing user credentials.</param>
		/// <returns>User object if found, otherwise null.</returns>
		public User FindUser(LoginViewModel user)
		{
			string sqlStatement = "SELECT * FROM users WHERE Username = @USERNAME and Password = @PASSWORD";

			using (MySqlConnection connection = new MySqlConnection(myConnectionString))
			{
				MySqlCommand command = new MySqlCommand(sqlStatement, connection);
				command.Parameters.AddWithValue("@USERNAME", user.UserName);
				command.Parameters.AddWithValue("@PASSWORD", user.Password);

				try
				{
					connection.Open();
					MySqlDataReader reader = command.ExecuteReader();
					while (reader.Read())
					{
						User foundUser = new User()
						{
							UserId = reader.GetInt32(0),
							FirstName = reader.GetString(1),
							LastName = reader.GetString(2),
							UserName = reader.GetString(3),
							Email = reader.GetString(4),
							PhoneNumber = reader.GetString(5),
							Password = reader.GetString(6)
						};
						return foundUser;
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message);
					Console.WriteLine(ex.StackTrace);
				}
			}
			return null;
		}

		/// <summary>
		/// Deletes a user from the database.
		/// </summary>
		/// <param name="user">User object to delete.</param>
		/// <returns>True if deletion is successful, otherwise false.</returns>
		public bool DeleteUser(User user)
		{
			string deleteQuery = "DELETE FROM users WHERE UserId = @Id";

			using (MySqlConnection connection = new MySqlConnection(myConnectionString))
			{
				MySqlCommand command = new MySqlCommand(deleteQuery, connection);
				command.Parameters.AddWithValue("@id", user.UserId);

				connection.Open();
				int rowsAffected = command.ExecuteNonQuery();
				connection.Close();

				return rowsAffected > 0;
			}
		}

		/// <summary>
		/// Retrieves a username by username from the database.
		/// </summary>
		/// <param name="username">Username to retrieve.</param>
		/// <returns>Username if found, otherwise null.</returns>
		public string GetUserByUsername(string username)
		{
			string result = null;

			string sqlStatement = "SELECT Username FROM users WHERE Username = @Username";

			using (MySqlConnection connection = new MySqlConnection(myConnectionString))
			{
				MySqlCommand command = new MySqlCommand(sqlStatement, connection);
				command.Parameters.AddWithValue("@Username", username);

				try
				{
					connection.Open();
					object obj = command.ExecuteScalar();
					if (obj != null)
					{
						result = obj.ToString();
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine("Error retrieving username: " + ex.Message);
				}
			}

			return result;
		}

		/// <summary>
		/// Retrieves a password by username from the database.
		/// </summary>
		/// <param name="username">Username to retrieve password for.</param>
		/// <returns>Password if found, otherwise null.</returns>
		public string GetPasswordByUsername(string username)
		{
			string result = null;

			string sqlStatement = "SELECT Password FROM users WHERE Username = @Username";

			using (MySqlConnection connection = new MySqlConnection(myConnectionString))
			{
				MySqlCommand command = new MySqlCommand(sqlStatement, connection);
				command.Parameters.AddWithValue("@Username", username);

				try
				{
					connection.Open();
					object obj = command.ExecuteScalar();
					if (obj != null)
					{
						result = obj.ToString();
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine("Error retrieving password: " + ex.Message);
				}
			}

			return result;
		}

		/// <summary>
		/// Registers a new user in the database.
		/// </summary>
		/// <param name="newUser">RegistrationViewModel containing user details.</param>
		public void RegisterUser(RegistrationViewModel newUser)
		{
			string sqlStatement = "INSERT INTO users (Username, Email, Password, FirstName, LastName, Phone) VALUES (@USERNAME, @EMAIL, @PASSWORD, @FIRSTNAME, @LASTNAME, @PHONENUMBER)";

			using (MySqlConnection connection = new MySqlConnection(myConnectionString))
			{
				MySqlCommand command = new MySqlCommand(sqlStatement, connection);

				command.Parameters.AddWithValue("@USERNAME", newUser.UserName);
				command.Parameters.AddWithValue("@EMAIL", newUser.Email);
				command.Parameters.AddWithValue("@PASSWORD", newUser.Password);
				command.Parameters.AddWithValue("@FIRSTNAME", newUser.FirstName);
				command.Parameters.AddWithValue("@LASTNAME", newUser.LastName);
				command.Parameters.AddWithValue("@PHONENUMBER", newUser.PhoneNumber);

				try
				{
					connection.Open();
					command.ExecuteNonQuery();
					Console.WriteLine("User registration successful.");
				}
				catch (MySqlException ex2)
				{
					Console.WriteLine("Error occurred during user registration: " + ex2.Message);
					Console.WriteLine("Error code: " + ex2.ErrorCode);
					Console.WriteLine("Error number: " + ex2.Number);
				}
			}
		}

		/// <summary>
		/// Retrieves accounts belonging to a user from the database.
		/// </summary>
		/// <param name="userId">ID of the user whose accounts are to be retrieved.</param>
		/// <returns>List of accounts belonging to the user.</returns>
		public List<Account> GetAccountsByUserId(int userId)
		{
			List<Account> accounts = new List<Account>();
			string sqlStatement = "SELECT * FROM accounts2 WHERE UserId = @UserId";

			using (MySqlConnection connection = new MySqlConnection(myConnectionString))
			{
				MySqlCommand command = new MySqlCommand(sqlStatement, connection);
				command.Parameters.AddWithValue("@UserId", userId);

				try
				{
					connection.Open();
					MySqlDataReader reader = command.ExecuteReader();
					while (reader.Read())
					{
						Account account = new Account()
						{
							AccountId = reader.GetInt32(0),
							UserId = reader.GetInt32(1),
							AccountNumber = reader.GetInt64(2),
							AccountType = reader.GetString(3),
							Balance = reader.GetDecimal(4),
							CreatedAt = reader.GetDateTime(5)
						};
						accounts.Add(account);
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine("Error retrieving accounts: " + ex.Message);
				}
			}

			return accounts;
		}

		/// <summary>
		/// Adds a new account to the database.
		/// </summary>
		/// <param name="account">Account object to be added.</param>
		public void AddAccount(Account account)
		{
			string insertQuery = "INSERT INTO accounts2 (UserId, AccountNumber, AccountType, Balance, CreationDate) " +
								 "VALUES (@UserId, @AccountNumber, @AccountType, @Balance, @CreatedAt)";

			using (MySqlConnection connection = new MySqlConnection(myConnectionString))
			{
				MySqlCommand command = new MySqlCommand(insertQuery, connection);
				command.Parameters.AddWithValue("@UserId", account.UserId);
				command.Parameters.AddWithValue("@AccountNumber", account.AccountNumber);
				command.Parameters.AddWithValue("@AccountType", account.AccountType);
				command.Parameters.AddWithValue("@Balance", account.Balance);
				command.Parameters.AddWithValue("@CreatedAt", account.CreatedAt);

				try
				{
					connection.Open();
					command.ExecuteNonQuery();
					Console.WriteLine("Account added successfully.");
				}
				catch (MySqlException ex)
				{
					Console.WriteLine("Error adding account: " + ex.Message);
				}
			}
		}

		/// <summary>
		/// Deletes an account from the database.
		/// </summary>
		/// <param name="accountId">ID of the account to be deleted.</param>
		public void DeleteAccount(int accountId)
		{
			string deleteQuery = "DELETE FROM accounts2 WHERE AccountId = @AccountId";

			using (MySqlConnection connection = new MySqlConnection(myConnectionString))
			{
				MySqlCommand command = new MySqlCommand(deleteQuery, connection);
				command.Parameters.AddWithValue("@AccountId", accountId);

				try
				{
					connection.Open();
					int rowsAffected = command.ExecuteNonQuery();
					if (rowsAffected > 0)
					{
						Console.WriteLine("Account deleted successfully.");
					}
					else
					{
						Console.WriteLine("No account found with ID: " + accountId);
					}
				}
				catch (MySqlException ex)
				{
					Console.WriteLine("Error deleting account: " + ex.Message);
				}
			}
		}
	}
}
