using ChatHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChatHub.Infrastructures
{
    public class ChatDbContext : DbContext
    {
        public ChatDbContext(DbContextOptions<ChatDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ChatDbContext).Assembly);
        }

        public DbSet<User> Users { get; set; }

        public DbSet<ChatRooms> ChatRooms { get; set; }

        public DbSet<Messages> Messages { get; set; }
        
        public DbSet<MessageStatus> messageStatuses { get; set; }



    }
} 
