using System.Xml;
using NotificationSystemApplication.BLL.Senders;
using NotificationSystemApplication.BLL.Services;
using NotificationSystemApplication.Core.Models;
using NotificationSystemApplication.Core.CustomExceptions;

namespace NotificationSystemApplication.PL
{
    internal class Program
    {         
        UserService _userService;
        NotificationService _notificationService;

        MessageService _messageService;

        public Program()
        {   
            _messageService = new MessageService();
            _userService = new UserService();
            _notificationService = new NotificationService();
        }

        void Run()
        {
            Console.WriteLine("\nWelcome to Notification service:\n");
           
            while(true){

                Console.WriteLine("\nEnter your Preference:\n  1.Register [ new user ]\n  2.Login\n  3.Exit");
                string userPrefOption = Console.ReadLine()??"";

                if (userPrefOption == "1")
                     _RegisterUser();

                else if(userPrefOption=="2")
                {
                    try
                    {
                        Console.WriteLine("\n-------------------------------------       Login      ------------------------------------------\n");

                        User currentUser = _handleLogin();
                        Console.WriteLine($"\n-------------------------------------   Login successfully {currentUser.UserName}   --------------------------------\n");

                        while (true)
                        {
                            Console.WriteLine($"\nEnter your Preference {currentUser.UserName}:\n   1.Message Area \n   2.Edit Profile\n   3.Exit");
                            string loginUserPrefOption = Console.ReadLine()??"";

                            if(loginUserPrefOption == "1")
                                 _handleMessageOperation(currentUser);   
                          
                            else if(loginUserPrefOption == "2")
                                _handleUserProfileEdit(currentUser); 

                            else if (loginUserPrefOption == "3")
                                  break; 

                            else
                                Console.WriteLine("Please Enter valid option"); 
                        } 
                    }
                    catch(UserNotFoundException unf)
                    {
                        Console.WriteLine(unf.Message);
                    }
  
                }

                else if(userPrefOption=="3")
                    return;  

                else
                    Console.WriteLine("Please Enter valid option"); 
            }
        }

// ==================Support functions ================================================== 



        private void _RegisterUser()
        {
            Console.WriteLine("\n========================= Create Your Profile =============================\n");

            Console.Write("Please enter your full name : ");
            string userName = Console.ReadLine()??"";

            Console.WriteLine("");
            Console.Write("Please enter your EmailId: ");
            string userEmail = Console.ReadLine()??"";
            Console.WriteLine("");
            Console.Write("Please enter your PhoneNumber: ");
            string userPhoneNo = Console.ReadLine()??"";
            try{
                 User newUser = _userService.CreateUserProfile(userName, userEmail, userPhoneNo);
                 Console.WriteLine(newUser.ToString());
                 Console.WriteLine("\n============================== Thank You ==================================\n");

            }
            catch(Exception e)
            {
                Console.WriteLine($"Sorry, {e.Message}");
            }

        }


        private void _handleUserProfileEdit(User currentUser)
        {
            
            Console.WriteLine("\n========================= Edit Your Profile =============================\n");
            string currEmail = currentUser.Email;
            while(true)
            {
            Console.WriteLine("Please enter what you need to edit:\n    1.Name\n    2.Email\n    3.PhoneNo\n   4.Save & Exit\n");
            string editSection =  Console.ReadLine()??"";
            if(editSection =="1")
                {
                    Console.WriteLine($"\nCurrent Name: {currentUser.UserName}");
                    Console.Write("New Name : ");
                    string newName = Console.ReadLine()??"";
                    try
                    {
                        currentUser =_userService.EditUserName(currentUser,newName);
                        Console.WriteLine($"\n-----------\nUpdated Profile : {currentUser.ToString()}");
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine($"Sorry Unable to update, \n{e.Message}");
                    }
                }
            else if (editSection =="2")
                {
                    Console.WriteLine($"\nCurrent Email: {currentUser.Email}");
                    Console.Write("New Email : ");
                    string newEmail = Console.ReadLine()??"";
                    try
                    {
                        currentUser =_userService.EditUserEmail(currentUser,newEmail);
                        Console.WriteLine($"\n-----------\nUpdated Profile :  {currentUser.ToString()}");
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine($"Sorry Unable to update, \n{e.Message}");
                    }
                }
            else if (editSection =="3")
                {
                    {
                    Console.WriteLine($"\nCurrent Phone Number: {currentUser.PhoneNumber}");
                    Console.Write("New PhoneNo : ");
                    string newPhoneNumber = Console.ReadLine()??"";
                     try
                    {
                        currentUser =_userService.EditUserPhoneNo(currentUser,newPhoneNumber);
                        Console.WriteLine($"\n-----------\nUpdated Profile :  {currentUser.ToString()}");
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine($"Sorry Unable to update, \n{e.Message}");
                    }
                }
                }
            else if (editSection =="4")
                {
                    try{
                        _userService.UpdateUser(currEmail,currentUser);
                        Console.WriteLine("\n============================== Updated,Thank You ==================================\n");
                        currentUser.ToString();
                        return;
                    }
                    catch
                    {
                        Console.WriteLine("Sorry Unable to save the user");
                    }
                }
            else 
                {
                    Console.WriteLine("Enter correct option please...");
                }

            }

            
        }


