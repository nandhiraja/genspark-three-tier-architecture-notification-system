using System;
using System.Collections.Generic;
using NotificationSystemApplication.Core.CustomExceptions;
using NotificationSystemApplication.Core.Interfaces;
using NotificationSystemApplication.Core.Models;
using Npgsql;

namespace NotificationSystemApplication.DAL.Repositories
{


    public class MessageRepository : IMessageRepository
    {
        private readonly string _connectionString;

        public MessageRepository()
        {   
            _connectionString = "Host=localhost;Port=5432;Database=NotificationAppDB;Username=nandhiraja;Password=";
        }




/// <summary>
/// This function is used to add new message that user send
/// </summary>
/// <param name="user"></param>
/// <param name="message"></param>

        public void AddUserMessage(User user, Message message)
        {
            const string insertQuery = "INSERT INTO messages (sender_id, receiver_id, message_content, notification_mode, send_date) VALUES (@senderId, @receiverId, @content, @mode, @date);";
            
            try
            {   
                using var connection = new NpgsqlConnection(_connectionString);
               
                using var command = new NpgsqlCommand(insertQuery, connection);

                command.Parameters.AddWithValue("@senderId", int.Parse(message.SenderId));
                command.Parameters.AddWithValue("@receiverId", int.Parse(message.ReceiverId));
                command.Parameters.AddWithValue("@content", message.MessageContent);
                command.Parameters.AddWithValue("@mode", message.NotificationMode.ToString());
                command.Parameters.AddWithValue("@date", message.Date.ToUniversalTime());

                connection.Open();
                int affectedRows = command.ExecuteNonQuery();
                if (affectedRows > 0)
                {
                   Console.WriteLine("DB: Message sent successfully");
                }
            }

            catch (Exception e)
            {
                throw new Exception($"Database Error: Failed to add message record. {e.Message}", e);
            }
        }





/// <summary>
/// This function is used to get all user send messages
/// </summary>
/// <param name="user"></param>
/// <returns> list of Mssages</returns>
        public List<Message> GetUserMessages(User user)
        {
            var userMessages = new List<Message>();
            const string selectQuery = "SELECT message_id, sender_id, receiver_id, message_content, notification_mode, send_date FROM messages WHERE sender_id = @senderId ORDER BY send_date DESC;";

            try
            {   
                using var connection = new NpgsqlConnection(_connectionString);
                using var command = new NpgsqlCommand(selectQuery, connection);
                
                command.Parameters.AddWithValue("@senderId", int.Parse(user.Id));

                connection.Open();
                using var reader = command.ExecuteReader();
                
                while (reader.Read())
                {
                    var message = new Message
                    {
                        MessageId = reader.GetInt32(0).ToString(),
                        SenderId = reader.GetInt32(1).ToString(),
                        ReceiverId = reader.GetInt32(2).ToString(),
                        MessageContent = reader.GetString(3),
                        NotificationMode = Enum.Parse<NotificationType>(reader.GetString(4)),
                        Date = reader.GetDateTime(5)
                    };
                    userMessages.Add(message);
                }
                return userMessages;
            }


            catch (Exception e)
            {
                throw new Exception($"Database Error: Failed to retrieve user chat records. {e.Message}", e);
            }
        }






/// <summary>
/// This function are used to update the user send message
/// </summary>
/// <param name="user"></param>
/// <param name="updatedMessage"></param>
/// <returns>true/false based on action done</returns>
        public bool UpdateUserMessages(User user, Message updatedMessage)
        {
            const string updateQuery = "UPDATE messages SET message_content = @content, notification_mode = @mode WHERE message_id = @messageId AND sender_id = @senderId;";

            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                 
                 using var command = new NpgsqlCommand(updateQuery, connection);

                command.Parameters.AddWithValue("@content", updatedMessage.MessageContent);
                command.Parameters.AddWithValue("@mode", updatedMessage.NotificationMode.ToString());
                command.Parameters.AddWithValue("@messageId", int.Parse(updatedMessage.MessageId));
                command.Parameters.AddWithValue("@senderId", int.Parse(user.Id));

                connection.Open();
                int affectedRows = command.ExecuteNonQuery();
                 return affectedRows > 0;
            }
            catch (Exception e)
            {
                throw new Exception($"Database Error: Failed to modify targeted message. {e.Message}", e);
            }
        }



          /// <summary>
          /// This function used to delete a message send by user
          /// </summary>
          /// <param name="user"></param>
          /// <param name="message"></param>
          /// <returns>deleted message</returns>
        public Message? DeleteUserMessages(User user, Message message)
        {
            const string selectQuery = "SELECT message_id, sender_id, receiver_id, message_content, notification_mode, send_date FROM messages WHERE message_id = @messageId AND sender_id = @senderId;";
            const string deleteQuery = "DELETE FROM messages WHERE message_id = @messageId AND sender_id = @senderId;";

            // validate ID before opening connection
            if (!int.TryParse(message.MessageId, out int targetMessageId) || !int.TryParse(user.Id, out int senderId))
            {
                throw new ArgumentException("Invalid ID format");
            }

            try
            {
                Message? messageToDelete = null;

                using var connection = new NpgsqlConnection(_connectionString);
                connection.Open();

                // Fetch record first
                using (var selectCommand = new NpgsqlCommand(selectQuery, connection))
                {
                    selectCommand.Parameters.AddWithValue("@messageId", targetMessageId);
                    selectCommand.Parameters.AddWithValue("@senderId", senderId);

                    using var reader = selectCommand.ExecuteReader();
                    if (reader.Read())
                    {
                        messageToDelete = new Message
                        {
                            MessageId = reader.GetInt32(0).ToString(),
                            SenderId = reader.GetInt32(1).ToString(),
                            ReceiverId = reader.GetInt32(2).ToString(),
                            MessageContent = reader.GetString(3),
                            NotificationMode = Enum.Parse<NotificationType>(reader.GetString(4)),
                            Date = reader.GetDateTime(5)
                        };
                    }
                } 
                if (messageToDelete == null) return null;
                // delete the message

                using (var deleteCommand = new NpgsqlCommand(deleteQuery, connection))
                {
                    deleteCommand.Parameters.AddWithValue("@messageId", targetMessageId);
                    deleteCommand.Parameters.AddWithValue("@senderId", senderId);

                    deleteCommand.ExecuteNonQuery();
                    
                    return messageToDelete;
                }
            }
            catch (Exception e)
            {
                throw new Exception($"DB Error: {e.Message}", e);
            }
        }       
    }
}
