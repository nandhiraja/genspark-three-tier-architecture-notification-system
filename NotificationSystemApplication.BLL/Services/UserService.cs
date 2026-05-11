using System.ComponentModel;
using System.Diagnostics;
using System.Text.RegularExpressions;
using NotificationSystemApplication.Core.CustomExceptions;
using NotificationSystemApplication.Core.Interfaces;
using NotificationSystemApplication.Core.Models;
using NotificationSystemApplication.DAL.Repositories;
namespace NotificationSystemApplication.BLL.Services
{
    /// <summary>
    /// Service for handling user profiles.
    /// </summary>
    public class UserService
    {   
        private IUserRepository _userRepository ;
        static string _userId = "0";

        public UserService()
        {
            _userRepository = new UserRepository();;
        }

/// <summary>
/// This function helps to create a new user 
/// </summary>
/// <param name="userName"></param>
/// <param name="userEmail"></param>
/// <param name="userPhoneNo"></param>
/// <returns>User</returns>
/// <exception cref="InvalidUserInputException"></exception>
        public User CreateUserProfile(string userName,string userEmail,string userPhoneNo)
        {   
            string id = _GenerateUserId();

             if (!_validateUserEmail(userEmail))
            {                   
                _reduceUserIdCount();       // avoid unsuccessfull user create id generation   
                throw new InvalidUserInputException("Unable to add user due to invalid email input, eg email: example@sample.com"); 
            }
            if(!_validateUserphoneNo(userPhoneNo))
            {
                _reduceUserIdCount(); 
                throw new InvalidUserInputException("Unable to add user due to invalid phoneNo, eg PhNo: 1234567890 (10 numbers)");
            }

            User newUser =  new User(id,userName,userEmail,userPhoneNo);
            
            _userRepository.AddUser(newUser);
            return newUser;
           
            
           
            
        }

/// <summary>
/// This function is used to edit the existing user
/// </summary>
/// <param name="user"></param>
/// <param name="name"></param>
/// <returns></returns>
/// <exception cref="InvalidUserInputException"></exception>
        public User EditUserName(User user ,string name)
        {
            if (name != "")
            {
                user.UserName= name;
                return user;
            }
            throw new InvalidUserInputException("User name can't be empty");
        }
        public User EditUserPhoneNo(User user ,string PhoneNo)
        {
            try 
            {
                if (_validateUserphoneNo(PhoneNo))
                {
                    user.PhoneNumber = PhoneNo;
                    return user;
                }
                return user;

            }
            catch(InvalidUserInputException invalidInput)
            {
                throw new Exception($"{invalidInput}");
            }
        }
        public User EditUserEmail(User user ,string email)
        {
            try 
            {
                if (_validateUserEmail(email))
                {
                    user.Email = email;
                    return user;
                }
                return user;

            }
            catch(InvalidUserInputException invalidInput)
            {
                throw new Exception($"{invalidInput}");
            }

        }
        public void UpdateUser(User user)
        {
            try
            {
                 _userRepository.UpdateUser(user.Id,user);
            }
            catch(Exception e)
            {
                throw new Exception($"Unable to update, {e.Message}");
            }
        }
        private string _GenerateUserId()
        {
            long previousId = Convert.ToInt64(_userId);
            string newId =  Convert.ToString(++previousId);
            _userId = newId;
            return newId;
        }


/// <summary>
/// It is used to validate the user Email format
/// </summary>
/// <param name="email"></param>
/// <returns>bool</returns>
/// <exception cref="InvalidUserInputException"></exception>
        private bool _validateUserEmail(string email)
        {
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if(Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase))
                return true;
            throw new InvalidUserInputException("Invalid User email type, example email: examle@samlpe.com"); 
            
        }

/// <summary>
/// Vlaidate the user PhoneNo as per standard
/// </summary>
/// <param name="PhoneNumber"></param>
/// <returns>bool</returns>
/// <exception cref="InvalidUserInputException"></exception>
        private bool _validateUserphoneNo(string PhoneNo)
        {
            string phoneNoPattern = @"[0-9]";
            if( Regex.IsMatch(PhoneNo,phoneNoPattern))
                return true;
            throw new InvalidUserInputException("Invalid User phoneNo type, example PhoneNo: 0987654321"); 

        }
/// <summary>
/// This function user to get the user form df if exist
/// </summary>
/// <param name="loginEmail"></param>
/// <returns>User</returns>
/// <exception cref="UserNotFoundException"></exception>
        public User GetUser(string loginEmail)
        {   
            User? loginUser = _userRepository.FindUserByEmail(loginEmail);

            if(loginUser!=null)
            {
                return loginUser;
            }
            throw new UserNotFoundException($"Unable to find user, Entered email id {loginEmail}");
            }

/// <summary>
/// this function used to reduce the user id count incase of no user created
/// </summary>
        private void _reduceUserIdCount()
        {
            long previousId = Convert.ToInt64(_userId);
            string newId =  Convert.ToString(--previousId);
            _userId = newId;
        }
    }
}