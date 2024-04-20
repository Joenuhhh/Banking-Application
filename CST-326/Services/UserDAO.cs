using CST_326.DAO;
using CST_326.Models;
using CST_326.Models.ViewModel;
using MySql.Data.MySqlClient;
using NuGet.Protocol.Plugins;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;

namespace CST_326.Services
{
 
    public class UserDAO 
    {

        string myConnectionString = "Server=jonahmysqlserver.mysql.database.azure.com;Database=capstone;UserId=joenuh;Password=Jonah124;SslMode=Preferred;";

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
        public User FindUser(LoginViewModel user)
        {

            string sqlStatement = "SELECT * FROM users WHERE Username = @USERNAME and Password = @PASSWORD";

            using (MySqlConnection connection = new MySqlConnection(myConnectionString))
            {
                MySqlCommand command = new MySqlCommand(sqlStatement, connection);

                command.Parameters.AddWithValue("@USERNAME", user.UserName).Value = user.UserName;
                command.Parameters.AddWithValue("@PASSWORD", user.Password).Value = user.Password;
                // Add debugging to check the actual command text and parameters
                Console.WriteLine("1. Executing SQL: " + command.CommandText);
                try
                {
                    connection.Open();
                    Console.WriteLine("2. Executing SQL: " + command.CommandText);
                    MySqlDataReader reader = command.ExecuteReader();
                    Console.WriteLine("3. Executing SQL: " + command.CommandText);
                    while (reader.Read())
                    {
                        Console.WriteLine("4. Executing SQL: " + command.CommandText);
                        User a = new User()
                        {
                            UserId = reader.GetInt32(0),
                            FirstName = reader.GetString(1),
                            LastName = reader.GetString(2),
                            UserName = reader.GetString(3),
                            Email = reader.GetString(4),
                            PhoneNumber = reader.GetString(5),
                            Password = reader.GetString(6)
                        };
                        return a;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("_________________________________________________________");
                    Console.WriteLine(ex.StackTrace);
                };
            }
            return null;


        }

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

                if (rowsAffected > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
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

        // register a new user 
        public void RegisterUser(RegistrationViewModel newUser)
            {
            Console.WriteLine("testestest");
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

                // Print the SQL statement to the console
                Console.WriteLine("SQL Statement: " + sqlStatement);
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    Console.WriteLine("User registration successful.");
                }
                catch (MySqlException ex2)
                {
                    Console.WriteLine("||||||||||||||||||||" + ex2.Message);
                    Console.WriteLine("Error occurred during user registration: " + ex2.Message);
                    Console.WriteLine("Error code: " + ex2.ErrorCode); // This will print the MySQL error code
                    Console.WriteLine("Error number: " + ex2.Number); // This will print the MySQL error number


                };
            }
        }
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
                    Console.WriteLine("Final SQL query: " + insertQuery);

                    // Log parameters and their values
                    foreach (MySqlParameter parameter in command.Parameters)
                    {
                        Console.WriteLine($"{parameter.ParameterName}: {parameter.Value}");
                    }

                    command.ExecuteNonQuery();
                    Console.WriteLine("Account added successfully.");
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine("Error adding account: " + ex.Message);
                }
            }
        }

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
        public Account GetAccountById(int accountId)
        {
            string selectQuery = "SELECT * FROM accounts2 WHERE AccountId = @AccountId";

            using (MySqlConnection connection = new MySqlConnection(myConnectionString))
            {
                MySqlCommand command = new MySqlCommand(selectQuery, connection);
                command.Parameters.AddWithValue("@AccountId", accountId);

                try
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Account
                            {
                                AccountId = Convert.ToInt32(reader["AccountId"]),
                                UserId = Convert.ToInt32(reader["UserId"]),
                                AccountNumber = Convert.ToInt32(reader["AccountNumber"]),
                                AccountType = Convert.ToString(reader["AccountType"]),
                                Balance = Convert.ToDecimal(reader["Balance"]),
                                CreatedAt = Convert.ToDateTime(reader["CreationDate"])
                            };
                        }
                        else
                        {
                            return null; // Account not found
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine("Error retrieving account: " + ex.Message);
                    return null;
                }
            }
        }

        public void UpdateAccount(Account account)
        {
            string updateQuery = "UPDATE accounts2 SET Balance = @Balance WHERE AccountId = @AccountId";

            using (MySqlConnection connection = new MySqlConnection(myConnectionString))
            {
                MySqlCommand command = new MySqlCommand(updateQuery, connection);
                command.Parameters.AddWithValue("@Balance", account.Balance);
                command.Parameters.AddWithValue("@AccountId", account.AccountId);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    Console.WriteLine("Account updated successfully.");
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine("Error updating account: " + ex.Message);
                }
            }
        }


    }
}
