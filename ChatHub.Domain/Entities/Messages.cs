
namespace ChatHub.Domain.Entities
{
    public class Messages : BaseEntity
    {
        public string? Content {  get; set; }
        public Guid? SenderId { get; set; }
        public Guid? ReceiverId { get; set; }
        public DateTime MessageDate { get; set; }
        public bool IsSenderDeleted { get; set; }
        public bool IsReceiverDeleted { get; set; }
        public string? FileImageName { get; set; }
        public string ext { get; set; }
    }
}
