using ChatHub.Domain.Entities;

namespace ChatHub.Global.Shared.Message
{
    public class MessageDeleteModel
    {
        public string DeleteType { get; set; }
        public Messages Message { get; set; }
        public Guid? DeletedUserId { get; set; }
    }
}
