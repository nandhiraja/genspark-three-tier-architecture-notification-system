using NotificationSystemApplication.Core.Interfaces;
using NotificationSystemApplication.Core.Models;
using NotificationSystemApplication.DAL.Repositories;

namespace NotificationSystemApplication.BLL.Services
{
    public class MessageService 
    {
        MessageRepository _messageRepository = new MessageRepository();
          // Retrieves all messages for a user
        public List<Message> GetUserMessages(User currentUser)
        {
              return _messageRepository.GetUserMessages(currentUser);
        }

        // Edits existing message
        public bool EditMessage(User currentUser, int messageId, string newContent)
        {
            var messages = _messageRepository.GetUserMessages(currentUser);
            foreach(var msg in messages)
            {
                
                if(msg.MessageId == messageId)
                {
                    msg.MessageContent = newContent;
                    return _messageRepository.UpdateUserMessages(currentUser, msg);
                }
            }
           
           
            return false;
        }

        // Deletes an existing message
        public Message? DeleteMessage(User currentUser, int messageId)
        {
            var messages = _messageRepository.GetUserMessages(currentUser);
            foreach(var msg in messages)
            {
                if(msg.MessageId == messageId)
                {
                    return _messageRepository.DeleteUserMessages(currentUser, msg);
                }
            }
            return null;
        }
    }
}