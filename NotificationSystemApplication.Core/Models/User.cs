using System.Data.Common;

namespace NotificationSystemApplication.Core.Models
{
    public class User
    {   
        public int Id {get; set;}  
        public string UserName {get;set;} = string.Empty;
        public string Email {get;set;} = string.Empty;
        public string PhoneNumber {get;set;} = string.Empty;
        public DateTime CreateAt {get;set;} = DateTime.UtcNow;
        public virtual ICollection<Message> SentMessages { get; set; } = new List<Message>();
         public virtual ICollection<Message> ReceivedMessages { get; set; } = new List<Message>();

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