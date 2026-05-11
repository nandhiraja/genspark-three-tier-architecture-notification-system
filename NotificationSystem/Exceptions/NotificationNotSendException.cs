namespace NotificationSystem.CustomExceptions
{
    public class NotificationNotSendException : System.Exception
    {
        public NotificationNotSendException() { }
        public NotificationNotSendException(string message) : base(message) { }
        public NotificationNotSendException(string message, System.Exception inner) : base(message, inner) { }

}
}