namespace NotificationSystemApplication.Core.Models
{   
    public enum NotificationType{
        Email,
        SMS
    }
    public class Message
    {
        public string MessageId {get; set;} = string.Empty;
        public string SenderId {get; set;} = string.Empty;
        public string ReceiverId {get; set;} = string.Empty;
        public string MessageContent {get; set;} = string.Empty;
        public DateTime Date {get; set;}
        public NotificationType NotificationMode {get; set;}


        public Message()
        {
            Date = DateTime.UtcNow;
        }
        public Message(string   messageId, string senderId, string receiverId, string messageContent)
        {   
            this.MessageId  =messageId;
            this.SenderId =senderId;
            this.ReceiverId =receiverId;
            this.MessageContent=messageContent;
            Date = DateTime.UtcNow;
        }

        public override string ToString()
        {
            return $"MessageId :{MessageId} | SenderId :{SenderId} | Receiver : {ReceiverId}\nMessage: \n{MessageContent}\nDate : {Date} ";
        }
    }
}