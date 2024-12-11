using ChatHub.Domain.Entities;
using ChatHub.Global.Shared.ViewModel.TokenViewModel;

namespace ChatHub.Application.IService
{
    public interface IJwtService
    {
        public TokenObjectModel GenerateToken(User user);
    }
}
