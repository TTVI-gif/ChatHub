namespace ChatHub.Domain.Entities
{
    public class User : BaseEntity
    {
        public string? Phone { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? UserName {  get; set; }
        public string? HashedPassword { get; set; }
        public string? Email { get; set; }
        public string? DisplayName {  get; set; }
        public string? Avartar {  get; set; }
        public DateTime? Dob { get; set; }
        public bool? IsLockedOut { get; set; }
        public string? Address { get; set; }
        public string? Province { get; set; }
        public string? Ward { get; set; }
        public string? City { get; set; }
        //public virtual ICollection<Messages> Messages { get; set; }
        //public virtual ICollection<ChatRooms> ChatRooms { get; set; }
    }
}
