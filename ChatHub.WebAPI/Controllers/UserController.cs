using Azure.Core;
using ChatHub.Application.IService;
using ChatHub.Global.Shared.ViewModel.UserViewModel;
using Microsoft.AspNetCore.Mvc;

namespace ChatHub.WebAPI.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestModel loginRequestModel)
        {
            var user = await _userService.GetByUserName(loginRequestModel.UserName);
            var result = await _userService.AuthenCate(loginRequestModel);
            return Ok(new { result, user });
        }


        [HttpPost]
        public async Task<IActionResult> Register( RegisterRequestModel registerModel)
        {
    
            var result = await _userService.Register(registerModel);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUserChatWithUserLogin(Guid loginId)
        {
            var result = await _userService.GetAllUserChatWithUserLogin(loginId);
            return Ok(result);
        }


    }
}
