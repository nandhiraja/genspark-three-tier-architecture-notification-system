using NotificationSystemApplication.Core.Models;

namespace NotificationSystemApplication.Core.Interfaces
{
    // Interface for User Repository
    public interface IUserRepository
    {
        public User AddUser(User user);
        public User? FindUserByEmail(string email);
        public User? FindUserById(string id);
        public bool UpdateUser(string email, User newUpdateUser);
        public User? DeleteUser(string email);

        
    }
}