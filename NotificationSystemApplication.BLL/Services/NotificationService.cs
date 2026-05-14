using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices.Marshalling;
using NotificationSystemApplication.Core.CustomExceptions;
using NotificationSystemApplication.Core.Interfaces;
using NotificationSystemApplication.Core.Models;
using NotificationSystemApplication.DAL.Repositories;
using NotificationSystemApplication.BLL.Senders;

namespace NotificationSystemApplication.BLL.Services
{
    
    public class NotificationService
    {   
        static string _messageId = "1"; 
        
        private IUserRepository _userRepository;
        private IMessageRepository _messageRepository;
        private NotificationFactory _notificationFactory;
        private UserService _userService;

        /// <summary>
        /// Constructor injection to decouple concrete classes.
        /// </summary>
        public NotificationService()
        {   
            _userRepository =  new UserRepository();
            _messageRepository = new MessageRepository();
            var _emailSender = new EmailNotificationSender(_userRepository);
            var  _smsSender = new SMSNotificationSender(_userRepository);
            _notificationFactory = new NotificationFactory(_emailSender,_smsSender);
            _userService = new UserService();
        }

        /// <summary>
        /// Processes the notification by taking sender, receiver, and message content then sending via the chosen mode.
        /// </summary>
        public void ProcessNotification(User currentUser, User receiver, string userMessage, string userPrefNotification)
        {   
            Message newMessage = _SendNewMessage(currentUser, receiver, userMessage);
           

            var senders = _notificationFactory.GetNotificationSenders(userPrefNotification);
            
            if (senders.Count == 0)
            {
                Console.WriteLine("Enter valid input");
                return;
            }

            foreach (var sender in senders)
            {
                Message sendMsg = new Message(newMessage.SenderId, newMessage.ReceiverId, newMessage.MessageContent);
                
                if (sender is EmailNotificationSender) 
                {
                     sendMsg.NotificationMode = NotificationType.Email;
                }
                
                 else if (sender is SMSNotificationSender)
                {
                    sendMsg.NotificationMode = NotificationType.SMS;
                }
                try {
                    if (_ValidateUserMessage(sendMsg)) // validate user message
                    {
                     sender.Send(sendMsg);
                     _messageRepository.AddUserMessage(currentUser, sendMsg);
                    }
                }
                catch (NotificationNotSendException exp)
                {
                    Console.WriteLine($"Sorry , {exp.Message}");
                }
            

            }
        }
       
        /// <summary>
        /// Authenticates the user based on the provided email
        /// </summary>
        
  

        public Message _SendNewMessage(User sender, User receiver, string userMessage)
        {
            string messageId = _GenerateMessageId();
            Message newMessage = new Message(sender.Id,receiver.Id,userMessage);
            return newMessage;
        }

        public void PrintNotification(Message notificationMessage)
        {
            Console.WriteLine($"\n================ {notificationMessage.NotificationMode} Notification send Successfully ============================\n");
            Console.WriteLine($"Sender : {notificationMessage.SenderId}");
            Console.WriteLine($"Receiver : {notificationMessage.ReceiverId}");
            Console.WriteLine($"Message : {notificationMessage.MessageContent}");

            Console.WriteLine($"Date : {notificationMessage.Date}");
            Console.WriteLine($"\n================ ===================================================================== ============================\n");
        }

        string _GenerateMessageId()
        {
            long previousId = Convert.ToInt64(_messageId);
            string newId =  Convert.ToString(++previousId);
            _messageId = newId;
            return newId;
        }

         public bool _ValidateUserMessage(Message message)

        {   
            int messageLength = message.MessageContent.Length;   // business logic

            if(message.NotificationMode == NotificationType.SMS)
            {
                if( messageLength>5 && messageLength < 160)
                {
                    return true;   // sms satifies both 5 to 160 char
                }
                throw new NotificationNotSendException("Unable to send SMS notification, Message size should be 5 to 160 characters");
            }
            else // for email
            {
            if( messageLength>5 && messageLength < 160)
                {
                    return true;   //email satifies  5 char
                }
                throw new NotificationNotSendException("Unable to send Email notification, Message size should be minimun 5 characters");
            }

        }

     
    }
}