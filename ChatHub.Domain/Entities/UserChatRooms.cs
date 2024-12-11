namespace ChatHub.Domain.Entities
{
    public class UserChatRooms : BaseEntity
    {
        public Guid? userId { get; set; }
        public Guid? ChatRoomId { get; set; }
        public User User { get; set; }
        public ChatRooms ChatRooms { get; set; }

    }
}
