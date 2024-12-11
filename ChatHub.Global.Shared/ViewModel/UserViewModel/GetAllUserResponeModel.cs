using Microsoft.AspNetCore.Mvc;

namespace ChatHub.Global.Shared.ViewModel.UserViewModel
{
    public class GetAllUserResponeModel
    {
        public Guid Id { get; set; }
        public string? Phone { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? DisplayName { get; set; }
        public string? Avartar { get; set;}
        public string? LastMessage { get; set; }
        public DateTime? ModifileDateMessage { get; set; }
        public int? TimeMessageSendd { get; set; }
        public bool? IsHour {  get; set; }
        public bool? IsDay { get; set; }
    }
}
