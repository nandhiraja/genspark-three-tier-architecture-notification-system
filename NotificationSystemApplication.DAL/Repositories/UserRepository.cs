using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;
using NotificationSystemApplication.Core.CustomExceptions;
using NotificationSystemApplication.Core.Interfaces;
using NotificationSystemApplication.Core.Models;

namespace NotificationSystemApplication.DAL.Repositories
{
    public class UserRepository: IUserRepository
    {
        static List<User> _userDataBase = new List<User>();

        public User AddUser(User user)
        {
           
            _userDataBase.Add(user);
            return user;
        }
        public User FindUserByEmail(string email)
        {   
           
            foreach(var currentUser in _userDataBase)
            {
                if(currentUser.Email == email)   // find user with user email
                {
                  return currentUser;   
                }
            }
            
            throw new UserNotFoundException($"User not found for email : {email}");
        }
         public User FindUserById(string id)
        {
            foreach(var currentUser in _userDataBase)
            {
                if(currentUser.Id == id)   // find user with user id
                {
                  return currentUser;   
                }
            }
            throw new UserNotFoundException($"User not found for Id : {id}");
        }

        public bool UpdateUser(string email, User newUpdateUser)
        {
            int length =_userDataBase.Count;

            for (int i =0;i<length;i++) // get the index of user in db to update
            {
                if(_userDataBase[i].Email==email)
                {
                    _userDataBase[i] = newUpdateUser;
                    return true;
                }
                
            }
            return false;
            
        }


        public User? DeleteUser(string email)
        {
            int length =_userDataBase.Count;

            for (int i =0;i<length;i++)
            {
                if(_userDataBase[i].Email==email)
                {   User currentUser = _userDataBase[i];
                    _userDataBase.RemoveAt(i);
                    return currentUser;
                }
                
            }
            throw new UserNotFoundException($"User not found for email : {email}, Unable to remove user");

            
        }
    }
}