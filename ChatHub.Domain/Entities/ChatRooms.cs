namespace ChatHub.Domain.Entities
{
    public class ChatRooms : BaseEntity
    {
        public string? RoomName {  get; set; }
        public Guid? UserAdminId { get; set; }
        public virtual ICollection<User> Users { get; set; }    
    }
}
