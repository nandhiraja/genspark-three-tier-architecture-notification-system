namespace NotificationSystem.CustomExceptions
{
    public class InvalidUserInputException : System.Exception
    {
        public InvalidUserInputException() { }
        public InvalidUserInputException(string message) : base(message) { }
        public InvalidUserInputException(string message, System.Exception inner) : base(message, inner) { }

    }
}