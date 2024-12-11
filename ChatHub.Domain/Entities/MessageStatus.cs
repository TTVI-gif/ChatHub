namespace ChatHub.Domain.Entities
{
    public class MessageStatus : BaseEntity
    {
        public bool? IsRead { get; set; }
        public Guid? userId { get; set; }
        public Guid? MessageId { get; set; }
        public DateTime? ReadAt { get; set; }
}
}
