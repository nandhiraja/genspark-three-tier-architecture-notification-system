using NotificationSystemApplication.Core.Models;

namespace NotificationSystemApplication.Core.Interfaces

{
    public interface INotification
    {
        public bool Send(Message message);

        public void PrintNotification(Message notificationMessage);


    }
}