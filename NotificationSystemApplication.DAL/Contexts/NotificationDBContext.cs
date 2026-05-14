using Microsoft.EntityFrameworkCore;
using NotificationSystemApplication.Core.Models;

namespace NotificationSystemApplication.DAL.DBContext
{
    public class NotificationDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("Host=localhost;Database=NotificationDbEF;Username=nandhiraja;Password=");
            }
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(u =>
            {
                u.HasKey(user => user.Id);
                
                u.Property(user => user.Id)
                 .UseIdentityByDefaultColumn(); 

                u.Property(user => user.UserName)
                 .IsRequired()
                 .HasMaxLength(100);
            });

            modelBuilder.Entity<Message>(m =>
            {
                m.HasKey(msg => msg.MessageId);

                m.Property(msg => msg.MessageId)
                 .UseIdentityByDefaultColumn();

                m.HasOne(msg => msg.Sender)
                 .WithMany(user => user.SentMessages)
                 .HasForeignKey(msg => msg.SenderId)
                 .OnDelete(DeleteBehavior.Restrict);

                m.HasOne(msg => msg.Receiver)
                 .WithMany(user => user.ReceivedMessages)
                 .HasForeignKey(msg => msg.ReceiverId)
                 .OnDelete(DeleteBehavior.Restrict);

                m.Property(msg => msg.NotificationMode)
                 .HasConversion<string>();
            });
        }
    }
}