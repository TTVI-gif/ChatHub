using ChatHub.Domain.Entities;
using ChatHub.Global.Shared.Commons;
using ChatHub.Global.Shared.ViewModel.UserViewModel;
using Microsoft.AspNetCore.Http;


namespace ChatHub.Application.IService
{
    public interface IUserService
    {
        Task<ApiResult<string>> AuthenCate(LoginRequestModel request);
        Task<ApiResult<bool>> Register(RegisterRequestModel request);
        Task<ApiResult<User>> GetByID(Guid ID);
        Task<ApiResult<List<GetAllUserResponeModel>>> GetAllUserChatWithUserLogin(Guid loginId);
        Task<ApiResult<UserResponseModel>> GetByUserName(string userName);
    }
}