        private User _handleLogin()
        {
            Console.Write("Please enter your email : ");
            string loginEmail = Console.ReadLine()??"";

            try{
            User user = _userService.GetUser(loginEmail);
            return user;
            }

            catch(Exception e)
            {
              Console.WriteLine($"Unable to login {e.Message}");
              throw new UserNotFoundException($"Can't find the user {loginEmail}");
            }
            
        }

        private void _handleMessageOperation(User currentUser)
        {
                    
            while (true)
            {
                Console.WriteLine("\nMessage Options: \n   1.Send New Message\n   2.View Sent Messages\n   3.Edit Message\n   4.Delete Message\n   5.Go Back");
                string msgOption = Console.ReadLine()??"";
                
                if(msgOption == "1") 
                    _handleSendMessage(currentUser);    

                else if(msgOption == "2") 
                     _handleViewSentMessage(currentUser);
                
                else if(msgOption == "3")
                     _handleEditMessage(currentUser);
                
                else if(msgOption == "4")
                     _handleDeleteMessage(currentUser);
                
                else if(msgOption == "5")
                     break;
                
                else
                     Console.WriteLine("Please Enter valid option");
                
            } 
        }
        
        private void _handleSendMessage(User currentUser)
        {
            Console.WriteLine("\n-------------------------Message area -----------------------\n");
            Console.WriteLine($"Sender : {currentUser.Email}");
           
            Console.Write("Please enter receiver Email : ");
            string receiverEmail = Console.ReadLine()??"";
            User? receiver;
            
            try {
                receiver= _userService.GetUser(receiverEmail);
                  
                Console.WriteLine("Do you need to push notification via: ");
                Console.WriteLine("\n   1.Email notification\n   2.SMS notification\n   3.Both");
                string userPrefNotification = Console.ReadLine()??"";
                string userMessage = "";
               
                while(true)  // prevent accident send without 0 characteres
                {
                    Console.WriteLine($"Please message for {receiver.UserName} :  ");
                    userMessage =  Console.ReadLine()??"";
                    try{
                      _notificationService.ProcessNotification(currentUser, receiver, userMessage, userPrefNotification);
                      break;
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine($"{e.Message}");
                        continue;
                    }

                }
              }
            
             catch(Exception e)
               {
                Console.WriteLine($"Unable to find user {e.Message}");
              }
        }

        private void _handleViewSentMessage(User currentUser)
        {
            var messages = _messageService.GetUserMessages(currentUser);
            Console.WriteLine("\n------------------------------------ Sent Messages ------------------------------------------\n");
            if(messages.Count == 0) Console.WriteLine("No messages found");
            foreach(var msg in messages)
            {
                Console.WriteLine($"{msg.ToString()}\n");
            }
        }

        private void _handleEditMessage(User currentUser)
        {
            Console.Write("Enter Message ID to edit : ");
            string editId = Console.ReadLine()??"";
            Console.Write("Enter new message content: ");
            string newContent = Console.ReadLine()??"";
            
            bool success = _messageService.EditMessage(currentUser, editId, newContent);
             if(success) {
                Console.WriteLine("Message updated successfully");
             }
            else 
            {
                Console.WriteLine("Message not found or update failed");
            }
        }

        private void _handleDeleteMessage(User currentUser)
        {
             Console.Write("Enter Message ID to delete : ");
            string delId = Console.ReadLine()??"";
            
            var deletedMsg = _messageService.DeleteMessage(currentUser, delId);
            if(deletedMsg != null) 
            {
                Console.WriteLine($"Message '{deletedMsg.MessageContent}' deleted successfully");
            }
            else Console.WriteLine("Message not found");
        }
        static void Main(string[] args)
        {
            new Program().Run();
        }
    }
}