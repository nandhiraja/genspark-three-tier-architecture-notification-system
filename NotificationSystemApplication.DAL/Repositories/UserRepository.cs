using System;
using System.Collections.Generic;
using NotificationSystemApplication.Core.CustomExceptions;
using NotificationSystemApplication.Core.Interfaces;
using NotificationSystemApplication.Core.Models;
using Npgsql;

namespace NotificationSystemApplication.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;

        public UserRepository()
        {   
            // local db connection string 
            _connectionString = "Host=localhost;Port=5432;Database=NotificationAppDB;Username=nandhiraja;Password=;";
        }

        /// <summary>
        /// This function used to find the new user 
        /// </summary>
        /// <param name="user"></param>
        /// <returns>User</returns>
        public User AddUser(User user)
        {   
            const string insertQuery = "INSERT INTO users (user_name, email, phone_number, create_at) VALUES (@name, @email, @phone, @date);";

            try
            {   // create a new connection
                using var connection = new NpgsqlConnection(_connectionString);   // using var to automaticaly close/clear resource at end of this section
                using var command = new NpgsqlCommand(insertQuery, connection);

                command.Parameters.AddWithValue("@name", user.UserName);
                command.Parameters.AddWithValue("@email", user.Email);
                command.Parameters.AddWithValue("@phone", user.PhoneNumber);
                command.Parameters.AddWithValue("@date", user.CreateAt.ToUniversalTime()); 

                connection.Open();
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception($"DB Error: Unable to insert user profile. {ex.Message}", ex);
            }

            return FindUserByEmail(user.Email);  // get the newly registered user with id
        }
        /// <summary>
        /// This function used to find the exist user  in db by Email
        /// </summary>
        /// <param name="email"></param>
        /// <returns>User</returns>
        public User FindUserByEmail(string email)
        {   
            const string selectQuery = "SELECT id, user_name, email, phone_number, create_at FROM users WHERE email = @email;";

            using var connection = new NpgsqlConnection(_connectionString);
            using var command = new NpgsqlCommand(selectQuery, connection);
            
            command.Parameters.AddWithValue("@email", email);
            connection.Open();
            
            using var reader = command.ExecuteReader();

            if (!reader.HasRows)    // check any user has returned 
            {
               
                throw new UserNotFoundException($"No user record matched the email address: {email}");  
            }
            //  execute when any user has fetched
            var user = new User();
            while (reader.Read()) 
            {
              
                user.Id = reader.GetInt32(0).ToString(); 
                user.UserName = reader.GetString(1);
                user.Email = reader.GetString(2);
                user.PhoneNumber = reader.IsDBNull(3) ? string.Empty : reader.GetString(3);
                user.CreateAt = reader.GetDateTime(4);
           
            } 
            return user;
        }
        /// <summary>
        /// This function used to find the exist user in db by id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>User</returns>
        public User FindUserById(string id)
        {
            const string selectQuery = "SELECT id, user_name, email, phone_number, create_at FROM users WHERE id = @id;";

            if (!int.TryParse(id, out int numericId))
            {
                throw new ArgumentException("The provided ID is not a valid numeric identifier.", nameof(id));
            }

            using var connection = new NpgsqlConnection(_connectionString);
            using var command = new NpgsqlCommand(selectQuery, connection);
            
            command.Parameters.AddWithValue("@id", numericId);
           
           
            connection.Open();
            
            using var reader = command.ExecuteReader();

            if (!reader.HasRows)
            {
                throw new UserNotFoundException($"No user record matched the user ID: {id}");
            }

            var user = new User();
            while (reader.Read())
            {

                user.Id = reader.GetInt32(0).ToString();
                user.UserName = reader.GetString(1);
                
                user.Email = reader.GetString(2);
                user.PhoneNumber = reader.IsDBNull(3) ? string.Empty : reader.GetString(3);
                user.CreateAt = reader.GetDateTime(4);
            } 
            return user;
        }

/// <summary>
/// This function used to update the existing user 
/// </summary>
/// <param name="email"></param>
/// <param name="updatedUser"></param>
/// <returns>bool</returns>
        
        public bool UpdateUser(string email, User updatedUser)
        {   
            
            const string updateQuery = "UPDATE users SET user_name = @name, email = @newEmail, phone_number = @phone WHERE email = @oldEmail;";

            try
            {   
                using var connection = new NpgsqlConnection(_connectionString);
               
                using var command = new NpgsqlCommand(updateQuery, connection);

                command.Parameters.AddWithValue("@name", updatedUser.UserName);
                command.Parameters.AddWithValue("@newEmail", updatedUser.Email); 
                command.Parameters.AddWithValue("@phone", updatedUser.PhoneNumber);
                command.Parameters.AddWithValue("@oldEmail", email);

                connection.Open();
                int affectedRows = command.ExecuteNonQuery();
                 return affectedRows > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"DB Error: could not execute update logic {ex.Message}", ex);
            }
        }
/// <summary>
/// This function is used to delete the existing user 
/// </summary>
/// <param name="email"></param>
/// <returns>user</returns>
        public User? DeleteUser(string email)
        {
            User user;
            try 
            {
                user = FindUserByEmail(email);    // check the user if register or not
            }
            catch (UserNotFoundException)
             {
                return null; 
            }

            const string deleteQuery = "DELETE FROM users WHERE email = @email;";

            try
            {   
                using var connection = new NpgsqlConnection(_connectionString);
               
                using var command = new NpgsqlCommand(deleteQuery, connection);

                command.Parameters.AddWithValue("@email", email);
                connection.Open();
                
                int affectedRows = command.ExecuteNonQuery();
                 return affectedRows > 0 ? user : null;    // check if user has deleted or not
            }
            catch (Exception ex)
            {
                throw new Exception($"DB Error: Unable to delete for email {email}. {ex.Message}", ex);
            }
        }
    }
}
