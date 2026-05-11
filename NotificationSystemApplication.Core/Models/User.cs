using System.Data.Common;

namespace NotificationSystemApplication.Core.Models
{
    public class User
    {   
        public string Id {get; set;}  = string.Empty; 
        public string UserName {get;set;} = string.Empty;
        public string Email {get;set;} = string.Empty;
        public string PhoneNumber {get;set;} = string.Empty;
        public DateTime CreateAt {get;set;}

        public User()
        {
            
        }
        public User(string userName,string email, string phoneNumber)
        {   
            
            this.UserName =userName;
            this.Email = email;
            this.PhoneNumber = phoneNumber;
            this.CreateAt = DateTime.UtcNow;

        }
        public override string ToString()
        {
            return $"\nUserId : {Id}\nName : {UserName}\nEmail : {Email}\nPhoneNo : {PhoneNumber}\n";
        }

    }
}