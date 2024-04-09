using CST_326.Models;
using CST_326.Models.ViewModel;
using CST_326.Services;

namespace CST_326.DAO
{
    public class UserRepository : IUserRepository<LoginViewModel>
    {
        public UserDAO userDAO = new UserDAO();

        public User EditUser(LoginViewModel user)
        {
            throw new NotImplementedException();
        }

        public User GetUser(LoginViewModel user)
        {
      
            return userDAO.FindUser(user);
        }

    }
}
