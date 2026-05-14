using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NotificationSystemApplication.Core.Interfaces;
using NotificationSystemApplication.Core.Models;
using NotificationSystemApplication.DAL.DBContext;

namespace NotificationSystemApplication.DAL.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly NotificationDbContext _context;

        public MessageRepository()
        {
            // Initializing the context directly
            _context = new NotificationDbContext();
        }

         /// <summary>
         /// Adds a new message 
         /// </summary>
         /// <param name="user"></param>
         /// <param name="message"></param>
         /// <exception cref="Exception"></exception>
        public void AddUserMessage(User user, Message message)
        {
            try
            {
               
                _context.Messages.Add(message);
                int affectedRows = _context.SaveChanges();
                
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
        /// This function is used to retrieves user messages ordered by date descending 
        /// </summary>
        /// <param name="user"></param>
        /// <returns>List of messages</returns>
        /// <exception cref="Exception"></exception>
        
        public List<Message> GetUserMessages(User user)
        {
            try
            {
                return _context.Messages
                    .Where(m => m.SenderId == user.Id)
                    .OrderByDescending(m => m.Date)
                    .ToList();
            }
            catch (Exception e)
            {
                throw new Exception($"DB Error: Failed to retrieve user chat, {e.Message}", e);
            }
        }

        /// <summary>
        /// Updates an existing message content and notification mode.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="updatedMessage"></param>
        /// <returns>True or False</returns>
        /// <exception cref="Exception"></exception>
        public bool UpdateUserMessages(User user, Message updatedMessage)
        {
            try
            {
                var existingMessage = _context.Messages
                    .FirstOrDefault(m => m.MessageId == updatedMessage.MessageId && m.SenderId == user.Id);

                if (existingMessage == null) return false;

                existingMessage.MessageContent = updatedMessage.MessageContent;
                existingMessage.NotificationMode = updatedMessage.NotificationMode;

                return _context.SaveChanges() > 0;
            }
            catch (Exception e)
            {
                throw new Exception($"DB Error: Failed to modify message. {e.Message}", e);
            }
        }
        /// <summary>
        /// Deletes a message
        /// </summary>
        /// <param name="user"></param>
        /// <param name="message"></param>
        /// <returns>Message - which is deleted</returns>
        /// <exception cref="Exception"></exception>
        public Message? DeleteUserMessages(User user, Message message)
        {
            try
            {
                // Find the message 
                var messageToDelete = _context.Messages
                    .FirstOrDefault(m => m.MessageId == message.MessageId && m.SenderId == user.Id);

                if (messageToDelete == null) return null;

                _context.Messages.Remove(messageToDelete);
                _context.SaveChanges();
                
                return messageToDelete;
            }
            catch (Exception e)
            {
                throw new Exception($"DB Error: {e.Message}", e);
            }
        }
    }
}