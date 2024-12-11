using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ChatHub.Global.Shared.ViewModel.UserViewModel;
public class RegisterRequestModel
{
    [Display(Name = "Họ")]
    public string? FirstName { get; set; }
    [Display(Name = "Tên")]
    public string? LastName { get; set; }
    [Display(Name = "Ngày sinh")]
    [DataType(DataType.Date)]
    public DateTime Dob { get; set; }
    [Display(Name = "Tên hiển thị")]
    public string? DisplayName { get; set; }
    [Display(Name = "Email")]
    public string? Email { get; set; }
    [Display(Name = "Số điện thoại")]
    public string? PhoneNumber { get; set; }
    [Display(Name = "Tên đăng nhập")]
    public string? UserName { get; set; }
    [Display(Name = "Mật khẩu")]
    [DataType(DataType.Password)]
    public string? PassWord { get; set; }
    [Display(Name = "Nhập lại mật khẩu")] 
    [DataType(DataType.Password)]
    public string? ConfirmPassWord { get; set; } 
    public IFormFile? File { get; set; }
}
