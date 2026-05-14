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
## Console 

<img width="1090" height="635" alt="Image" src="https://github.com/user-attachments/assets/0fd3db5c-9ef8-4625-9990-9b972ce2cc6f" />
<img width="1059" height="623" alt="Image" src="https://github.com/user-attachments/assets/7dc91909-64c5-4bb9-a105-897a70653db1" />
<img width="1073" height="563" alt="Image" src="https://github.com/user-attachments/assets/84302c63-795b-4f6b-8652-357647bece23" />
<img width="1069" height="400" alt="Image" src="https://github.com/user-attachments/assets/d4cb512e-34bd-40d5-a101-b17bbcb3420a" />

## DataBase
<img width="849" height="120" alt="Image" src="https://github.com/user-attachments/assets/1ada2af7-e827-4254-9a60-a56d62587524" />
<img width="848" height="141" alt="Image" src="https://github.com/user-attachments/assets/5fac5c12-23a1-46a1-b459-24072b16ddb3" />