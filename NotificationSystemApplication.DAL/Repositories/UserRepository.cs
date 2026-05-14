using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NotificationSystemApplication.Core.CustomExceptions;
using NotificationSystemApplication.Core.Interfaces;
using NotificationSystemApplication.Core.Models;
using NotificationSystemApplication.DAL.DBContext; 

namespace NotificationSystemApplication.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly NotificationDbContext _context;

        public UserRepository()
        {
           
            _context = new NotificationDbContext();
        }
        /// <summary>
        /// This function is used to add new user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public User AddUser(User user)
        {
            try
            {
                _context.Users.Add(user);
                _context.SaveChanges(); 
                
                return user; 
            }
            catch (Exception ex)
            {
                throw new Exception($"DB Error: Unable to insert user profile. {ex.Message}", ex);
            }
        }

        /// <summary>
        /// It is used to identify the user with registed Email id
        /// </summary>
        /// <param name="email"></param>
        /// <returns>User</returns>
        /// <exception cref="UserNotFoundException"></exception>
        public User FindUserByEmail(string email)
        {
            var user = _context.Users
                .FirstOrDefault(u => u.Email == email);

            if (user == null)
            {
                throw new UserNotFoundException($"No user record matched the email address: {email}");
            }

            return user;
        }
        /// <summary>
        /// This function is used to identify the user by user id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>User</returns>
        /// <exception cref="UserNotFoundException"></exception>
      
        public User FindUserById(int id)
        {
            var user = _context.Users.Find(id);

            if (user == null)
            {
                throw new UserNotFoundException($"No user record matched the user ID: {id}");
            }
            return user;
        }

        /// <summary>
        /// This function is used to update the user details [Name,Email,Phone No]
        /// </summary>
        /// <param name="email"></param>
        /// <param name="updatedUser"></param>
        /// <returns>Bool</returns>
        /// <exception cref="Exception"></exception>
        public bool UpdateUser(string email, User updatedUser)
        {
            try
            {
                var existingUser = _context.Users.FirstOrDefault(u => u.Email == email);
                
                if (existingUser == null) return false;

                existingUser.UserName = updatedUser.UserName;
                existingUser.Email = updatedUser.Email;
                existingUser.PhoneNumber = updatedUser.PhoneNumber;

                int affectedRows = _context.SaveChanges();
                return affectedRows > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"DB Error: could not execute update logic {ex.Message}", ex);
            }
        }
        /// <summary>
        /// This function is used to delete the user if found [if used not found return null]
        /// </summary>
        /// <param name="email"></param>
        /// <returns>User or null</returns>
        /// <exception cref="Exception"></exception>
        public User? DeleteUser(string email)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(u => u.Email == email);
                
                if (user == null) return null;

                _context.Users.Remove(user);
                _context.SaveChanges(); 
                
                return user;
            }
            catch (Exception ex)
            {
                throw new Exception($"DB Error: Unable to delete for email {email} {ex.Message}", ex);
            }
        }


    }
}