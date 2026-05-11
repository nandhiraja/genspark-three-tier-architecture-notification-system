namespace NotificationSystem.CustomExceptions
{
    [System.Serializable]
    public class MessageNotFoundException : System.Exception
    {
        public MessageNotFoundException() { }
        public MessageNotFoundException(string message) : base(message) { }
        public MessageNotFoundException(string message, System.Exception inner) : base(message, inner) { }

    }
}