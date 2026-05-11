```
3-Tire-NotificationSystem
│
├── NotificationSystemApplication.PL       [ Presentation Layer ]
│   └── Program.cs
│
├── NotificationSystemApplication.BLL      [ Business Logic Layer ]
│   |-Services/
|   |    |── NotificationService.cs
|   |    |── UserService.cs
|   |    └── MessageService.cs
|   └──Senders/
│           ├── EmailNotificationSender.cs
│           └── SMSNotificationSender.cs
│
├── NotificationSystemApplication.DAL       [ Data Access Layer ]
│      └── Repositries/      
|           └── NotificationRepository.cs
|           └── UserRepository.cs
│
└── NotificationSystemApplication.Core       [ Model/ Interface/ Exceptions]
    ├── Models/
    |        ├── User.cs
    |        └── Message.cs
    ├── Interfaces/
    |        ├── IUserRepository.cs
    |        └── IMessageRepository.cs
    |        └── INotification.cs
    └── Exceptions/
             ├── InvalidUserException.cs
             └── MessageNotFoundException.cs
             └── NotificationNotSendException.cs
             └── UserNotFoundException.cs
```