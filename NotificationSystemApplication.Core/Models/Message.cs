namespace NotificationSystemApplication.Core.Models
{   
    public enum NotificationType{
        Email,
        SMS
    }
    public class Message
    {
        public int MessageId {get; set;} 
        public int SenderId {get; set;} 
        public int ReceiverId {get; set;} 
        public string MessageContent {get; set;} = string.Empty;
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public NotificationType NotificationMode {get; set;}

        public virtual User Sender { get; set; } = null!;
    
        public virtual User Receiver { get; set; } = null!;
        public Message()
        {
        }
        public Message(int senderId, int receiverId, string messageContent)
        {   
            this.SenderId = senderId;
            this.ReceiverId = receiverId;
            this.MessageContent = messageContent;
            this.Date = DateTime.UtcNow;
        }

        public override string ToString()
        {
            return $"Notification Mode : {NotificationMode}\nMessageId :{MessageId} | SenderId :{SenderId} | Receiver : {ReceiverId}\nMessage: \n{MessageContent}\nDate : {Date} ";
        }
    }
}