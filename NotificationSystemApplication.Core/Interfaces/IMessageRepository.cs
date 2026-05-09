using NotificationSystemApplication.Core.Models;

namespace NotificationSystemApplication.Core.Interfaces
{
    // Interface for Message Repository
    public interface IMessageRepository
    {
        public void AddUserMessage(User user, Message message);
        public List<Message> GetUserMessages(User user);
        public bool UpdateUserMessages(User user, Message updatedMessage);
        public Message? DeleteUserMessages(User user , Message message);
    }
}