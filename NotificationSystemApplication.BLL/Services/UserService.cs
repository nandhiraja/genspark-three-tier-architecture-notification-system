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

            User newUser =  new User(userName,userEmail,userPhoneNo);
            
           
            return  _userRepository.AddUser(newUser);
           
            
           
            
        }

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
        public void UpdateUser(string email ,User user)
        {
            try
            {
                 _userRepository.UpdateUser(email,user);
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


        private bool _validateUserEmail(string email)
        {
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if(Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase))
                return true;
            throw new InvalidUserInputException("Invalid User email type, example email: examle@samlpe.com"); 
            
        }
        private bool _validateUserphoneNo(string PhoneNo)
        {
            string phoneNoPattern = @"[0-9]";
            if( Regex.IsMatch(PhoneNo,phoneNoPattern))
                return true;
            throw new InvalidUserInputException("Invalid User phoneNo type, example PhoneNo: 0987654321"); 

        }

        public User GetUser(string loginEmail)
        {   
            User? loginUser = _userRepository.FindUserByEmail(loginEmail);

            if(loginUser!=null)
            {
                return loginUser;
            }
            throw new UserNotFoundException($"Unable to find user, Entered email id {loginEmail}");
            }

        private void _reduceUserIdCount()
        {
            long previousId = Convert.ToInt64(_userId);
            string newId =  Convert.ToString(--previousId);
            _userId = newId;
        }
    }
}